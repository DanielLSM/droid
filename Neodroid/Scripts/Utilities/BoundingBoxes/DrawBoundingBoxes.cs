using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class ShowBoundingBoxes : MonoBehaviour {
    public GameObject _line_object;
    private Dictionary<GameObject, GameObject> _lines;

    private MeshFilter[] _mesh_filter_objects;
    public Color color = Color.green;

    private void Start() { }

    private void ReallocateLineRenderers() {
      _mesh_filter_objects = FindObjectsOfType<MeshFilter>();
      _lines = new Dictionary<GameObject, GameObject>();
    }

    private void Update() {
      if (_lines == null || _mesh_filter_objects == null) ReallocateLineRenderers();
      CalcPositonsAndDrawBoxes();
    }

    private void CalcPositonsAndDrawBoxes() {
      foreach (var mesh_filter_object in _mesh_filter_objects)
        if (mesh_filter_object.gameObject.tag == "Target") {
          GameObject liner;
          if (!_lines.ContainsKey(mesh_filter_object.gameObject)) {
            liner = Instantiate(
                                _line_object,
                                _line_object.transform);
            _lines.Add(
                       mesh_filter_object.gameObject,
                       liner);
          } else {
            print("found Target");
            liner = _lines[mesh_filter_object.gameObject];
          }

          var bounds = mesh_filter_object.mesh.bounds;

          //Bounds bounds;
          //BoxCollider bc = GetComponent<BoxCollider>();
          //if (bc != null)
          //    bounds = bc.bounds;
          //else
          //return;

          var v3Center = bounds.center;
          var v3Extents = bounds.extents;

          var v3FrontTopLeft = new Vector3(
                                           v3Center.x - v3Extents.x,
                                           v3Center.y + v3Extents.y,
                                           v3Center.z - v3Extents.z); // Front top left corner
          var v3FrontTopRight = new Vector3(
                                            v3Center.x + v3Extents.x,
                                            v3Center.y + v3Extents.y,
                                            v3Center.z - v3Extents.z); // Front top right corner
          var v3FrontBottomLeft = new Vector3(
                                              v3Center.x - v3Extents.x,
                                              v3Center.y - v3Extents.y,
                                              v3Center.z - v3Extents.z); // Front bottom left corner
          var v3FrontBottomRight = new Vector3(
                                               v3Center.x + v3Extents.x,
                                               v3Center.y - v3Extents.y,
                                               v3Center.z - v3Extents.z); // Front bottom right corner
          var v3BackTopLeft = new Vector3(
                                          v3Center.x - v3Extents.x,
                                          v3Center.y + v3Extents.y,
                                          v3Center.z + v3Extents.z); // Back top left corner
          var v3BackTopRight = new Vector3(
                                           v3Center.x + v3Extents.x,
                                           v3Center.y + v3Extents.y,
                                           v3Center.z + v3Extents.z); // Back top right corner
          var v3BackBottomLeft = new Vector3(
                                             v3Center.x - v3Extents.x,
                                             v3Center.y - v3Extents.y,
                                             v3Center.z + v3Extents.z); // Back bottom left corner
          var v3BackBottomRight = new Vector3(
                                              v3Center.x + v3Extents.x,
                                              v3Center.y - v3Extents.y,
                                              v3Center.z + v3Extents.z); // Back bottom right corner

          v3FrontTopLeft = mesh_filter_object.transform.TransformPoint(v3FrontTopLeft);
          v3FrontTopRight = mesh_filter_object.transform.TransformPoint(v3FrontTopRight);
          v3FrontBottomLeft = mesh_filter_object.transform.TransformPoint(v3FrontBottomLeft);
          v3FrontBottomRight = mesh_filter_object.transform.TransformPoint(v3FrontBottomRight);
          v3BackTopLeft = mesh_filter_object.transform.TransformPoint(v3BackTopLeft);
          v3BackTopRight = mesh_filter_object.transform.TransformPoint(v3BackTopRight);
          v3BackBottomLeft = mesh_filter_object.transform.TransformPoint(v3BackBottomLeft);
          v3BackBottomRight = mesh_filter_object.transform.TransformPoint(v3BackBottomRight);

          liner.GetComponent<LineRenderer>().SetPosition(
                                                         0,
                                                         v3BackTopLeft);
          liner.GetComponent<LineRenderer>().SetPosition(
                                                         1,
                                                         v3BackTopRight);

          DrawBox(
                  v3FrontTopLeft,
                  v3FrontTopRight,
                  v3FrontBottomLeft,
                  v3FrontBottomRight,
                  v3BackTopLeft,
                  v3BackTopRight,
                  v3BackBottomLeft,
                  v3BackBottomRight);
        }
    }

    private void DrawBox(
      Vector3 v3FrontTopLeft,
      Vector3 v3FrontTopRight,
      Vector3 v3FrontBottomLeft,
      Vector3 v3FrontBottomRight,
      Vector3 v3BackTopLeft,
      Vector3 v3BackTopRight,
      Vector3 v3BackBottomLeft,
      Vector3 v3BackBottomRight) {
      Debug.DrawLine(
                     v3FrontTopLeft,
                     v3FrontTopRight,
                     color);
      Debug.DrawLine(
                     v3FrontTopRight,
                     v3FrontBottomRight,
                     color);
      Debug.DrawLine(
                     v3FrontBottomRight,
                     v3FrontBottomLeft,
                     color);
      Debug.DrawLine(
                     v3FrontBottomLeft,
                     v3FrontTopLeft,
                     color);

      Debug.DrawLine(
                     v3BackTopLeft,
                     v3BackTopRight,
                     color);
      Debug.DrawLine(
                     v3BackTopRight,
                     v3BackBottomRight,
                     color);
      Debug.DrawLine(
                     v3BackBottomRight,
                     v3BackBottomLeft,
                     color);
      Debug.DrawLine(
                     v3BackBottomLeft,
                     v3BackTopLeft,
                     color);

      Debug.DrawLine(
                     v3FrontTopLeft,
                     v3BackTopLeft,
                     color);
      Debug.DrawLine(
                     v3FrontTopRight,
                     v3BackTopRight,
                     color);
      Debug.DrawLine(
                     v3FrontBottomRight,
                     v3BackBottomRight,
                     color);
      Debug.DrawLine(
                     v3FrontBottomLeft,
                     v3BackBottomLeft,
                     color);
    }
  }
}
