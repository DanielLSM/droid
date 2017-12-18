using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  public class HexTransformMotor : EulerTransformMotor {

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
      return name + "Transform";
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < _min_strength || motion.Strength > _max_strength) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      if (_debug)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (motion.GetMotorName () == _X) {
        transform.Translate (Vector3.left * motion.Strength, _space);
      } else if (motion.GetMotorName () == _Y) {
        transform.Translate (-Vector3.up * motion.Strength, _space);
      } else if (motion.GetMotorName () == _Z) {
        transform.Translate (-Vector3.forward * motion.Strength, _space);
      } else if (motion.GetMotorName () == _RotX) {
        transform.Rotate (Vector3.left, motion.Strength, _space);
      } else if (motion.GetMotorName () == _RotY) {
        transform.Rotate (Vector3.up, motion.Strength, _space);
      } else if (motion.GetMotorName () == _RotZ) {
        transform.Rotate (Vector3.forward, motion.Strength, _space);
      }

      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }
  }
}

