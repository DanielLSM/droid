using Neodroid.Messaging.Messages;
using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Motors {
  [RequireComponent (typeof(ParticleSystem))]
  [RequireComponent (typeof(Rigidbody))]
  public class RocketMotor : RigidbodyMotor {
    protected ParticleSystem _particle_system;
    bool _fired_this_step = false;


    protected override void Start () {
      _rigidbody = GetComponent<Rigidbody> ();
      _particle_system = GetComponent<ParticleSystem> ();
      var valid_input = ValidInput;
      valid_input.min_value = 0;
      ValidInput = valid_input;
      RegisterComponent ();
    }

    void LateUpdate () {
      if (_fired_this_step) {
        if (!_particle_system.isPlaying) {
          _particle_system.Play (true);
        }
      } else {
        _particle_system.Stop (true);
      }
      _fired_this_step = false;
    }

    public override void InnerApplyMotion (MotorMotion motion) {
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
      _fired_this_step = true;
    }

    public override string GetMotorIdentifier () {
      return name + "Rocket" + _axis_of_motion.ToString ();
    }
  }
}
