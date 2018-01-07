using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {
    private Vector3 _bottom_back_left;
    private Vector3 _bottom_back_right;
    private Vector3 _bottom_front_left;
    private Vector3 _bottom_front_right;

    [HideInInspector]
    public Bounds _bounds;

    [HideInInspector]
    public Vector3 _bounds_offset;

    public DrawBoundingBoxOnCamera _camera;
    private Collider[] _children_colliders;

    private MeshFilter[] _children_meshes;

    public bool _collider_based;

    private Vector3[] _corners;
    public bool _freeze = true;

    public bool _include_children = true;

    // Vector3 startingBoundSize;
    // Vector3 startingBoundCenterLocal;
    private Vector3 _last_position;
    private Quaternion _last_rotation;

    [HideInInspector]
    //public Vector3 startingScale;
    private Vector3 _last_scale;

    public Color _line_color = new Color (
                                 0f,
                                 1f,
                                 0.4f,
                                 0.74f);

    private Vector3[,] _lines;

    private Quaternion _rotation;

    public bool _setup_on_awake = true;
    private Vector3 _top_back_left;
    private Vector3 _top_back_right;

    private Vector3 _top_front_left;
    private Vector3 _top_front_right;

    public Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
          _top_front_left,
          _top_front_right,
          _top_back_left,
          _top_back_right,
          _bottom_front_left,
          _bottom_front_right,
          _bottom_back_left,
          _bottom_back_right
        };
      }
    }

    public Bounds Bounds { get { return _bounds; } }

    public Vector3 Max { get { return _bounds.max; } }

    public Vector3 Min { get { return _bounds.min; } }

    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += "\"_top_front_left\":" + BoundingBoxCoordinates [0] + ", ";
        str_rep += "\"_top_front_right\":" + BoundingBoxCoordinates [1] + ", ";
        str_rep += "\"_top_back_left\":" + BoundingBoxCoordinates [2] + ", ";
        str_rep += "\"_top_back_right\":" + BoundingBoxCoordinates [3] + ", ";
        str_rep += "\"_bottom_front_left\":" + BoundingBoxCoordinates [4] + ", ";
        str_rep += "\"_bottom_front_right\":" + BoundingBoxCoordinates [5] + ", ";
        str_rep += "\"_bottom_back_left\":" + BoundingBoxCoordinates [6] + ", ";
        str_rep += "\"_bottom_back_right\":" + BoundingBoxCoordinates [7];
        return str_rep;
      }
    }

    public string BoundingBoxCoordinatesAsJSON {
      get {
        var str_rep = "{";
        str_rep += "\"_top_front_left\":" + JSONifyVec3 (BoundingBoxCoordinates [0]) + ", ";
        str_rep += "\"_bottom_back_right\":" + JSONifyVec3 (BoundingBoxCoordinates [7]);
        str_rep += "}";
        return str_rep;
      }
    }

    private string JSONifyVec3 (Vector3 vec) {
      return string.Format (
        "[{0},{1},{2}]",
        vec.x,
        vec.y,
        vec.z);
    }

    private void Reset () {
      Awake ();
    }

    private void Awake () {
      if (_setup_on_awake) {
        Setup ();
        CalculateBounds ();
      }

      _last_position = transform.position;
      _last_rotation = transform.rotation;
      _last_scale = transform.localScale;
      Initialise ();
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
    }

    private void Setup () {
      _camera = FindObjectOfType<DrawBoundingBoxOnCamera> ();
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
    }

    public void Initialise () {
      RecalculatePoints ();
      RecalculateLines ();
    }

    private void LateUpdate () {
      if (_freeze)
        return;
      if (_children_meshes != GetComponentsInChildren<MeshFilter> ())
        Reset ();
      if (_children_colliders != GetComponentsInChildren<Collider> ())
        Reset ();
      if (transform.localScale != _last_scale) {
        ScaleBounds ();
        RecalculatePoints ();
      }

      if (transform.position != _last_position
          || transform.rotation != _last_rotation
          || transform.localScale != _last_scale) {
        RecalculateLines ();
        _last_rotation = transform.rotation;
        _last_position = transform.position;
        _last_scale = transform.localScale;
      }

      if (_camera)
        _camera.setOutlines (
          _lines,
          _line_color,
          new Vector3[0, 0]);
    }

    public void ScaleBounds () {
      //_bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //_bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    private void FitBoundingBoxToChildrenColliders () {
      var col = GetComponent<BoxCollider> ();
      var bounds = new Bounds (
                     transform.position,
                     Vector3.zero); // position and size

      if (col)
        bounds.Encapsulate (col.bounds);

      if (_include_children)
        foreach (var child_col in _children_colliders)
          if (child_col != col)
            bounds.Encapsulate (child_col.bounds);

      _bounds = bounds;
      _bounds_offset = bounds.center - transform.position;
    }

    private void FitBoundingBoxToChildrenRenders () {
      var bounds = new Bounds (
                     transform.position,
                     Vector3.zero);

      var mesh = GetComponent<MeshFilter> ();
      if (mesh) {
        var ms = mesh.sharedMesh;
        var vc = ms.vertexCount;
        for (var i = 0; i < vc; i++)
          bounds.Encapsulate (mesh.transform.TransformPoint (ms.vertices [i]));
      }

      for (var i = 0; i < _children_meshes.Length; i++) {
        var ms = _children_meshes [i].sharedMesh;
        var vc = ms.vertexCount;
        for (var j = 0; j < vc; j++)
          bounds.Encapsulate (_children_meshes [i].transform.TransformPoint (ms.vertices [j]));
      }

      _bounds = bounds;
      _bounds_offset = _bounds.center - transform.position;
    }

    private void CalculateBounds () {
      _rotation = transform.rotation;
      transform.rotation = Quaternion.Euler (
        0f,
        0f,
        0f);

      if (_collider_based)
        FitBoundingBoxToChildrenColliders ();
      else
        FitBoundingBoxToChildrenRenders ();

      transform.rotation = _rotation;
    }

    private void RecalculatePoints () {
      _bounds.size = new Vector3 (
        _bounds.size.x * transform.localScale.x / _last_scale.x,
        _bounds.size.y * transform.localScale.y / _last_scale.y,
        _bounds.size.z * transform.localScale.z / _last_scale.z);
      _bounds_offset = new Vector3 (
        _bounds_offset.x * transform.localScale.x / _last_scale.x,
        _bounds_offset.y * transform.localScale.y / _last_scale.y,
        _bounds_offset.z * transform.localScale.z / _last_scale.z);

      _top_front_right = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          1,
          1,
          1));
      _top_front_left = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          -1,
          1,
          1));
      _top_back_left = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          -1,
          1,
          -1));
      _top_back_right = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          1,
          1,
          -1));
      _bottom_front_right = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          1,
          -1,
          1));
      _bottom_front_left = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          -1,
          -1,
          1));
      _bottom_back_left = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          -1,
          -1,
          -1));
      _bottom_back_right = _bounds_offset
      + Vector3.Scale (
        _bounds.extents,
        new Vector3 (
          1,
          -1,
          -1));

      _corners = new[] {
        _top_front_right,
        _top_front_left,
        _top_back_left,
        _top_back_right,
        _bottom_front_right,
        _bottom_front_left,
        _bottom_back_left,
        _bottom_back_right
      };
    }

    private void RecalculateLines () {
      var rot = transform.rotation;
      var pos = transform.position;

      var lines = new List<Vector3[]> ();
      //int linesCount = 12;

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {
                                 rot * _corners [2 * i] + pos,
                                 rot * _corners [2 * i + 1] + pos
                               };
        lines.Add (line);

        //height
        line = new[] {
          rot * _corners [i] + pos,
          rot * _corners [i + 4] + pos
        };
        lines.Add (line);

        //depth
        line = new[] {
          rot * _corners [2 * i] + pos,
          rot * _corners [2 * i + 3 - 4 * (i % 2)] + pos
        };
        lines.Add (line);
      }

      _lines = new Vector3[lines.Count, 2];
      for (var j = 0; j < lines.Count; j++) {
        _lines [j,
          0] = lines [j] [0];
        _lines [j,
          1] = lines [j] [1];
      }
    }

    private void OnMouseDown () {
      //if (_permanent)
      //  return;
      enabled = !enabled;
    }

    #if UNITY_EDITOR
    private void OnValidate() {
      if (EditorApplication.isPlaying)
        return;
      Initialise();
    }

    #endif

    private void OnDrawGizmos () {
      Gizmos.color = _line_color;
      if (_lines != null)
        for (var i = 0; i < _lines.GetLength (0); i++)
          Gizmos.DrawLine (
            _lines [i,
              0],
            _lines [i,
              1]);
    }
  }
}
