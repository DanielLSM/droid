﻿using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using UnityEngine;

namespace Neodroid.Models.Motors.WheelColliderMotor {
  [RequireComponent( typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    public override void InnerApplyMotion(MotorMotion motion) {
      this.GetComponent<WheelCollider>().motorTorque = motion.Strength;
    }

    public override string GetMotorIdentifier() { return this.name + "Torque"; }
  }
}
