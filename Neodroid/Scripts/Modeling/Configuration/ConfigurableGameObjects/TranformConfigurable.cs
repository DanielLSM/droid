using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.BoundingBoxes;
using System.Collections.Specialized;

namespace Neodroid.Configurations {
  public class TransformConfigurable : ConfigurableGameObject {

    public Axis _axis_of_configuration;
    public bool _use_bounding_box_for_range = false;
    public BoundingBox _bounding_box;

    protected override void Awake () {
      AddToEnvironment ();
      if (_use_bounding_box_for_range) {
        if (_bounding_box != null) {
          _max_value = Math.Min (_bounding_box._bounds.size.x, Math.Min (_bounding_box._bounds.size.y, _bounding_box._bounds.size.z));
          _min_value = -_max_value;
        }
      }
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (configuration.ConfigurableValue < _min_value || configuration.ConfigurableValue > _max_value) {
        Debug.Log (String.Format ("It does not accept input, outside allowed range {0} to {1}", _min_value, _max_value));
        return; // Do nothing
      }
      if (_debug)
        Debug.Log ("Applying " + configuration.ToString () + " To " + GetConfigurableIdentifier ());
      var pos = _environment.TransformPosition (this.transform.position);
      var dir = _environment.TransformDirection (this.transform.forward);
      switch (_axis_of_configuration) {
      case Axis.X:
        pos.Set (configuration.ConfigurableValue - pos.x, pos.y, pos.z);
        break;
      case Axis.Y:
        pos.Set (pos.x, configuration.ConfigurableValue - pos.y, pos.z);
        break;
      case Axis.Z:
        pos.Set (pos.x, pos.y, configuration.ConfigurableValue - pos.z);
        break;
      case Axis.RotX:
        dir.Set (configuration.ConfigurableValue - dir.x, dir.y, dir.z);
        break;
      case Axis.RotY:
        dir.Set (dir.x, configuration.ConfigurableValue - dir.y, dir.z);
        break;
      case Axis.RotZ:
        dir.Set (dir.x, dir.y, configuration.ConfigurableValue - dir.z);
        break;
      default:
        break;
      }
      var inv_pos = _environment.InverseTransformPosition (pos);
      var inv_dir = _environment.InverseTransformDirection (dir);
      transform.position = inv_pos;
      transform.rotation = Quaternion.identity;
      transform.Rotate (inv_dir);
    }

    public override string GetConfigurableIdentifier () {
      return name + "Transform" + _axis_of_configuration.ToString ();
      ;
    }
  }
}

