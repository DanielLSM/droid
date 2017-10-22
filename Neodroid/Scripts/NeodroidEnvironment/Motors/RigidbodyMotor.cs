using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.NeodroidEnvironment.Motors {

  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyMotor : Motor {
    public MotorAxis _axis_of_motion;
    Rigidbody _rigidbody;

    private void Start () {
      _rigidbody = GetComponent<Rigidbody> ();
      RegisterComponent ();
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (!_bidirectional && motion._strength < 0) {
        Debug.Log ("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      if (_debug)
        Debug.Log ("Applying " + motion._strength.ToString () + " To " + name);
      switch (_axis_of_motion) {
      case MotorAxis.X:
        _rigidbody.AddForce (Vector3.left * motion._strength);
        break;
      case MotorAxis.Y:
        _rigidbody.AddForce (Vector3.up * motion._strength);
        break;
      case MotorAxis.Z:
        _rigidbody.AddForce (Vector3.forward * motion._strength);
        break;
      case MotorAxis.RotX:
        _rigidbody.AddTorque (Vector3.left * motion._strength);
          //GetComponent<Rigidbody>().AddForceAtPosition(Vector3.forward * motion._strength, transform.position);
          //GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * motion._strength);
        break;
      case MotorAxis.RotY:
        _rigidbody.AddTorque (Vector3.up * motion._strength);
        break;
      case MotorAxis.RotZ:
        _rigidbody.AddTorque (Vector3.forward * motion._strength);
        break;
      default:
        break;
      }
      _energy_spend_since_reset += _energy_cost * motion._strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Rigidbody" + _axis_of_motion.ToString ();
    }
  }
}
