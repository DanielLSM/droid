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
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _X);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Y);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Z);
    }

    public override string GetMotorIdentifier () {
      return name + "Rigidbody";
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      if (Debugging)
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

      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }
  }
}
