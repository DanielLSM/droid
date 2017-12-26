using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  public class EulerTransformMotor : Motor {
    [SerializeField]
    Axis _axis_of_motion;

    public Space _space = Space.Self;

    public override void ApplyMotion (MotorMotion motion) {
      if (Debugging)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      switch (_axis_of_motion) {
      case Axis.X:
        transform.Translate (Vector3.left * motion.Strength, _space);
        break;
      case Axis.Y:
        transform.Translate (-Vector3.up * motion.Strength, _space);
        break;
      case Axis.Z:
        transform.Translate (-Vector3.forward * motion.Strength, _space);
        break;
      case Axis.RotX:
        transform.Rotate (Vector3.left, motion.Strength, _space);
        break;
      case Axis.RotY:
        transform.Rotate (Vector3.up, motion.Strength, _space);
        break;
      case Axis.RotZ:
        transform.Rotate (Vector3.forward, motion.Strength, _space);
        break;
      default:
        break;
      }
      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Transform" + _axis_of_motion.ToString ();
    }
  }
}
