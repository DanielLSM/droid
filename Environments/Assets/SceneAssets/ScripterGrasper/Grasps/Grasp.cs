using UnityEngine;

namespace SceneSpecificAssets.Grasping.Grasps {
  [ExecuteInEditMode]
  public class Grasp : MonoBehaviour {
    public bool _draw_ray_cast;

    public float _obstruction_cast_length = 0.1f;
    public float _obstruction_cast_radius = 0.1f;

    private void Update() {
      var color = Color.white;
      if (IsObstructed())
        color = Color.red;
      if (_draw_ray_cast) {
        Debug.DrawLine(
                       transform.position,
                       transform.position - transform.forward * _obstruction_cast_length,
                       color);
        Debug.DrawLine(
                       transform.position - transform.up * _obstruction_cast_radius,
                       transform.position + transform.up * _obstruction_cast_radius,
                       color);
        Debug.DrawLine(
                       transform.position - transform.right * _obstruction_cast_radius,
                       transform.position + transform.right * _obstruction_cast_radius,
                       color);
      }
    }

    public bool IsObstructed() {
      RaycastHit hit;
      if (Physics.Linecast(
                           transform.position,
                           transform.position - transform.forward * _obstruction_cast_length,
                           LayerMask.GetMask("Obstruction")))
        return true;
      if (Physics.SphereCast(
                             transform.position,
                             _obstruction_cast_radius,
                             -transform.forward,
                             out hit,
                             _obstruction_cast_length,
                             LayerMask.GetMask("Obstruction")))
        return true;
      return Physics.OverlapCapsule(
                                    transform.position - transform.forward * _obstruction_cast_radius,
                                    transform.position - transform.forward * _obstruction_cast_length,
                                    _obstruction_cast_radius,
                                    LayerMask.GetMask("Obstruction")).Length
             > 0;
    }
  }
}
