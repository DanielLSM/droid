using System;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using Neodroid.Scripts.Utilities.Enums;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class SingleAxisTransformConfigurable : ConfigurableGameObject, IHasSingle {
    #region Fields
    [SerializeField] Axis _axis_of_configuration;
    [SerializeField] BoundingBox _bounding_box;
    [SerializeField] bool _use_bounding_box_for_range;
    [SerializeField] float _observation_value;
    [SerializeField] bool _use_environments_space = false;
    #endregion

    public override string ConfigurableIdentifier {
      get { return this.name + "Transform" + this._axis_of_configuration; }
    }

    public override void UpdateObservation() {
      var pos = this.transform.position;
      var dir = this.transform.forward;
      var rot = this.transform.up;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition(pos);
        dir = this.ParentEnvironment.TransformDirection(dir);
        rot = this.ParentEnvironment.TransformDirection(rot);
      }

      switch (this._axis_of_configuration) {
        case Axis.X:
          this.ObservationValue = pos.x;
          break;
        case Axis.Y:
          this.ObservationValue = pos.y;
          break;
        case Axis.Z:
          this.ObservationValue = pos.z;
          break;
        case Axis.DirX:
          this.ObservationValue = dir.x;
          break;
        case Axis.DirY:
          this.ObservationValue = dir.y;
          break;
        case Axis.DirZ:
          this.ObservationValue = dir.z;
          break;
        case Axis.RotX:
          this.ObservationValue = rot.x;
          break;
        case Axis.RotY:
          this.ObservationValue = rot.y;
          break;
        case Axis.RotZ:
          this.ObservationValue = rot.z;
          break;
        default: throw new ArgumentOutOfRangeException();
      }


    }


    protected override void Awake () {
      this.AddToEnvironment ();
      if (this._use_bounding_box_for_range) {
        if (this._bounding_box != null) {
          var valid_input = new SingleSpace {
            MaxValue = Math.Min (
              this._bounding_box._bounds.size.x,
              Math.Min (this._bounding_box._bounds.size.y, this._bounding_box._bounds.size.z))
          };
          valid_input.MinValue = -valid_input.MaxValue;
          this.ConfigurableSpace = valid_input;
        }
      }
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (configuration.ConfigurableValue < this.ConfigurableSpace.MinValue
          || configuration.ConfigurableValue > this.ConfigurableSpace.MaxValue) {
        print (
          string.Format (
            "It does not accept input, outside allowed range {0} to {1}",
            this.ConfigurableSpace.MinValue,
            this.ConfigurableSpace.MaxValue));
        return; // Do nothing
      }

      if (this.Debugging)
        print ("Applying " + configuration + " To " + this.ConfigurableIdentifier);
      var pos = this.transform.position;
      var dir = this.transform.forward;
      var rot = this.transform.up;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition (pos);
        dir = this.ParentEnvironment.TransformDirection (dir);
        rot = this.ParentEnvironment.TransformDirection (rot);
      }

      switch (this._axis_of_configuration) {
      case Axis.X:
        pos.Set (configuration.ConfigurableValue - pos.x, pos.y, pos.z);
        break;
      case Axis.Y:
        pos.Set (pos.x, configuration.ConfigurableValue - pos.y, pos.z);
        break;
      case Axis.Z:
        pos.Set (pos.x, pos.y, configuration.ConfigurableValue - pos.z);
        break;
      case Axis.DirX:
        dir.Set (configuration.ConfigurableValue - dir.x, dir.y, dir.z);
        break;
      case Axis.DirY:
        dir.Set (dir.x, configuration.ConfigurableValue - dir.y, dir.z);
        break;
      case Axis.DirZ:
        dir.Set (dir.x, dir.y, configuration.ConfigurableValue - dir.z);
        break;
      case Axis.RotX:
        rot.Set (configuration.ConfigurableValue - rot.x, rot.y, rot.z);
        break;
      case Axis.RotY:
        rot.Set (rot.x, configuration.ConfigurableValue - rot.y, rot.z);
        break;
      case Axis.RotZ:
        rot.Set (rot.x, rot.y, configuration.ConfigurableValue - rot.z);
        break;
        default: throw new ArgumentOutOfRangeException();
      }
      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPosition (inv_pos);
        inv_dir = this.ParentEnvironment.InverseTransformDirection (inv_dir);
        inv_rot = this.ParentEnvironment.InverseTransformDirection (inv_rot);
      }
      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }

    public float ObservationValue { get { return this._observation_value;}
      private set { this._observation_value = value; }
    }
  }
}
