using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Motors {
  [RequireComponent(typeof(Rigidbody))]
  public class HexRigidbodyMotor : Motor {
    [SerializeField] protected Space _relative_to = Space.Self;

    [SerializeField] protected Rigidbody _rigidbody;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    protected override void Start() { this._rigidbody = this.GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      this.ParentActor = NeodroidUtilities.MaybeRegisterComponent(this.ParentActor, (Motor)this);

      this._x = this.GetMotorIdentifier() + "X";
      this._y = this.GetMotorIdentifier() + "Y";
      this._z = this.GetMotorIdentifier() + "Z";
      this._rot_x = this.GetMotorIdentifier() + "RotX";
      this._rot_y = this.GetMotorIdentifier() + "RotY";
      this._rot_z = this.GetMotorIdentifier() + "RotZ";
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

    public override string GetMotorIdentifier() { return this.name + "Rigidbody"; }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.GetMotorName() == this._x)
        this._rigidbody.AddForce(Vector3.left * motion.Strength);
      else if (motion.GetMotorName() == this._y)
        this._rigidbody.AddForce(Vector3.up * motion.Strength);
      else if (motion.GetMotorName() == this._z)
        this._rigidbody.AddForce(Vector3.forward * motion.Strength);
      else if (motion.GetMotorName() == this._rot_x)
        this._rigidbody.AddTorque(Vector3.left * motion.Strength);
      else if (motion.GetMotorName() == this._rot_y)
        this._rigidbody.AddTorque(Vector3.up * motion.Strength);
      else if (motion.GetMotorName() == this._rot_z)
        this._rigidbody.AddTorque(Vector3.forward * motion.Strength);
    }
  }
}
