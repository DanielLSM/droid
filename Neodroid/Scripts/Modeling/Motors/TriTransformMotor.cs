﻿using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  public class TriTransformMotor : Motor {

    string _X;
    string _Y;
    string _Z;

    public Space _space = Space.Self;
    public bool _rotational_motors = false;

    public override void RegisterComponent () {
      if (!_rotational_motors) {
        _X = GetMotorIdentifier () + "X";
        _Y = GetMotorIdentifier () + "Y";
        _Z = GetMotorIdentifier () + "Z";
      } else {
        _X = GetMotorIdentifier () + "RotX";
        _Y = GetMotorIdentifier () + "RotY";
        _Z = GetMotorIdentifier () + "RotZ";
      }
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _X);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Y);
      _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent (_actor_game_object, (Motor)this, _Z);
    }

    public override string GetMotorIdentifier () {
      return name + "TriTransform";
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (!_bidirectional && motion.Strength < 0) {
        Debug.Log ("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      if (_debug)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (!_rotational_motors) {
        if (motion.GetMotorName () == _X) {
          transform.Translate (Vector3.left * motion.Strength, _space);
        } else if (motion.GetMotorName () == _Y) {
          transform.Translate (-Vector3.up * motion.Strength, _space);
        } else if (motion.GetMotorName () == _Z) {
          transform.Translate (-Vector3.forward * motion.Strength, _space);
        }
      } else {
        if (motion.GetMotorName () == _X) {
          transform.Rotate (Vector3.left, motion.Strength, _space);
        } else if (motion.GetMotorName () == _Y) {
          transform.Rotate (Vector3.up, motion.Strength, _space);
        } else if (motion.GetMotorName () == _Z) {
          transform.Rotate (Vector3.forward, motion.Strength, _space);
        }
      }

      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }
  }
}

