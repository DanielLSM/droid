using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    public override void ApplyMotion (MotorMotion motion) {
      if (Debugging)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      GetComponent<WheelCollider> ().motorTorque = motion.Strength;
      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Torque";
    }
  }
}
