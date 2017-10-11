using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.BoundingBoxes {

  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {

    public bool _collider_based = false;

    public Color _line_color = new Color (0f, 1f, 0.4f, 0.74f);

    private Bounds _bounds;
    private Vector3 _bounds_offset;
    [HideInInspector]
    public Bounds _collider_bounds;
    [HideInInspector]
    public Vector3 _collider_bounds_offset;
    [HideInInspector]
    public Bounds _mesh_bounds;
    [HideInInspector]
    public Vector3 _mesh_bounds_offset;

    public bool _setup_on_awake = true;

    private Vector3[] _corners;

    private Vector3[,] _lines;

    private Quaternion _rotation;

    //private Camera _camera;

    private DrawBoundingBoxOnCamera _camera_lines;

    private MeshFilter[] _children_meshes;
    private Collider[] _children_colliders;

    public Vector3[] BoundingBoxCoordinates {
      get{ return new Vector3[] {
          _top_front_left,
          _top_front_right,
          _top_back_left,
          _top_back_right,
          _bottom_front_left,
          _bottom_front_right,
          _bottom_back_left,
          _bottom_back_right
        }; }
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

    private Vector3 _top_front_left;
    private Vector3 _top_front_right;
    private Vector3 _top_back_left;
    private Vector3 _top_back_right;
    private Vector3 _bottom_front_left;
    private Vector3 _bottom_front_right;
    private Vector3 _bottom_back_left;
    private Vector3 _bottom_back_right;

    [HideInInspector]
    public Vector3 startingScale;
    private Vector3 previousScale;
    //private Vector3 startingBoundSize;
    //private Vector3 startingBoundCenterLocal;
    private Vector3 previousPosition;
    private Quaternion previousRotation;


    void Reset () {
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
      CalculateBounds ();
      Start ();
    }

    void Awake () {
      if (_setup_on_awake) {
        _children_meshes = GetComponentsInChildren<MeshFilter> ();
        _children_colliders = GetComponentsInChildren<Collider> ();
        CalculateBounds ();
      }
    }

    void Start () {
      _camera_lines = FindObjectOfType (typeof(DrawBoundingBoxOnCamera)) as DrawBoundingBoxOnCamera;

      if (!_camera_lines) {
        Debug.LogError ("DimBoxes: no camera with DimBoxes.DrawLines in the scene", gameObject);
        return;
      }

      //_camera = _camera_lines.GetComponent<Camera> ();
      previousPosition = transform.position;
      previousRotation = transform.rotation;
      //startingBoundSize = _bounds.size;
      startingScale = transform.localScale;
      previousScale = startingScale;
      //startingBoundCenterLocal = transform.InverseTransformPoint (_bounds.center);
      Initialise ();
      _children_meshes = GetComponentsInChildren<MeshFilter> ();
      _children_colliders = GetComponentsInChildren<Collider> ();
    }

    public void Initialise () {
      RecalculatePoints ();
      RecalculateLines ();
    }

    void LateUpdate () {
      if (_children_meshes != GetComponentsInChildren<MeshFilter> ()) {
        _children_meshes = GetComponentsInChildren<MeshFilter> ();
        CalculateBounds ();
        Start ();
      }
      if (_children_colliders != GetComponentsInChildren<Collider> ()) {
        _children_colliders = GetComponentsInChildren<Collider> ();
        CalculateBounds ();
        Start ();
      }
      if (transform.localScale != previousScale) {
        ScaleBounds ();
        RecalculatePoints ();
      }
      if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale) {
        RecalculateLines ();
        previousRotation = transform.rotation;
        previousPosition = transform.position;
        previousScale = transform.localScale;
      }

      _camera_lines.setOutlines (_lines, _line_color, new Vector3[0, 0]);
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

      _collider_bounds = bounds;
      _collider_bounds_offset = bounds.center - this.transform.position;
    }

    void FitBoundingBoxToChildrenRenders () {
      _mesh_bounds = new Bounds ();
      for (int i = 0; i < _children_meshes.Length; i++) {
        Mesh ms = _children_meshes [i].sharedMesh;
        int vc = ms.vertexCount;
        for (int j = 0; j < vc; j++) {
          if (i == 0 && j == 0) {
            _mesh_bounds = new Bounds (_children_meshes [i].transform.TransformPoint (ms.vertices [j]), Vector3.zero);
          } else {
            _mesh_bounds.Encapsulate (_children_meshes [i].transform.TransformPoint (ms.vertices [j]));
          }
        }
      }
      _mesh_bounds_offset = _mesh_bounds.center - transform.position;
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

      if (_collider_based) {
        _bounds = _collider_bounds;
        _bounds_offset = _collider_bounds_offset;
      } else {
        _bounds = _mesh_bounds;
        _bounds_offset = _mesh_bounds_offset;
      }

      _bounds.size = new Vector3 (_bounds.size.x * transform.localScale.x / startingScale.x, _bounds.size.y * transform.localScale.y / startingScale.y, _bounds.size.z * transform.localScale.z / startingScale.z);
      _bounds_offset = new Vector3 (_bounds_offset.x * transform.localScale.x / startingScale.x, _bounds_offset.y * transform.localScale.y / startingScale.y, _bounds_offset.z * transform.localScale.z / startingScale.z);


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
      for (int i = 0; i < _lines.GetLength (0); i++) {
        Gizmos.DrawLine (_lines [i, 0], _lines [i, 1]);
      }
    }

  }
}