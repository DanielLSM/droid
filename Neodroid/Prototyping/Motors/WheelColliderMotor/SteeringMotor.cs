﻿using System;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Models.Motors.WheelColliderMotor {
  [RequireComponent(typeof(WheelCollider))]
  public class SteeringMotor : Motor {
    [SerializeField] WheelCollider _wheel_collider;

    public override String MotorIdentifier { get { return this.name + "Steering"; } }

    protected override void Awake() {
      base.Awake();
      this._wheel_collider = this.GetComponent<WheelCollider>();
    }

    void FixedUpdate() { this.ApplyLocalPositionToVisuals(this._wheel_collider); }

    protected override void InnerApplyMotion(MotorMotion motion) {
      this._wheel_collider.steerAngle = motion.Strength;
    }

    void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0)
        return;

      var visual_wheel = col.transform.GetChild(0);

      Vector3 position;
      Quaternion rotation;
      col.GetWorldPose(out position, out rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}
