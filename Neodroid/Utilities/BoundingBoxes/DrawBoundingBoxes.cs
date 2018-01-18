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
      if (this._lines == null || this._mesh_filter_objects == null)
        this.ReallocateLineRenderers();
      this.CalcPositonsAndDrawBoxes();
    }

    void CalcPositonsAndDrawBoxes() {
      foreach (var mesh_filter_object in this._mesh_filter_objects) {
        if (mesh_filter_object.gameObject.tag == "Target") {
          GameObject liner;
          if (!this._lines.ContainsKey(mesh_filter_object.gameObject)) {
            liner = Instantiate(this._line_object, this._line_object.transform);
            this._lines.Add(mesh_filter_object.gameObject, liner);
          } else {
            print("found Target");
            liner = this._lines[mesh_filter_object.gameObject];
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

          var corners = NeodroidUtilities.ExtractCorners(v3Center, v3Extents, mesh_filter_object.transform);

          liner.GetComponent<LineRenderer>().SetPosition(0, corners[4]);
          liner.GetComponent<LineRenderer>().SetPosition(1, corners[5]);

          NeodroidUtilities.DrawBox(
              corners[0],
              corners[1],
              corners[2],
              corners[3],
              corners[4],
              corners[5],
              corners[6],
              corners[7],
              this.color);
        }
      }
    }
  }
}
