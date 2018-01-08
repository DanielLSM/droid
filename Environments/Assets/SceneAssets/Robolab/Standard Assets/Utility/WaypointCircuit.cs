using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityStandardAssets.Utility {
  public class WaypointCircuit : MonoBehaviour {
    float[] distances;

    public float editorVisualisationSubsteps = 100;

    float i;
    int numPoints;
    Vector3 P0;

    //this being here will save GC allocs
    int p0n;
    Vector3 P1;
    int p1n;
    Vector3 P2;
    int p2n;
    Vector3 P3;
    int p3n;
    Vector3[] points;

    [SerializeField] bool smoothRoute = true;

    public WaypointList waypointList = new WaypointList();

    public float Length { get; private set; }

    public Transform[] Waypoints { get { return this.waypointList.items; } }

    // Use this for initialization
    void Awake() {
      if (this.Waypoints.Length > 1) this.CachePositionsAndDistances();
      this.numPoints = this.Waypoints.Length;
    }

    public RoutePoint GetRoutePoint(float dist) {
      // position and direction
      var p1 = this.GetRoutePosition(dist);
      var p2 = this.GetRoutePosition(dist + 0.1f);
      var delta = p2 - p1;
      return new RoutePoint(p1, delta.normalized);
    }

    public Vector3 GetRoutePosition(float dist) {
      var point = 0;

      if (this.Length == 0) this.Length = this.distances[this.distances.Length - 1];

      dist = Mathf.Repeat(dist, this.Length);

      while (this.distances[point] < dist)
        ++point;

      // get nearest two points, ensuring points wrap-around start & end of circuit
      this.p1n = (point - 1 + this.numPoints) % this.numPoints;
      this.p2n = point;

      // found point numbers, now find interpolation value between the two middle points

      this.i = Mathf.InverseLerp(this.distances[this.p1n], this.distances[this.p2n], dist);

      if (this.smoothRoute) {
        // smooth catmull-rom calculation between the two relevant points

        // get indices for the surrounding 2 points, because
        // four points are required by the catmull-rom function
        this.p0n = (point - 2 + this.numPoints) % this.numPoints;
        this.p3n = (point + 1) % this.numPoints;

        // 2nd point may have been the 'last' point - a dupe of the first,
        // (to give a value of max track distance instead of zero)
        // but now it must be wrapped back to zero if that was the case.
        this.p2n = this.p2n % this.numPoints;

        this.P0 = this.points[this.p0n];
        this.P1 = this.points[this.p1n];
        this.P2 = this.points[this.p2n];
        this.P3 = this.points[this.p3n];

        return this.CatmullRom(this.P0, this.P1, this.P2, this.P3, this.i);
      }
      // simple linear lerp between the two points:

      this.p1n = (point - 1 + this.numPoints) % this.numPoints;
      this.p2n = point;

      return Vector3.Lerp(this.points[this.p1n], this.points[this.p2n], this.i);
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i) {
      // comments are no use here... it's the catmull-rom equation.
      // Un-magic this, lord vector!
      return 0.5f
             * (2 * p1
                + (-p0 + p2) * i
                + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i
                + (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }

    void CachePositionsAndDistances() {
      // transfer the position of each point and distances between points to arrays for
      // speed of lookup at runtime
      this.points = new Vector3[this.Waypoints.Length + 1];
      this.distances = new float[this.Waypoints.Length + 1];

      float accumulate_distance = 0;
      for (var i = 0; i < this.points.Length; ++i) {
        var t1 = this.Waypoints[i % this.Waypoints.Length];
        var t2 = this.Waypoints[(i + 1) % this.Waypoints.Length];
        if (t1 != null && t2 != null) {
          var p1 = t1.position;
          var p2 = t2.position;
          this.points[i] = this.Waypoints[i % this.Waypoints.Length].position;
          this.distances[i] = accumulate_distance;
          accumulate_distance += (p1 - p2).magnitude;
        }
      }
    }

    void OnDrawGizmos() { this.DrawGizmos(false); }

    void OnDrawGizmosSelected() { this.DrawGizmos(true); }

    void DrawGizmos(bool selected) {
      this.waypointList.circuit = this;
      if (this.Waypoints.Length > 1) {
        this.numPoints = this.Waypoints.Length;

        this.CachePositionsAndDistances();
        this.Length = this.distances[this.distances.Length - 1];

        Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);
        var prev = this.Waypoints[0].position;
        if (this.smoothRoute) {
          for (float dist = 0; dist < this.Length; dist += this.Length / this.editorVisualisationSubsteps) {
            var next = this.GetRoutePosition(dist + 1);
            Gizmos.DrawLine(prev, next);
            prev = next;
          }

          Gizmos.DrawLine(prev, this.Waypoints[0].position);
        } else {
          for (var n = 0; n < this.Waypoints.Length; ++n) {
            var next = this.Waypoints[(n + 1) % this.Waypoints.Length].position;
            Gizmos.DrawLine(prev, next);
            prev = next;
          }
        }
      }
    }

    [Serializable]
    public class WaypointList {
      public WaypointCircuit circuit;
      public Transform[] items = new Transform[0];
    }

    public struct RoutePoint {
      public Vector3 position;
      public Vector3 direction;

      public RoutePoint(Vector3 position, Vector3 direction) {
        this.position = position;
        this.direction = direction;
      }
    }
  }
}

namespace UnityStandardAssets.Utility.Inspector {
  #if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(WaypointCircuit.WaypointList))]
  public class WaypointListDrawer : PropertyDrawer {
    readonly float lineHeight = 18;
    readonly float spacing = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(position, label, property);

      var x = position.x;
      var y = position.y;
      var inspectorWidth = position.width;

      // Draw label

      // Don't make child fields be indented
      var indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;

      var items = property.FindPropertyRelative("items");
      var titles = new[] {"Transform", "", "", ""};
      var props = new[] {"transform", "^", "v", "-"};
      var widths = new[] {.7f, .1f, .1f, .1f};
      float lineHeight = 18;
      var changedLength = false;
      if (items.arraySize > 0) {
        for (var i = -1; i < items.arraySize; ++i) {
          var item = items.GetArrayElementAtIndex(i);

          var rowX = x;
          for (var n = 0; n < props.Length; ++n) {
            var w = widths[n] * inspectorWidth;

            // Calculate rects
            var rect = new Rect(rowX, y, w, lineHeight);
            rowX += w;

            if (i == -1)
              EditorGUI.LabelField(rect, titles[n]);
            else {
              if (n == 0)
                EditorGUI.ObjectField(rect, item.objectReferenceValue, typeof(Transform), true);
              else {
                if (GUI.Button(rect, props[n])) {
                  switch (props[n]) {
                    case "-":
                      items.DeleteArrayElementAtIndex(i);
                      items.DeleteArrayElementAtIndex(i);
                      changedLength = true;
                      break;
                    case "v":
                      if (i > 0)
                        items.MoveArrayElement(i, i + 1);
                      break;
                    case "^":
                      if (i < items.arraySize - 1)
                        items.MoveArrayElement(i, i - 1);
                      break;
                  }
                }
              }
            }
          }

          y += lineHeight + this.spacing;
          if (changedLength)
            break;
        }
      } else {
        // add button
        var addButtonRect = new Rect(
            x + position.width - widths[widths.Length - 1] * inspectorWidth,
            y,
            widths[widths.Length - 1] * inspectorWidth,
            lineHeight);
        if (GUI.Button(addButtonRect, "+"))
          items.InsertArrayElementAtIndex(items.arraySize);

        y += lineHeight + this.spacing;
      }

      // add all button
      var addAllButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
      if (GUI.Button(addAllButtonRect, "Assign using all child objects")) {
        var circuit = property.FindPropertyRelative("circuit").objectReferenceValue as WaypointCircuit;
        var children = new Transform[circuit.transform.childCount];
        var n = 0;
        foreach (Transform child in circuit.transform)
          children[n++] = child;
        Array.Sort(children, new TransformNameComparer());
        circuit.waypointList.items = new Transform[children.Length];
        for (n = 0; n < children.Length; ++n)
          circuit.waypointList.items[n] = children[n];
      }

      y += lineHeight + this.spacing;

      // rename all button
      var renameButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
      if (GUI.Button(renameButtonRect, "Auto Rename numerically from this order")) {
        var circuit = property.FindPropertyRelative("circuit").objectReferenceValue as WaypointCircuit;
        var n = 0;
        foreach (var child in circuit.waypointList.items)
          child.name = "Waypoint " + n++.ToString("000");
      }

      y += lineHeight + this.spacing;

      // Set indent back to what it was
      EditorGUI.indentLevel = indent;
      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var items = property.FindPropertyRelative("items");
      var lineAndSpace = this.lineHeight + this.spacing;
      return 40 + items.arraySize * lineAndSpace + lineAndSpace;
    }

    // comparer for check distances in ray cast hits
    public class TransformNameComparer : IComparer {
      public int Compare(object x, object y) { return ((Transform)x).name.CompareTo(((Transform)y).name); }
    }
  }
  #endif
}
