using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(Rigidbody))]
  public class HexRigidbodyMotor : RigidbodyMotor {

    string _X;
    string _Y;
    string _Z;
    string _RotX;
    string _RotY;
    string _RotZ;


    public override void RegisterComponent () {
      _X = GetMotorIdentifier () + "X";
      _Y = GetMotorIdentifier () + "Y";
      _Z = GetMotorIdentifier () + "Z";
      _RotX = GetMotorIdentifier () + "RotX";
      _RotY = GetMotorIdentifier () + "RotY";
      _RotZ = GetMotorIdentifier () + "RotZ";
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _X);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Y);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Z);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _RotX);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _RotY);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _RotZ);
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
      if (motion.GetMotorName () == _X) {
        _rigidbody.AddForce (Vector3.left * motion.Strength);
      } else if (motion.GetMotorName () == _Y) {
        _rigidbody.AddForce (Vector3.up * motion.Strength);
      } else if (motion.GetMotorName () == _Z) {
        _rigidbody.AddForce (Vector3.forward * motion.Strength);
      } else if (motion.GetMotorName () == _RotX) {
        _rigidbody.AddTorque (Vector3.left * motion.Strength);
      } else if (motion.GetMotorName () == _RotY) {
        _rigidbody.AddTorque (Vector3.up * motion.Strength);
      } else if (motion.GetMotorName () == _RotZ) {
        _rigidbody.AddTorque (Vector3.forward * motion.Strength);
      }

      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }
  }
}