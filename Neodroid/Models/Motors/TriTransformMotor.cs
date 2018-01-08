using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Motors {
  public class TriTransformMotor : Motor {
    [SerializeField] protected string _layer_mask = "Obstructions";

    [SerializeField] protected bool _no_collisions = true;

    [SerializeField] protected Space _relative_to = Space.Self;

    [SerializeField] protected bool _rotational_motors;

    string _x;
    string _y;
    string _z;

    public override void RegisterComponent() {
      if (!this._rotational_motors) {
        this._x = this.GetMotorIdentifier() + "X";
        this._y = this.GetMotorIdentifier() + "Y";
        this._z = this.GetMotorIdentifier() + "Z";
      } else {
        this._x = this.GetMotorIdentifier() + "RotX";
        this._y = this.GetMotorIdentifier() + "RotY";
        this._z = this.GetMotorIdentifier() + "RotZ";
      }

      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                       r : this.ParentActor,
                                                                       c : (Motor)this,
                                                                       identifier : this._x);
      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                       r : this.ParentActor,
                                                                       c : (Motor)this,
                                                                       identifier : this._y);
      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                       r : this.ParentActor,
                                                                       c : (Motor)this,
                                                                       identifier : this._z);
    }

    public override string GetMotorIdentifier() { return this.name + "Transform"; }

    public override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(layerName : this._layer_mask);
      if (!this._rotational_motors) {
        if (motion.GetMotorName() == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._no_collisions) {
            if (!Physics.Raycast(
                                 origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask))
              this.transform.Translate(
                                       translation : vec,
                                       relativeTo : this._relative_to);
          } else {
            this.transform.Translate(
                                     translation : vec,
                                     relativeTo : this._relative_to);
          }
        } else if (motion.GetMotorName() == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._no_collisions) {
            if (!Physics.Raycast(
                                 origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask))
              this.transform.Translate(
                                       translation : vec,
                                       relativeTo : this._relative_to);
          } else {
            this.transform.Translate(
                                     translation : vec,
                                     relativeTo : this._relative_to);
          }
        } else if (motion.GetMotorName() == this._z) {
          var vec = -Vector3.forward * motion.Strength;
          if (this._no_collisions) {
            if (!Physics.Raycast(
                                 origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask))
              this.transform.Translate(
                                       translation : vec,
                                       relativeTo : this._relative_to);
          } else {
            this.transform.Translate(
                                     translation : vec,
                                     relativeTo : this._relative_to);
          }
        }
      } else {
        if (motion.GetMotorName() == this._x)
          this.transform.Rotate(
                                axis : Vector3.left,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
        else if (motion.GetMotorName() == this._y)
          this.transform.Rotate(
                                axis : Vector3.up,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
        else if (motion.GetMotorName() == this._z)
          this.transform.Rotate(
                                axis : Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
      }
    }
  }
}
