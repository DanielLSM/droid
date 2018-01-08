using System.Collections.Generic;
using Neodroid.Utilities.BoundingBoxes;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {
    Vector3 _bottom_back_left;
    Vector3 _bottom_back_right;
    Vector3 _bottom_front_left;
    Vector3 _bottom_front_right;

    [HideInInspector] public Bounds _bounds;

    [HideInInspector] public Vector3 _bounds_offset;

    public DrawBoundingBoxOnCamera _camera;
    Collider[] _children_colliders;

    MeshFilter[] _children_meshes;

    public bool _collider_based;

    Vector3[] _corners;
    public bool _freeze = true;

    public bool _include_children = true;

    // Vector3 startingBoundSize;
    // Vector3 startingBoundCenterLocal;
    Vector3 _last_position;
    Quaternion _last_rotation;

    [HideInInspector]
    //public Vector3 startingScale;
    Vector3 _last_scale;

    public Color _line_color = new Color (
                                 r : 0f,
                                 g : 1f,
                                 b : 0.4f,
                                 a : 0.74f);

    Vector3[,] _lines;

    Quaternion _rotation;

    public bool _setup_on_awake = true;
    Vector3 _top_back_left;
    Vector3 _top_back_right;

    Vector3 _top_front_left;
    Vector3 _top_front_right;

    public Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
          this._top_front_left,
          this._top_front_right,
          this._top_back_left,
          this._top_back_right,
          this._bottom_front_left,
          this._bottom_front_right,
          this._bottom_back_left,
          this._bottom_back_right
        };
      }
    }

    public Bounds Bounds { get { return this._bounds; } }

    public Vector3 Max { get { return this._bounds.max; } }

    public Vector3 Min { get { return this._bounds.min; } }

    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += "\"_top_front_left\":" + this.BoundingBoxCoordinates [0] + ", ";
        str_rep += "\"_top_front_right\":" + this.BoundingBoxCoordinates [1] + ", ";
        str_rep += "\"_top_back_left\":" + this.BoundingBoxCoordinates [2] + ", ";
        str_rep += "\"_top_back_right\":" + this.BoundingBoxCoordinates [3] + ", ";
        str_rep += "\"_bottom_front_left\":" + this.BoundingBoxCoordinates [4] + ", ";
        str_rep += "\"_bottom_front_right\":" + this.BoundingBoxCoordinates [5] + ", ";
        str_rep += "\"_bottom_back_left\":" + this.BoundingBoxCoordinates [6] + ", ";
        str_rep += "\"_bottom_back_right\":" + this.BoundingBoxCoordinates [7];
        return str_rep;
      }
    }

    public string BoundingBoxCoordinatesAsJSON {
      get {
        var str_rep = "{";
        str_rep += "\"_top_front_left\":" + this.JSONifyVec3 (vec : this.BoundingBoxCoordinates [0]) + ", ";
        str_rep += "\"_bottom_back_right\":" + this.JSONifyVec3 (vec : this.BoundingBoxCoordinates [7]);
        str_rep += "}";
        return str_rep;
      }
    }

    string JSONifyVec3 (Vector3 vec) {
      return string.Format (
        format : "[{0},{1},{2}]",
        arg0 : vec.x,
        arg1 : vec.y,
        arg2 : vec.z);
    }

    void Reset () {
      this.Awake ();
    }

    void Awake () {
      if (this._setup_on_awake) {
        this.Setup ();
        this.CalculateBounds ();
      }

      this._last_position = this.transform.position;
      this._last_rotation = this.transform.rotation;
      this._last_scale = this.transform.localScale;
      this.Initialise ();
      this._children_meshes = this.GetComponentsInChildren<MeshFilter> ();
      this._children_colliders = this.GetComponentsInChildren<Collider> ();
    }

    void Setup () {
      this._camera = FindObjectOfType<DrawBoundingBoxOnCamera> ();
      this._children_meshes = this.GetComponentsInChildren<MeshFilter> ();
      this._children_colliders = this.GetComponentsInChildren<Collider> ();
    }

    public void Initialise () {
      this.RecalculatePoints ();
      this.RecalculateLines ();
    }

    void LateUpdate () {
      if (this._freeze)
        return;
      if (this._children_meshes != this.GetComponentsInChildren<MeshFilter> ())
        this.Reset ();
      if (this._children_colliders != this.GetComponentsInChildren<Collider> ())
        this.Reset ();
      if (this.transform.localScale != this._last_scale) {
        this.ScaleBounds ();
        this.RecalculatePoints ();
      }

      if (this.transform.position != this._last_position
          || this.transform.rotation != this._last_rotation
          || this.transform.localScale != this._last_scale) {
        this.RecalculateLines ();
        this._last_rotation = this.transform.rotation;
        this._last_position = this.transform.position;
        this._last_scale = this.transform.localScale;
      }

      if (this._camera)
        this._camera.setOutlines (
          newOutlines : this._lines,
          newcolor : this._line_color,
          newTriangles : new Vector3[0, 0]);
    }

    public void ScaleBounds () {
      //_bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //_bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    void FitBoundingBoxToChildrenColliders () {
      var col = this.GetComponent<BoxCollider> ();
      var bounds = new Bounds (
                     center : this.transform.position,
                     size : Vector3.zero); // position and size

      if (col)
        bounds.Encapsulate (bounds : col.bounds);

      if (this._include_children)
        foreach (var child_col in this._children_colliders)
          if (child_col != col)
            bounds.Encapsulate (bounds : child_col.bounds);

      this._bounds = bounds;
      this._bounds_offset = bounds.center - this.transform.position;
    }

    void FitBoundingBoxToChildrenRenders () {
      var bounds = new Bounds (
                     center : this.transform.position,
                     size : Vector3.zero);

      var mesh = this.GetComponent<MeshFilter> ();
      if (mesh) {
        var ms = mesh.sharedMesh;
        var vc = ms.vertexCount;
        for (var i = 0; i < vc; i++)
          bounds.Encapsulate (point : mesh.transform.TransformPoint (position : ms.vertices [i]));
      }

      for (var i = 0; i < this._children_meshes.Length; i++) {
        var ms = this._children_meshes [i].sharedMesh;
        var vc = ms.vertexCount;
        for (var j = 0; j < vc; j++)
          bounds.Encapsulate (
            point : this._children_meshes [i].transform
                                         .TransformPoint (position : ms.vertices [j]));
      }

      this._bounds = bounds;
      this._bounds_offset = this._bounds.center - this.transform.position;
    }

    void CalculateBounds () {
      this._rotation = this.transform.rotation;
      this.transform.rotation = Quaternion.Euler (
        x : 0f,
        y : 0f,
        z : 0f);

      if (this._collider_based)
        this.FitBoundingBoxToChildrenColliders ();
      else
        this.FitBoundingBoxToChildrenRenders ();

      this.transform.rotation = this._rotation;
    }

    void RecalculatePoints () {
      this._bounds.size = new Vector3 (
        x : this._bounds.size.x
        * this.transform.localScale.x
        / this._last_scale.x,
        y : this._bounds.size.y
        * this.transform.localScale.y
        / this._last_scale.y,
        z : this._bounds.size.z
        * this.transform.localScale.z
        / this._last_scale.z);
      this._bounds_offset = new Vector3 (
        x : this._bounds_offset.x
        * this.transform.localScale.x
        / this._last_scale.x,
        y : this._bounds_offset.y
        * this.transform.localScale.y
        / this._last_scale.y,
        z : this._bounds_offset.z
        * this.transform.localScale.z
        / this._last_scale.z);

      this._top_front_right = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : 1,
          y : 1,
          z : 1));
      this._top_front_left = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : -1,
          y : 1,
          z : 1));
      this._top_back_left = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : -1,
          y : 1,
          z : -1));
      this._top_back_right = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : 1,
          y : 1,
          z : -1));
      this._bottom_front_right = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : 1,
          y : -1,
          z : 1));
      this._bottom_front_left = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : -1,
          y : -1,
          z : 1));
      this._bottom_back_left = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : -1,
          y : -1,
          z : -1));
      this._bottom_back_right = this._bounds_offset
      + Vector3.Scale (
        a : this._bounds.extents,
        b : new Vector3 (
          x : 1,
          y : -1,
          z : -1));

      this._corners = new[] {
        this._top_front_right,
        this._top_front_left,
        this._top_back_left,
        this._top_back_right,
        this._bottom_front_right,
        this._bottom_front_left,
        this._bottom_back_left,
        this._bottom_back_right
      };
    }

    void RecalculateLines () {
      var rot = this.transform.rotation;
      var pos = this.transform.position;

      var lines = new List<Vector3[]> ();
      //int linesCount = 12;

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {
          rot * this._corners [2 * i] + pos,
          rot * this._corners [2 * i + 1] + pos
        };
        lines.Add (item : line);

        //height
        line = new[] {
          rot * this._corners [i] + pos,
          rot * this._corners [i + 4] + pos
        };
        lines.Add (item : line);

        //depth
        line = new[] {
          rot * this._corners [2 * i] + pos,
          rot * this._corners [2 * i + 3 - 4 * (i % 2)] + pos
        };
        lines.Add (item : line);
      }

      this._lines = new Vector3[lines.Count, 2];
      for (var j = 0; j < lines.Count; j++) {
        this._lines [j,
          0] = lines [index : j] [0];
        this._lines [j,
          1] = lines [index : j] [1];
      }
    }

    void OnMouseDown () {
      //if (_permanent)
      //  return;
      this.enabled = !this.enabled;
    }

    #if UNITY_EDITOR
    void OnValidate() {
      if (EditorApplication.isPlaying)
        return;
      this.Initialise();
    }

    #endif

    void OnDrawGizmos () {
      Gizmos.color = this._line_color;
      if (this._lines != null)
        for (var i = 0; i < this._lines.GetLength (dimension : 0); i++)
          Gizmos.DrawLine (
            @from : this._lines [i,
              0],
            to : this._lines [i,
              1]);
    }
  }
}
