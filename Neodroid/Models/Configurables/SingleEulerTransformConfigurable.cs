using System;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
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
      if (this._use_bounding_box_for_range) {
        if (this._bounding_box != null) {
          var valid_input = new InputRange {
              MaxValue = Math.Min(
                  this._bounding_box._bounds.size.x,
                  Math.Min(this._bounding_box._bounds.size.y, this._bounding_box._bounds.size.z))
          };
          valid_input.MinValue = -valid_input.MaxValue;
          this.ValidInput = valid_input;
        }
      }
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (configuration.ConfigurableValue < this.ValidInput.MinValue
          || configuration.ConfigurableValue > this.ValidInput.MaxValue) {
        print(
            string.Format(
                "It does not accept input, outside allowed range {0} to {1}",
                this.ValidInput.MinValue,
                this.ValidInput.MaxValue));
        return; // Do nothing
      }

      if (this.Debugging)
        print("Applying " + configuration + " To " + this.ConfigurableIdentifier);
      var pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      var dir = this.ParentEnvironment.TransformDirection(this.transform.forward);
      switch (this._axis_of_configuration) {
        case Axis.X:
          pos.Set(configuration.ConfigurableValue - pos.x, pos.y, pos.z);
          break;
        case Axis.Y:
          pos.Set(pos.x, configuration.ConfigurableValue - pos.y, pos.z);
          break;
        case Axis.Z:
          pos.Set(pos.x, pos.y, configuration.ConfigurableValue - pos.z);
          break;
        case Axis.RotX:
          dir.Set(configuration.ConfigurableValue - dir.x, dir.y, dir.z);
          break;
        case Axis.RotY:
          dir.Set(dir.x, configuration.ConfigurableValue - dir.y, dir.z);
          break;
        case Axis.RotZ:
          dir.Set(dir.x, dir.y, configuration.ConfigurableValue - dir.z);
          break;
        default:
          break;
      }

      var inv_pos = this.ParentEnvironment.InverseTransformPosition(pos);
      var inv_dir = this.ParentEnvironment.InverseTransformDirection(dir);
      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.Rotate(inv_dir);
    }
  }
}
