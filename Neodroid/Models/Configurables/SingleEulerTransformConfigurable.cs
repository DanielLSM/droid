using System;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using Neodroid.Scripts.Utilities.Enums;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class SingleEulerTransformConfigurable : ConfigurableGameObject {
    [SerializeField] Axis _axis_of_configuration;
    [SerializeField] BoundingBox _bounding_box;
    [SerializeField] bool _use_bounding_box_for_range;

    [SerializeField] Vector3 current_value;

    public override string ConfigurableIdentifier {
      get { return this.name + "Transform" + this._axis_of_configuration; }
    }

    protected override void Awake() {
      this.AddToEnvironment();
      if (this._use_bounding_box_for_range)
        if (this._bounding_box != null) {
          var valid_input = new InputRange {
                                             MaxValue = Math.Min(
                                                                  val1 : this._bounding_box._bounds.size.x,
                                                                  val2 : Math.Min(
                                                                                  val1 : this
                                                                                         ._bounding_box
                                                                                         ._bounds.size.y,
                                                                                  val2 : this
                                                                                         ._bounding_box
                                                                                         ._bounds.size.z))
                                           };
          valid_input.MinValue = -valid_input.MaxValue;
          this.ValidInput = valid_input;
        }
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (configuration.ConfigurableValue < this.ValidInput.MinValue
          || configuration.ConfigurableValue > this.ValidInput.MaxValue) {
        print(
              message : string.Format(
                                      format : "It does not accept input, outside allowed range {0} to {1}",
                                      arg0 : this.ValidInput.MinValue,
                                      arg1 : this.ValidInput.MaxValue));
        return; // Do nothing
      }

      if (this.Debugging)
        print(message : "Applying " + configuration + " To " + this.ConfigurableIdentifier);
      var pos = this.ParentEnvironment.TransformPosition(position : this.transform.position);
      var dir = this.ParentEnvironment.TransformDirection(direction : this.transform.forward);
      switch (this._axis_of_configuration) {
        case Axis.X:
          pos.Set(
                  newX : configuration.ConfigurableValue - pos.x,
                  newY : pos.y,
                  newZ : pos.z);
          break;
        case Axis.Y:
          pos.Set(
                  newX : pos.x,
                  newY : configuration.ConfigurableValue - pos.y,
                  newZ : pos.z);
          break;
        case Axis.Z:
          pos.Set(
                  newX : pos.x,
                  newY : pos.y,
                  newZ : configuration.ConfigurableValue - pos.z);
          break;
        case Axis.RotX:
          dir.Set(
                  newX : configuration.ConfigurableValue - dir.x,
                  newY : dir.y,
                  newZ : dir.z);
          break;
        case Axis.RotY:
          dir.Set(
                  newX : dir.x,
                  newY : configuration.ConfigurableValue - dir.y,
                  newZ : dir.z);
          break;
        case Axis.RotZ:
          dir.Set(
                  newX : dir.x,
                  newY : dir.y,
                  newZ : configuration.ConfigurableValue - dir.z);
          break;
        default:
          break;
      }

      var inv_pos = this.ParentEnvironment.InverseTransformPosition(position : pos);
      var inv_dir = this.ParentEnvironment.InverseTransformDirection(direction : dir);
      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.Rotate(eulerAngles : inv_dir);
    }
  }
}
