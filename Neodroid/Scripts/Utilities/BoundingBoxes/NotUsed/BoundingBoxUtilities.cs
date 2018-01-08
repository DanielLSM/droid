using UnityEngine;

namespace Neodroid.Scripts.Utilities.BoundingBoxes.NotUsed {
  public static class Utilities {
    public static void DrawBoxFromCenter(Vector3 p, float r, Color c) {
      // p is pos.yition of the center, r is "radius" and c is the color of the box
      //Bottom lines
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : -r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : -r + p.y,
                                       z : -r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : -r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : -r + p.y,
                                       z : r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : -r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : -r + p.y,
                                       z : r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : -r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : -r + p.y,
                                       z : -r + p.z),
                     color : c);

      //Vertical lines
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : r + p.y,
                                       z : -r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : r + p.y,
                                       z : r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : r + p.y,
                                       z : r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : r + p.y,
                                       z : -r + p.z),
                     color : c);

      //Top lines
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : -r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : r + p.y,
                                       z : -r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -r + p.x,
                                         y : -r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : -r + p.x,
                                       y : r + p.y,
                                       z : r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : -r + p.y,
                                         z : -r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : r + p.y,
                                       z : -r + p.z),
                     color : c);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : r + p.x,
                                         y : -r + p.y,
                                         z : r + p.z),
                     end : new Vector3(
                                       x : r + p.x,
                                       y : r + p.y,
                                       z : r + p.z),
                     color : c);
    }

    public static void RegisterCollisionTriggerCallbacksOnChildren(
      Transform transform,
      ChildSensor.OnChildCollisionEnterDelegate OnCollisionEnterChild,
      ChildSensor.OnChildTriggerEnterDelegate OnTriggerEnterChild,
      ChildSensor.OnChildCollisionExitDelegate OnCollisionExitChild,
      ChildSensor.OnChildTriggerExitDelegate OnTriggerExitChild,
      bool debug = false) {
      var childrenWithColliders =
        transform.GetComponentsInChildren<Collider>(includeInactive : transform.gameObject);

      foreach (var child in childrenWithColliders) {
        var child_sensor = child.gameObject.AddComponent<ChildSensor>();
        child_sensor.OnCollisionEnterDelegate = OnCollisionEnterChild;
        child_sensor.OnTriggerEnterDelegate = OnTriggerEnterChild;
        child_sensor.OnCollisionExitDelegate = OnCollisionExitChild;
        child_sensor.OnTriggerExitDelegate = OnTriggerExitChild;
        //Debug.Log(transform.name + " has " + child_sensor.name + " registered");
      }
    }

    public static void DrawRect(float x_size, float y_size, float z_size, Vector3 pos, Color color) {
      var x = x_size / 2;
      var y = y_size / 2;
      var z = z_size / 2;

      //Vertical lines
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : -y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : -x + pos.x,
                                       y : y + pos.y,
                                       z : -z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : x + pos.x,
                                         y : -y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : y + pos.y,
                                       z : -z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : -y + pos.y,
                                         z : z + pos.z),
                     end : new Vector3(
                                       x : -x + pos.x,
                                       y : y + pos.y,
                                       z : z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : x + pos.x,
                                         y : -y + pos.y,
                                         z : z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : y + pos.y,
                                       z : z + pos.z),
                     color : color);

      //Horizontal top
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : y + pos.y,
                                       z : -z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : y + pos.y,
                                         z : z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : y + pos.y,
                                       z : z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : -x + pos.x,
                                       y : y + pos.y,
                                       z : z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : x + pos.x,
                                         y : y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : y + pos.y,
                                       z : z + pos.z),
                     color : color);

      //Horizontal bottom
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : -y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : -y + pos.y,
                                       z : -z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : -y + pos.y,
                                         z : z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : -y + pos.y,
                                       z : z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : -x + pos.x,
                                         y : -y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : -x + pos.x,
                                       y : -y + pos.y,
                                       z : z + pos.z),
                     color : color);
      Debug.DrawLine(
                     start : new Vector3(
                                         x : x + pos.x,
                                         y : -y + pos.y,
                                         z : -z + pos.z),
                     end : new Vector3(
                                       x : x + pos.x,
                                       y : -y + pos.y,
                                       z : z + pos.z),
                     color : color);
    }

    public static bool DidTransformsChange(
      Transform[] old_transforms,
      Transform[] newly_acquired_transforms) {
      if (old_transforms.Length != newly_acquired_transforms.Length) return true;

      var i = 0;
      foreach (var old in old_transforms) {
        if (old.position != newly_acquired_transforms[i].position
            || old.rotation != newly_acquired_transforms[i].rotation)
          return true;
        i++;
      }

      return false;
    }

    public static Bounds GetTotalMeshFilterBounds(Transform objectTransform) {
      var meshFilter = objectTransform.GetComponent<MeshFilter>();

      var result = meshFilter != null ? meshFilter.mesh.bounds : new Bounds();

      foreach (Transform transform in objectTransform) {
        var bounds = GetTotalMeshFilterBounds(objectTransform : transform);
        result.Encapsulate(point : bounds.min);
        result.Encapsulate(point : bounds.max);
      }

      /*var bounds1 = GetTotalColliderBounds(objectTransform);
      result.Encapsulate(bounds1.min);
      result.Encapsulate(bounds1.max);
      */
      /*
            foreach (Transform transform in objectTransform) {
              var bounds = GetTotalColliderBounds(transform);
              result.Encapsulate(bounds.min);
              result.Encapsulate(bounds.max);
            }
            */
      var scaledMin = result.min;
      scaledMin.Scale(scale : objectTransform.localScale);
      result.min = scaledMin;
      var scaledMax = result.max;
      scaledMax.Scale(scale : objectTransform.localScale);
      result.max = scaledMax;
      return result;
    }

    public static Bounds GetTotalColliderBounds(Transform objectTransform) {
      var meshFilter = objectTransform.GetComponent<Collider>();

      var result = meshFilter != null ? meshFilter.bounds : new Bounds();

      foreach (Transform transform in objectTransform) {
        var bounds = GetTotalColliderBounds(objectTransform : transform);
        result.Encapsulate(point : bounds.min);
        result.Encapsulate(point : bounds.max);
      }

      var scaledMin = result.min;
      scaledMin.Scale(scale : objectTransform.localScale);
      result.min = scaledMin;
      var scaledMax = result.max;
      scaledMax.Scale(scale : objectTransform.localScale);
      result.max = scaledMax;
      return result;
    }

    public static Bounds GetMaxBounds(GameObject g) {
      var b = new Bounds(
                         center : g.transform.position,
                         size : Vector3.zero);
      foreach (var r in g.GetComponentsInChildren<Renderer>()) b.Encapsulate(bounds : r.bounds);
      return b;
    }
  }

  public class Pair<T1, T2> {
    internal Pair(T1 first, T2 second) {
      this.First = first;
      this.Second = second;
    }

    public T1 First { get; private set; }

    public T2 Second { get; private set; }
  }

  public static class Pair {
    public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second) {
      var tuple = new Pair<T1, T2>(
                                   first : first,
                                   second : second);
      return tuple;
    }
  }
}
