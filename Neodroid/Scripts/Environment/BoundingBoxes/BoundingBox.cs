using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Utilities.BoundingBoxes {

  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {

    public bool _collider_based = false;
    public bool _freeze = false;

    public Color _line_color = new Color (0f, 1f, 0.4f, 0.74f);

    [HideInInspector]
    public Bounds _bounds;
    [HideInInspector]
    public Vector3 _bounds_offset;

    public bool _setup_on_awake = true;

    Vector3[] _corners;

    Vector3[,] _lines;

    Quaternion _rotation;

    DrawBoundingBoxOnCamera _camera_lines;

    MeshFilter[] _children_meshes;
    Collider[] _children_colliders;

    public Vector3[] BoundingBoxCoordinates {
      get {
        return new Vector3[] {
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

    public string BoundingBoxCoordinatesAsString {
      get {
        string str_rep = "";
        str_rep += "\"_top_front_left\":" + BoundingBoxCoordinates [0].ToString () + ", ";
        str_rep += "\"_top_front_right\":" + BoundingBoxCoordinates [1].ToString () + ", ";
        str_rep += "\"_top_back_left\":" + BoundingBoxCoordinates [2].ToString () + ", ";
        str_rep += "\"_top_back_right\":" + BoundingBoxCoordinates [3].ToString () + ", ";
        str_rep += "\"_bottom_front_left\":" + BoundingBoxCoordinates [4].ToString () + ", ";
        str_rep += "\"_bottom_front_right\":" + BoundingBoxCoordinates [5].ToString () + ", ";
        str_rep += "\"_bottom_back_left\":" + BoundingBoxCoordinates [6].ToString () + ", ";
        str_rep += "\"_bottom_back_right\":" + BoundingBoxCoordinates [7].ToString ();
        return str_rep;
      }
    }

    Vector3 _top_front_left;
    Vector3 _top_front_right;
    Vector3 _top_back_left;
    Vector3 _top_back_right;
    Vector3 _bottom_front_left;
    Vector3 _bottom_front_right;
    Vector3 _bottom_back_left;
    Vector3 _bottom_back_right;

    [HideInInspector]
    //public Vector3 startingScale;
    Vector3 _last_scale;
    // Vector3 startingBoundSize;
    // Vector3 startingBoundCenterLocal;
    Vector3 _last_position;
    Quaternion _last_rotation;


    void Reset () {
      Setup ();
      CalculateBounds ();
      Start ();
    }

    void Awake () {
      if (_setup_on_awake) {
        Setup ();
        CalculateBounds ();
      }
    }

    void Setup () {
      _camera_lines = FindObjectOfType<DrawBoundingBoxOnCamera> ();
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
    }

    void Start () {
      _last_position = transform.position;
      _last_rotation = transform.rotation;
      _last_scale = transform.localScale;
      Initialise ();
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
    }

    public void Initialise () {
      RecalculatePoints ();
      RecalculateLines ();
    }

    void LateUpdate () {
      if (_freeze) {
        return;
      }
      if (_children_meshes != GetComponentsInChildren<MeshFilter> ()) {
        Setup ();
        CalculateBounds ();
        Start ();
      }
      if (_children_colliders != GetComponentsInChildren<Collider> ()) {
        Setup ();
        CalculateBounds ();
        Start ();
      }
      if (transform.localScale != _last_scale) {
        ScaleBounds ();
        RecalculatePoints ();
      }
      if (transform.position != _last_position || transform.rotation != _last_rotation || transform.localScale != _last_scale) {
        RecalculateLines ();
        _last_rotation = transform.rotation;
        _last_position = transform.position;
        _last_scale = transform.localScale;
      }
      if (_camera_lines) {
        _camera_lines.setOutlines (_lines, _line_color, new Vector3[0, 0]);
      }
    }

    public void ScaleBounds () {
      //_bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //_bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    void FitBoundingBoxToChildrenColliders () {

      var collider = this.GetComponent<BoxCollider> ();
      Bounds bounds = new Bounds (this.transform.position, Vector3.zero); // position and size

      foreach (var col in _children_colliders) {
        if (col != collider) {
          bounds.Encapsulate (col.bounds);
        }
      }

      _bounds = bounds;
      _bounds_offset = bounds.center - this.transform.position;
    }

    void FitBoundingBoxToChildrenRenders () {
      _bounds = new Bounds ();
      for (int i = 0; i < _children_meshes.Length; i++) {
        Mesh ms = _children_meshes [i].sharedMesh;
        int vc = ms.vertexCount;
        for (int j = 0; j < vc; j++) {
          if (i == 0 && j == 0) {
            _bounds = new Bounds (_children_meshes [i].transform.TransformPoint (ms.vertices [j]), Vector3.zero);
          } else {
            _bounds.Encapsulate (_children_meshes [i].transform.TransformPoint (ms.vertices [j]));
          }
        }
      }
      _bounds_offset = _bounds.center - transform.position;
    }

    void CalculateBounds () {
      _rotation = transform.rotation;
      this.transform.rotation = Quaternion.Euler (0f, 0f, 0f);

      if (_collider_based) {
        FitBoundingBoxToChildrenColliders ();
      } else {
        FitBoundingBoxToChildrenRenders ();
      }
        
      transform.rotation = _rotation;
    }

    void RecalculatePoints () {

      _bounds.size = new Vector3 (_bounds.size.x * transform.localScale.x / _last_scale.x, _bounds.size.y * transform.localScale.y / _last_scale.y, _bounds.size.z * transform.localScale.z / _last_scale.z);
      _bounds_offset = new Vector3 (_bounds_offset.x * transform.localScale.x / _last_scale.x, _bounds_offset.y * transform.localScale.y / _last_scale.y, _bounds_offset.z * transform.localScale.z / _last_scale.z);


      _top_front_right = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (1, 1, 1));
      _top_front_left = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (-1, 1, 1));
      _top_back_left = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (-1, 1, -1));
      _top_back_right = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (1, 1, -1));
      _bottom_front_right = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (1, -1, 1));
      _bottom_front_left = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (-1, -1, 1));
      _bottom_back_left = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (-1, -1, -1));
      _bottom_back_right = _bounds_offset + Vector3.Scale (_bounds.extents, new Vector3 (1, -1, -1));

      _corners = new Vector3[] {
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

    void RecalculateLines () {

      Quaternion rot = transform.rotation;
      Vector3 pos = transform.position;

      List<Vector3[]> lines = new List<Vector3[]> ();
      //int linesCount = 12;

      Vector3[] line;
      for (int i = 0; i < 4; i++) {

        //width
        line = new Vector3[] { rot * _corners [2 * i] + pos, rot * _corners [2 * i + 1] + pos };
        lines.Add (line);

        //height
        line = new Vector3[] { rot * _corners [i] + pos, rot * _corners [i + 4] + pos };
        lines.Add (line);

        //depth
        line = new Vector3[] { rot * _corners [2 * i] + pos, rot * _corners [2 * i + 3 - 4 * (i % 2)] + pos };
        lines.Add (line);

      }
      _lines = new Vector3[lines.Count, 2];
      for (int j = 0; j < lines.Count; j++) {
        _lines [j, 0] = lines [j] [0];
        _lines [j, 1] = lines [j] [1];
      }
    }

    void OnMouseDown () {
      //if (_permanent)
      //  return;
      enabled = !enabled;
    }

    #if UNITY_EDITOR
    void OnValidate () {
      if (EditorApplication.isPlaying)
        return;
      Initialise ();
    }


    #endif

    void OnDrawGizmos () {

      Gizmos.color = _line_color;
      if (_lines != null) {
        for (int i = 0; i < _lines.GetLength (0); i++) {
          Gizmos.DrawLine (_lines [i, 0], _lines [i, 1]);
        }
      }
    }

  }
}