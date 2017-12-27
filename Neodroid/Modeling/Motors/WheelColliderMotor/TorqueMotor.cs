using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    public override void InnerApplyMotion (MotorMotion motion) {
      GetComponent<WheelCollider> ().motorTorque = motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Torque";
    }
  }
}
