using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Motors {
  [RequireComponent(typeof(Rigidbody))]
  public class TriRigidbodyMotor : Motor {
    [SerializeField] protected Space _relative_to = Space.Self;

    [SerializeField] protected Rigidbody _rigidbody;

    [SerializeField] protected bool _rotational_motors;

    string _x;
    string _y;
    string _z;

    protected override void Start() { this._rigidbody = this.GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      this._x = this.GetMotorIdentifier() + "X";
      this._y = this.GetMotorIdentifier() + "Y";
      this._z = this.GetMotorIdentifier() + "Z";
      if (this._rotational_motors) {
        this._x = this.GetMotorIdentifier() + "RotX";
        this._y = this.GetMotorIdentifier() + "RotY";
        this._z = this.GetMotorIdentifier() + "RotZ";
      }

      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
    }

    public override string GetMotorIdentifier() { return this.name + "Rigidbody"; }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (!this._rotational_motors) {
        if (motion.GetMotorName() == this._x) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
        } else if (motion.GetMotorName() == this._y) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
        } else if (motion.GetMotorName() == this._z) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.forward * motion.Strength);
        }
      } else {
        if (motion.GetMotorName() == this._x) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
        } else if (motion.GetMotorName() == this._y) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
        } else if (motion.GetMotorName() == this._z) {
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
        }
      }
    }
  }
}
