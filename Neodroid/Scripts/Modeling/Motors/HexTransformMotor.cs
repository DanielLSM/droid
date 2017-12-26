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

    public override void ApplyMotion (MotorMotion motion) {
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      if (Debugging)
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

      EnergySpendSinceReset += EnergyCost * motion.Strength;
    }
  }
}

