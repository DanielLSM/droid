using UnityEngine;

namespace SceneAssets.ScripterGrasper.Grasps {
  [ExecuteInEditMode]
  public class Grasp : MonoBehaviour {
    [SerializeField]  bool _draw_ray_cast;

    [SerializeField]  float _obstruction_cast_length = 0.1f;
    [SerializeField]  float _obstruction_cast_radius = 0.1f;

    void Update() {
      var color = Color.white;
      if (this.IsObstructed())
        color = Color.red;
      if (this._draw_ray_cast) {
        Debug.DrawLine(
                       start : this.transform.position,
                       end : this.transform.position - this.transform.forward * this._obstruction_cast_length,
                       color : color);
        Debug.DrawLine(
                       start : this.transform.position - this.transform.up * this._obstruction_cast_radius,
                       end : this.transform.position + this.transform.up * this._obstruction_cast_radius,
                       color : color);
        Debug.DrawLine(
                       start : this.transform.position - this.transform.right * this._obstruction_cast_radius,
                       end : this.transform.position + this.transform.right * this._obstruction_cast_radius,
                       color : color);
      }
    }

    public bool IsObstructed() {
      RaycastHit hit;
      if (Physics.Linecast(
                           start : this.transform.position,
                           end : this.transform.position
                                 - this.transform.forward * this._obstruction_cast_length,
                           layerMask : LayerMask.GetMask("Obstruction")))
        return true;
      if (Physics.SphereCast(
                             origin : this.transform.position,
                             radius : this._obstruction_cast_radius,
                             direction : -this.transform.forward,
                             hitInfo : out hit,
                             maxDistance : this._obstruction_cast_length,
                             layerMask : LayerMask.GetMask("Obstruction")))
        return true;
      return Physics.OverlapCapsule(
                                    point0 : this.transform.position
                                             - this.transform.forward * this._obstruction_cast_radius,
                                    point1 : this.transform.position
                                             - this.transform.forward * this._obstruction_cast_length,
                                    radius : this._obstruction_cast_radius,
                                    layerMask : LayerMask.GetMask("Obstruction")).Length
             > 0;
    }
  }
}
