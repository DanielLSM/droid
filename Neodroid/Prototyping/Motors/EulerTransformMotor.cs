using System;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Models.Motors {
  public class EulerTransformMotor : Motor {
    [SerializeField] protected Axis _axis_of_motion;

    [SerializeField] protected string _layer_mask = "Obstructions";

    [SerializeField] protected bool _no_collisions = true;

    [SerializeField] protected Space _relative_to = Space.Self;

    public override String MotorIdentifier { get { return this.name + "Transform" + this._axis_of_motion; } }

    protected override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._layer_mask);
      var vec = Vector3.zero;
      switch (this._axis_of_motion) {
        case Axis.X:
          vec = Vector3.right * motion.Strength;
          break;
        case Axis.Y:
          vec = -Vector3.up * motion.Strength;
          break;
        case Axis.Z:
          vec = -Vector3.forward * motion.Strength;
          break;
        case Axis.RotX:
          this.transform.Rotate(Vector3.left, motion.Strength, this._relative_to);
          break;
        case Axis.RotY:
          this.transform.Rotate(Vector3.up, motion.Strength, this._relative_to);
          break;
        case Axis.RotZ:
          this.transform.Rotate(Vector3.forward, motion.Strength, this._relative_to);
          break;
        default:
          break;
      }

      if (this._no_collisions) {
        if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask))
          this.transform.Translate(vec, this._relative_to);
      } else
        this.transform.Translate(vec, this._relative_to);
    }
  }
}
