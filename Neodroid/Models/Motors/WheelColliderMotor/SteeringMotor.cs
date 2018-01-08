using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using UnityEngine;

namespace Neodroid.Models.Motors.WheelColliderMotor {
  [RequireComponent( typeof(WheelCollider))]
  public class SteeringMotor : Motor {
    WheelCollider _wheel_collider;

    protected override void Start() {
      this._wheel_collider = this.GetComponent<WheelCollider>();
      this.RegisterComponent();
    }

    void FixedUpdate() { this.ApplyLocalPositionToVisuals(col : this._wheel_collider); }

    public override void InnerApplyMotion(MotorMotion motion) {
      this._wheel_collider.steerAngle = motion.Strength;
    }

    public override string GetMotorIdentifier() { return this.name + "Steering"; }

    public void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0) return;

      var visual_wheel = col.transform.GetChild(index : 0);

      Vector3 position;
      Quaternion rotation;
      col.GetWorldPose(
                       pos : out position,
                       quat : out rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}
