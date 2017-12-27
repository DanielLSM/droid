using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class SteeringMotor : Motor {

    WheelCollider _wheel_collider;

    protected override  void Start () {
      _wheel_collider = GetComponent<WheelCollider> ();
      RegisterComponent ();
    }

    void FixedUpdate () {
      ApplyLocalPositionToVisuals (_wheel_collider);
    }

    public override void InnerApplyMotion (MotorMotion motion) {
      _wheel_collider.steerAngle = motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Steering";
    }

    public void ApplyLocalPositionToVisuals (WheelCollider collider) {
      if (collider.transform.childCount == 0) {
        return;
      }

      Transform visualWheel = collider.transform.GetChild (0);

      Vector3 position;
      Quaternion rotation;
      collider.GetWorldPose (out position, out rotation);

      visualWheel.transform.position = position;
      visualWheel.transform.rotation = rotation;
    }
  }
}
