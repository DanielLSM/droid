using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Motors {
  [RequireComponent (typeof(Rigidbody))]
  public class TriRigidbodyMotor : Motor {
    [SerializeField] protected Space _relative_to = Space.Self;

    [SerializeField] protected Rigidbody _rigidbody;

    [SerializeField] protected bool _rotational_motors;

    string _x;
    string _y;
    string _z;

    protected override void Start () {
      this._rigidbody = this.GetComponent<Rigidbody> ();
    }

    public override void RegisterComponent () {
      this._x = this.GetMotorIdentifier () + "X";
      this._y = this.GetMotorIdentifier () + "Y";
      this._z = this.GetMotorIdentifier () + "Z";
      if (this._rotational_motors) {
        this._x = this.GetMotorIdentifier () + "RotX";
        this._y = this.GetMotorIdentifier () + "RotY";
        this._z = this.GetMotorIdentifier () + "RotZ";
      }

      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (
        r : this.ParentActor,
        c : (Motor)this,
        identifier : this._x);
      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (
        r : this.ParentActor,
        c : (Motor)this,
        identifier : this._y);
      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent (
        r : this.ParentActor,
        c : (Motor)this,
        identifier : this._z);
    }

    public override string GetMotorIdentifier () {
      return this.name + "Rigidbody";
    }

    public override void InnerApplyMotion (MotorMotion motion) {
      if (!this._rotational_motors) {
        if (motion.GetMotorName () == this._x)
        if (this._relative_to == Space.World)
          this._rigidbody.AddForce (force : Vector3.left * motion.Strength);
        else
          this._rigidbody.AddRelativeForce (force : Vector3.left * motion.Strength);
        else if (motion.GetMotorName () == this._y)
        if (this._relative_to == Space.World)
          this._rigidbody.AddForce (force : Vector3.up * motion.Strength);
        else
          this._rigidbody.AddRelativeForce (force : Vector3.up * motion.Strength);
        else if (motion.GetMotorName () == this._z)
        if (this._relative_to == Space.World)
          this._rigidbody.AddForce (force : Vector3.forward * motion.Strength);
        else
          this._rigidbody.AddRelativeForce (force : Vector3.forward * motion.Strength);
      } else {
        if (motion.GetMotorName () == this._x)
        if (this._relative_to == Space.World)
          this._rigidbody.AddTorque (torque : Vector3.left * motion.Strength);
        else
          this._rigidbody.AddRelativeTorque (torque : Vector3.left * motion.Strength);
        else if (motion.GetMotorName () == this._y)
        if (this._relative_to == Space.World)
          this._rigidbody.AddTorque (torque : Vector3.up * motion.Strength);
        else
          this._rigidbody.AddRelativeTorque (torque : Vector3.up * motion.Strength);
        else if (motion.GetMotorName () == this._z)
        if (this._relative_to == Space.World)
          this._rigidbody.AddTorque (torque : Vector3.forward * motion.Strength);
        else
          this._rigidbody.AddRelativeTorque (torque : Vector3.forward * motion.Strength);
      }
    }
  }
}
