﻿using System;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Motors {
  public class HexTransformMotor : Motor {
    [SerializeField] protected string _layer_mask = "Obstructions";

    [SerializeField] protected bool _no_collisions = true;

    [SerializeField] protected Space _relative_to = Space.Self;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    public override String MotorIdentifier { get { return this.name + "Transform"; } }

    public override void RegisterComponent() {
      this._x = this.MotorIdentifier + "X";
      this._y = this.MotorIdentifier + "Y";
      this._z = this.MotorIdentifier + "Z";
      this._rot_x = this.MotorIdentifier + "RotX";
      this._rot_y = this.MotorIdentifier + "RotY";
      this._rot_z = this.MotorIdentifier + "RotZ";
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_z);
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.GetMotorName() == this._x)
        this.transform.Translate(Vector3.left * motion.Strength, this._relative_to);
      else if (motion.GetMotorName() == this._y)
        this.transform.Translate(-Vector3.up * motion.Strength, this._relative_to);
      else if (motion.GetMotorName() == this._z)
        this.transform.Translate(-Vector3.forward * motion.Strength, this._relative_to);
      else if (motion.GetMotorName() == this._rot_x)
        this.transform.Rotate(Vector3.left, motion.Strength, this._relative_to);
      else if (motion.GetMotorName() == this._rot_y)
        this.transform.Rotate(Vector3.up, motion.Strength, this._relative_to);
      else if (motion.GetMotorName() == this._rot_z)
        this.transform.Rotate(Vector3.forward, motion.Strength, this._relative_to);
    }
  }
}
