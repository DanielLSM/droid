using System;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Models.Motors {
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyMotor : Motor {
    [SerializeField] protected Axis _axis_of_motion;

    [SerializeField] protected Space _relative_to = Space.Self;

    [SerializeField] protected Rigidbody _rigidbody;

    protected override void Start() { this._rigidbody = this.GetComponent<Rigidbody>(); }

    public override void InnerApplyMotion(MotorMotion motion) {
      switch (this._axis_of_motion) {
        case Axis.X:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
          break;
        case Axis.Y:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Z:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.RotX:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
          break;
        case Axis.RotY:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
          break;
        case Axis.RotZ:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override string GetMotorIdentifier() { return this.name + "Rigidbody" + this._axis_of_motion; }
  }
}
