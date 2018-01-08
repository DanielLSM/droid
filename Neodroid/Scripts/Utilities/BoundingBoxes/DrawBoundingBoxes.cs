using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class ShowBoundingBoxes : MonoBehaviour {
    public GameObject _line_object;
    Dictionary<GameObject, GameObject> _lines;

    MeshFilter[] _mesh_filter_objects;
    public Color color = Color.green;

    void Start() { }

    void ReallocateLineRenderers() {
      this._mesh_filter_objects = FindObjectsOfType<MeshFilter>();
      this._lines = new Dictionary<GameObject, GameObject>();
    }

    void Update() {
      if (this._lines == null || this._mesh_filter_objects == null) this.ReallocateLineRenderers();
      this.CalcPositonsAndDrawBoxes();
    }

    void CalcPositonsAndDrawBoxes() {
      foreach (var mesh_filter_object in this._mesh_filter_objects)
        if (mesh_filter_object.gameObject.tag == "Target") {
          GameObject liner;
          if (!this._lines.ContainsKey(key : mesh_filter_object.gameObject)) {
            liner = Instantiate(
                                original : this._line_object,
                                parent : this._line_object.transform);
            this._lines.Add(
                            key : mesh_filter_object.gameObject,
                            value : liner);
          } else {
            print(message : "found Target");
            liner = this._lines[key : mesh_filter_object.gameObject];
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
                                           x : v3Center.x - v3Extents.x,
                                           y : v3Center.y + v3Extents.y,
                                           z : v3Center.z - v3Extents.z); // Front top left corner
          var v3FrontTopRight = new Vector3(
                                            x : v3Center.x + v3Extents.x,
                                            y : v3Center.y + v3Extents.y,
                                            z : v3Center.z - v3Extents.z); // Front top right corner
          var v3FrontBottomLeft = new Vector3(
                                              x : v3Center.x - v3Extents.x,
                                              y : v3Center.y - v3Extents.y,
                                              z : v3Center.z - v3Extents.z); // Front bottom left corner
          var v3FrontBottomRight = new Vector3(
                                               x : v3Center.x + v3Extents.x,
                                               y : v3Center.y - v3Extents.y,
                                               z : v3Center.z - v3Extents.z); // Front bottom right corner
          var v3BackTopLeft = new Vector3(
                                          x : v3Center.x - v3Extents.x,
                                          y : v3Center.y + v3Extents.y,
                                          z : v3Center.z + v3Extents.z); // Back top left corner
          var v3BackTopRight = new Vector3(
                                           x : v3Center.x + v3Extents.x,
                                           y : v3Center.y + v3Extents.y,
                                           z : v3Center.z + v3Extents.z); // Back top right corner
          var v3BackBottomLeft = new Vector3(
                                             x : v3Center.x - v3Extents.x,
                                             y : v3Center.y - v3Extents.y,
                                             z : v3Center.z + v3Extents.z); // Back bottom left corner
          var v3BackBottomRight = new Vector3(
                                              x : v3Center.x + v3Extents.x,
                                              y : v3Center.y - v3Extents.y,
                                              z : v3Center.z + v3Extents.z); // Back bottom right corner

          v3FrontTopLeft = mesh_filter_object.transform.TransformPoint(position : v3FrontTopLeft);
          v3FrontTopRight = mesh_filter_object.transform.TransformPoint(position : v3FrontTopRight);
          v3FrontBottomLeft = mesh_filter_object.transform.TransformPoint(position : v3FrontBottomLeft);
          v3FrontBottomRight = mesh_filter_object.transform.TransformPoint(position : v3FrontBottomRight);
          v3BackTopLeft = mesh_filter_object.transform.TransformPoint(position : v3BackTopLeft);
          v3BackTopRight = mesh_filter_object.transform.TransformPoint(position : v3BackTopRight);
          v3BackBottomLeft = mesh_filter_object.transform.TransformPoint(position : v3BackBottomLeft);
          v3BackBottomRight = mesh_filter_object.transform.TransformPoint(position : v3BackBottomRight);

          liner.GetComponent<LineRenderer>().SetPosition(
                                                         index : 0,
                                                         position : v3BackTopLeft);
          liner.GetComponent<LineRenderer>().SetPosition(
                                                         index : 1,
                                                         position : v3BackTopRight);

          this.DrawBox(
                       v3FrontTopLeft : v3FrontTopLeft,
                       v3FrontTopRight : v3FrontTopRight,
                       v3FrontBottomLeft : v3FrontBottomLeft,
                       v3FrontBottomRight : v3FrontBottomRight,
                       v3BackTopLeft : v3BackTopLeft,
                       v3BackTopRight : v3BackTopRight,
                       v3BackBottomLeft : v3BackBottomLeft,
                       v3BackBottomRight : v3BackBottomRight);
        }
    }

    void DrawBox(
      Vector3 v3FrontTopLeft,
      Vector3 v3FrontTopRight,
      Vector3 v3FrontBottomLeft,
      Vector3 v3FrontBottomRight,
      Vector3 v3BackTopLeft,
      Vector3 v3BackTopRight,
      Vector3 v3BackBottomLeft,
      Vector3 v3BackBottomRight) {
      Debug.DrawLine(
                     start : v3FrontTopLeft,
                     end : v3FrontTopRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontTopRight,
                     end : v3FrontBottomRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontBottomRight,
                     end : v3FrontBottomLeft,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontBottomLeft,
                     end : v3FrontTopLeft,
                     color : this.color);

      Debug.DrawLine(
                     start : v3BackTopLeft,
                     end : v3BackTopRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3BackTopRight,
                     end : v3BackBottomRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3BackBottomRight,
                     end : v3BackBottomLeft,
                     color : this.color);
      Debug.DrawLine(
                     start : v3BackBottomLeft,
                     end : v3BackTopLeft,
                     color : this.color);

      Debug.DrawLine(
                     start : v3FrontTopLeft,
                     end : v3BackTopLeft,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontTopRight,
                     end : v3BackTopRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontBottomRight,
                     end : v3BackBottomRight,
                     color : this.color);
      Debug.DrawLine(
                     start : v3FrontBottomLeft,
                     end : v3BackBottomLeft,
                     color : this.color);
    }
  }
}
