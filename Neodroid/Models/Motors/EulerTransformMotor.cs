﻿using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Models.Motors {
  public class EulerTransformMotor : Motor {
    [SerializeField] protected Axis _axis_of_motion;

    [SerializeField] protected string _layer_mask = "Obstructions";

    [SerializeField] protected bool _no_collisions = true;

    [SerializeField] protected Space _relative_to = Space.Self;

    public override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(layerName : this._layer_mask);
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
          this.transform.Rotate(
                                axis : Vector3.left,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
          break;
        case Axis.RotY:
          this.transform.Rotate(
                                axis : Vector3.up,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
          break;
        case Axis.RotZ:
          this.transform.Rotate(
                                axis : Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._relative_to);
          break;
        default:
          break;
      }

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

    public override string GetMotorIdentifier() { return this.name + "Transform" + this._axis_of_motion; }
  }
}
