
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  public class TriTransformMotor : Motor {

    string _X;
    string _Y;
    string _Z;

    [SerializeField]
    protected Space _relative_to = Space.Self;
    [SerializeField]
    protected bool _rotational_motors = false;
    [SerializeField]
    protected bool _no_collisions = true;
    [SerializeField]
    protected string _layer_mask = "Obstructions";

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

    public override void InnerApplyMotion (MotorMotion motion) {
      int layer_mask = 1 << LayerMask.NameToLayer (_layer_mask);
      if (!_rotational_motors) {
        if (motion.GetMotorName () == _X) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, Vector3.left, motion.Strength, layer_mask)) {
              transform.Translate (Vector3.left * motion.Strength, _relative_to);
            }
          } else {
            transform.Translate (Vector3.left * motion.Strength, _relative_to);
          }
        } else if (motion.GetMotorName () == _Y) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, -Vector3.up, motion.Strength, layer_mask)) {
              transform.Translate (-Vector3.up * motion.Strength, _relative_to);
            }
          } else {
            transform.Translate (-Vector3.up * motion.Strength, _relative_to);
          }
        } else if (motion.GetMotorName () == _Z) {
          if (_no_collisions) {
            if (!Physics.Raycast (transform.position, -Vector3.forward, motion.Strength, layer_mask)) {
              transform.Translate (-Vector3.forward * motion.Strength, _relative_to);
            }
          } else {
            transform.Translate (-Vector3.forward * motion.Strength, _relative_to);
          }
        }
      } else {
        if (motion.GetMotorName () == _X) {
          transform.Rotate (Vector3.left, motion.Strength, _relative_to);
        } else if (motion.GetMotorName () == _Y) {
          transform.Rotate (Vector3.up, motion.Strength, _relative_to);
        } else if (motion.GetMotorName () == _Z) {
          transform.Rotate (Vector3.forward, motion.Strength, _relative_to);
        }
      }
    }
  }
}

