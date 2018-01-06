using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  public class EulerTransformMotor : Motor {
    [SerializeField]
    protected Axis _axis_of_motion;
    [SerializeField]
    protected Space _relative_to = Space.Self;
    [SerializeField]
    protected bool _no_collisions = true;
    [SerializeField]
    protected string _layer_mask = "Obstructions";

    public override void InnerApplyMotion (MotorMotion motion) {
      int layer_mask = 1 << LayerMask.NameToLayer (_layer_mask);
      var vec = Vector3.zero;
      switch (_axis_of_motion) {
      case Axis.X:
        vec = Vector3.right * motion.Strength;
        break;
      case Axis.Y:
        vec = -Vector3.up * motion.Strength;
        break;
      case Axis.Z:
        vec = -Vector3.forward * motion.Strength;
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
    
      if (_no_collisions) {
        if (!Physics.Raycast (transform.position, vec, Mathf.Abs (motion.Strength), layer_mask)) {
          transform.Translate (vec, _relative_to);
        }
      } else {
        transform.Translate (vec, _relative_to);
      }
    }

    public override string GetMotorIdentifier () {
      return name + "Transform" + _axis_of_motion.ToString ();
    }
  }
}
