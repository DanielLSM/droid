using Neodroid.Messaging.Messages;
using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Motors {

  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyMotor : Motor {
    [SerializeField]
    protected Axis _axis_of_motion;
    public Space _relative_to = Space.Self;
    protected Rigidbody _rigidbody;

    protected override void Start () {
      _rigidbody = GetComponent<Rigidbody> ();
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      if (Debugging)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      switch (_axis_of_motion) {
      case Axis.X:
        if (_relative_to == Space.World) {
          _rigidbody.AddForce (Vector3.left * motion.Strength);
        } else {
          _rigidbody.AddRelativeForce (Vector3.left * motion.Strength);
        }
        break;
      case Axis.Y:
        if (_relative_to == Space.World) {
          _rigidbody.AddForce (Vector3.up * motion.Strength);
        } else {
          _rigidbody.AddRelativeForce (Vector3.up * motion.Strength);
        }
        break;
      case Axis.Z:
        if (_relative_to == Space.World) {
          _rigidbody.AddForce (Vector3.forward * motion.Strength);
        } else {
          _rigidbody.AddRelativeForce (Vector3.up * motion.Strength);
        }
        break;
      case Axis.RotX:
        if (_relative_to == Space.World) {
          _rigidbody.AddTorque (Vector3.left * motion.Strength);
        } else {
          _rigidbody.AddRelativeTorque (Vector3.left * motion.Strength);
        }
        break;
      case Axis.RotY:
        if (_relative_to == Space.World) {
          _rigidbody.AddTorque (Vector3.up * motion.Strength);
        } else {
          _rigidbody.AddRelativeTorque (Vector3.up * motion.Strength);
        }
        break;
      case Axis.RotZ:
        if (_relative_to == Space.World) {
          _rigidbody.AddTorque (Vector3.forward * motion.Strength);
        } else {
          _rigidbody.AddRelativeTorque (Vector3.forward * motion.Strength);
        }
        break;
      default:
        break;
      }
      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Rigidbody" + _axis_of_motion.ToString ();
    }
  }
}
