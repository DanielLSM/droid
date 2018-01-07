using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent(typeof(WheelCollider))]
  public class SteeringMotor : Motor {
    private WheelCollider _wheel_collider;

    protected override void Start() {
      _wheel_collider = GetComponent<WheelCollider>();
      RegisterComponent();
    }

    private void FixedUpdate() { ApplyLocalPositionToVisuals(_wheel_collider); }

    public override void InnerApplyMotion(MotorMotion motion) {
      _wheel_collider.steerAngle = motion.Strength;
    }

    public override string GetMotorIdentifier() { return name + "Steering"; }

    public void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0) return;

      var visual_wheel = col.transform.GetChild(0);

      Vector3 position;
      Quaternion rotation;
      col.GetWorldPose(
                            out position,
                            out rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}
