using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    public override void ApplyMotion (MotorMotion motion) {
      if (_debug)
        Debug.Log ("Applying " + motion.Strength.ToString () + " To " + name);
      if (!_bidirectional && motion.Strength < 0) {
        Debug.Log ("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      GetComponent<WheelCollider> ().motorTorque = motion.Strength;
      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Torque";
    }
  }
}
