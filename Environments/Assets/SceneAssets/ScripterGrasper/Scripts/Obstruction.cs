using SceneSpecificAssets.Grasping.Utilities;
using UnityEngine;

namespace SceneSpecificAssets.Grasping {
  public class Obstruction : MonoBehaviour,
                             MotionTracker {
    private Vector3 _last_recorded_move;
    private Quaternion _last_recorded_rotation;
    private Vector3 _previous_position;
    private Quaternion _previous_rotation;

    public bool IsInMotion() {
      return transform.position != _previous_position || transform.rotation != _previous_rotation;
    }

    public bool IsInMotion(float sensitivity) {
      var distance_moved = Vector3.Distance(
                                            transform.position,
                                            _last_recorded_move);
      var angle_rotated = Quaternion.Angle(
                                           transform.rotation,
                                           _last_recorded_rotation);
      if (distance_moved > sensitivity || angle_rotated > sensitivity) {
        UpdateLastRecordedTranform();
        return true;
      }

      return false;
    }

    private void UpdatePreviousTranform() {
      _previous_position = transform.position;
      _previous_rotation = transform.rotation;
    }

    private void UpdateLastRecordedTranform() {
      _last_recorded_move = transform.position;
      _last_recorded_rotation = transform.rotation;
    }

    private void Start() {
      UpdatePreviousTranform();
      UpdateLastRecordedTranform();
    }

    private void Update() { UpdatePreviousTranform(); }
  }
}
