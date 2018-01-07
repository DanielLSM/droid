using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  public class TriTransformMotor : Motor {
    [SerializeField]
    protected string _layer_mask = "Obstructions";

    [SerializeField]
    protected bool _no_collisions = true;

    [SerializeField]
    protected Space _relative_to = Space.Self;

    [SerializeField]
    protected bool _rotational_motors;

    private string _X;
    private string _Y;
    private string _Z;

    public override void RegisterComponent() {
      if (!_rotational_motors) {
        _X = GetMotorIdentifier() + "X";
        _Y = GetMotorIdentifier() + "Y";
        _Z = GetMotorIdentifier() + "Z";
      } else {
        _X = GetMotorIdentifier() + "RotX";
        _Y = GetMotorIdentifier() + "RotY";
        _Z = GetMotorIdentifier() + "RotZ";
      }

      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _X);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _Y);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _Z);
    }

    public override string GetMotorIdentifier() { return name + "Transform"; }

    public override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(_layer_mask);
      if (!_rotational_motors) {
        if (motion.GetMotorName() == _X) {
          var vec = Vector3.right * motion.Strength;
          if (_no_collisions) {
            if (!Physics.Raycast(
                                 transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask))
              transform.Translate(
                                  vec,
                                  _relative_to);
          } else {
            transform.Translate(
                                vec,
                                _relative_to);
          }
        } else if (motion.GetMotorName() == _Y) {
          var vec = -Vector3.up * motion.Strength;
          if (_no_collisions) {
            if (!Physics.Raycast(
                                 transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask))
              transform.Translate(
                                  vec,
                                  _relative_to);
          } else {
            transform.Translate(
                                vec,
                                _relative_to);
          }
        } else if (motion.GetMotorName() == _Z) {
          var vec = -Vector3.forward * motion.Strength;
          if (_no_collisions) {
            if (!Physics.Raycast(
                                 transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask))
              transform.Translate(
                                  vec,
                                  _relative_to);
          } else {
            transform.Translate(
                                vec,
                                _relative_to);
          }
        }
      } else {
        if (motion.GetMotorName() == _X)
          transform.Rotate(
                           Vector3.left,
                           motion.Strength,
                           _relative_to);
        else if (motion.GetMotorName() == _Y)
          transform.Rotate(
                           Vector3.up,
                           motion.Strength,
                           _relative_to);
        else if (motion.GetMotorName() == _Z)
          transform.Rotate(
                           Vector3.forward,
                           motion.Strength,
                           _relative_to);
      }
    }
  }
}
