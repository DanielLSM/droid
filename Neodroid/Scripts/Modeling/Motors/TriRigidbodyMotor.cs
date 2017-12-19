using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(Rigidbody))]
  public class TriRigidbodyMotor : RigidbodyMotor {

    public bool _rotational_motors;
    string _X;
    string _Y;
    string _Z;

    public override void RegisterComponent () {
      _X = GetMotorIdentifier () + "X";
      _Y = GetMotorIdentifier () + "Y";
      _Z = GetMotorIdentifier () + "Z";
      if (_rotational_motors) {
        _X = GetMotorIdentifier () + "RotX";
        _Y = GetMotorIdentifier () + "RotY";
        _Z = GetMotorIdentifier () + "RotZ";
      }
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _X);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Y);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Z);
    }

    public override string GetMotorIdentifier () {
      return name + "Rigidbody";
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < _min_strength || motion.Strength > _max_strength) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      if (_debug)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (!_rotational_motors) {
        if (motion.GetMotorName () == _X) {
          if (_relative_to == Space.World) {
            _rigidbody.AddForce (Vector3.left * motion.Strength);
          } else {
            _rigidbody.AddRelativeForce (Vector3.left * motion.Strength);
          }
        } else if (motion.GetMotorName () == _Y) {
          if (_relative_to == Space.World) {
            _rigidbody.AddForce (Vector3.up * motion.Strength);
          } else {
            _rigidbody.AddRelativeForce (Vector3.up * motion.Strength);
          }
        } else if (motion.GetMotorName () == _Z) {
          if (_relative_to == Space.World) {
            _rigidbody.AddForce (Vector3.forward * motion.Strength);
          } else {
            _rigidbody.AddRelativeForce (Vector3.up * motion.Strength);
          }
        }
      } else {
        if (motion.GetMotorName () == _X) {
          if (_relative_to == Space.World) {
            _rigidbody.AddTorque (Vector3.left * motion.Strength);
          } else {
            _rigidbody.AddRelativeTorque (Vector3.left * motion.Strength);
          }
        } else if (motion.GetMotorName () == _Y) {
          if (_relative_to == Space.World) {
            _rigidbody.AddTorque (Vector3.up * motion.Strength);
          } else {
            _rigidbody.AddRelativeTorque (Vector3.up * motion.Strength);
          }
        } else if (motion.GetMotorName () == _Z) {
          if (_relative_to == Space.World) {
            _rigidbody.AddTorque (Vector3.forward * motion.Strength);
          } else {
            _rigidbody.AddRelativeTorque (Vector3.forward * motion.Strength);
          }
        }
      }

      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }
  }
}
