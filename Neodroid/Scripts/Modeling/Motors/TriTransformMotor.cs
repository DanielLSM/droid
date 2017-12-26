using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  public class TriTransformMotor : EulerTransformMotor {

    string _X;
    string _Y;
    string _Z;

    public bool _rotational_motors = false;
    public bool _no_collisions = true;
    public string _layer_mask = "Obstructions";

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
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _X);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Y);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Z);
    }

    public override string GetMotorIdentifier () {
      return name + "Transform";
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      int layer_mask = 1 << LayerMask.NameToLayer (_layer_mask);
      if (Debugging)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (!_rotational_motors) {
        if (motion.GetMotorName () == _X) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, Vector3.left, motion.Strength, layer_mask)) {
              transform.Translate (Vector3.left * motion.Strength, _space);
            }
          } else {
            transform.Translate (Vector3.left * motion.Strength, _space);
          }
        } else if (motion.GetMotorName () == _Y) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, -Vector3.up, motion.Strength, layer_mask)) {
              transform.Translate (-Vector3.up * motion.Strength, _space);
            }
          } else {
            transform.Translate (-Vector3.up * motion.Strength, _space);
          }
        } else if (motion.GetMotorName () == _Z) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, -Vector3.forward, motion.Strength, layer_mask)) {
              transform.Translate (-Vector3.forward * motion.Strength, _space);
            }
          } else {
            transform.Translate (-Vector3.forward * motion.Strength, _space);
          }
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

      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }
  }
}

