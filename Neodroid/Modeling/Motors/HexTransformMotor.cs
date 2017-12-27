
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  public class HexTransformMotor : Motor {

    [SerializeField]
    protected Space _relative_to = Space.Self;
    [SerializeField]
    protected bool _no_collisions = true;
    [SerializeField]
    protected string _layer_mask = "Obstructions";

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
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _X);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Y);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _Z);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _RotX);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _RotY);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (ParentActor, (Motor)this, _RotZ);
    }

    public override string GetMotorIdentifier () {
      return name + "Transform";
    }

    public override void InnerApplyMotion (MotorMotion motion) {
      if (motion.GetMotorName () == _X) {
        transform.Translate (Vector3.left * motion.Strength, _relative_to);
      } else if (motion.GetMotorName () == _Y) {
        transform.Translate (-Vector3.up * motion.Strength, _relative_to);
      } else if (motion.GetMotorName () == _Z) {
        transform.Translate (-Vector3.forward * motion.Strength, _relative_to);
      } else if (motion.GetMotorName () == _RotX) {
        transform.Rotate (Vector3.left, motion.Strength, _relative_to);
      } else if (motion.GetMotorName () == _RotY) {
        transform.Rotate (Vector3.up, motion.Strength, _relative_to);
      } else if (motion.GetMotorName () == _RotZ) {
        transform.Rotate (Vector3.forward, motion.Strength, _relative_to);
      }
    }
  }
}

