﻿using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent(typeof(Rigidbody))]
  public class HexRigidbodyMotor : Motor {
    [SerializeField]
    protected Space _relative_to = Space.Self;

    [SerializeField]
    protected Rigidbody _rigidbody;

    private string _RotX;
    private string _RotY;
    private string _RotZ;

    private string _X;
    private string _Y;
    private string _Z;

    protected override void Start() { _rigidbody = GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      _X = GetMotorIdentifier() + "X";
      _Y = GetMotorIdentifier() + "Y";
      _Z = GetMotorIdentifier() + "Z";
      _RotX = GetMotorIdentifier() + "RotX";
      _RotY = GetMotorIdentifier() + "RotY";
      _RotZ = GetMotorIdentifier() + "RotZ";
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
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _RotX);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _RotY);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _RotZ);
    }

    public override string GetMotorIdentifier() { return name + "Rigidbody"; }

    public override void InnerApplyMotion(MotorMotion motion) {
      if (motion.GetMotorName() == _X)
        _rigidbody.AddForce(Vector3.left * motion.Strength);
      else if (motion.GetMotorName() == _Y)
        _rigidbody.AddForce(Vector3.up * motion.Strength);
      else if (motion.GetMotorName() == _Z)
        _rigidbody.AddForce(Vector3.forward * motion.Strength);
      else if (motion.GetMotorName() == _RotX)
        _rigidbody.AddTorque(Vector3.left * motion.Strength);
      else if (motion.GetMotorName() == _RotY)
        _rigidbody.AddTorque(Vector3.up * motion.Strength);
      else if (motion.GetMotorName() == _RotZ)
        _rigidbody.AddTorque(Vector3.forward * motion.Strength);
    }
  }
}
