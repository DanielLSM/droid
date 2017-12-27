using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  public class EulerTransformMotor : Motor {
    [SerializeField]
    protected Axis _axis_of_motion;
    [SerializeField]
    protected Space _relative_to = Space.Self;

    public override void InnerApplyMotion (MotorMotion motion) {
      switch (_axis_of_motion) {
      case Axis.X:
        transform.Translate (Vector3.left * motion.Strength, _relative_to);
        break;
      case Axis.Y:
        transform.Translate (-Vector3.up * motion.Strength, _relative_to);
        break;
      case Axis.Z:
        transform.Translate (-Vector3.forward * motion.Strength, _relative_to);
        break;
      case Axis.RotX:
        transform.Rotate (Vector3.left, motion.Strength, _relative_to);
        break;
      case Axis.RotY:
        transform.Rotate (Vector3.up, motion.Strength, _relative_to);
        break;
      case Axis.RotZ:
        transform.Rotate (Vector3.forward, motion.Strength, _relative_to);
        break;
      default:
        break;
      }
    }

    public override string GetMotorIdentifier () {
      return name + "Transform" + _axis_of_motion.ToString ();
    }
  }
}
