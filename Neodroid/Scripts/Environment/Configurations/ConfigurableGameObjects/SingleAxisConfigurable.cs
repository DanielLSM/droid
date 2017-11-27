using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public class SingleAxisConfigurable : ConfigurableGameObject {

    public Axis _axis_of_configuration;

    public override void ApplyConfiguration (Configuration configuration) { 
      if (_debug)
        Debug.Log ("Applying " + configuration.ToString () + " To " + GetConfigurableIdentifier ());
      var pos = _environment_manager.TransformPosition (this.transform.position);
      var dir = _environment_manager.TransformDirection (this.transform.forward);
      switch (_axis_of_configuration) {
      case Axis.X:
        pos.Set (configuration.ConfigurableValue, pos.y, pos.z);
        break;
      case Axis.Y:
        pos.Set (pos.x, configuration.ConfigurableValue, pos.z);
        break;
      case Axis.Z:
        pos.Set (pos.x, pos.y, configuration.ConfigurableValue);
        break;
      case Axis.RotX:
        dir.Set (configuration.ConfigurableValue, dir.y, dir.z);
        break;
      case Axis.RotY:
        dir.Set (dir.x, configuration.ConfigurableValue, dir.z);
        break;
      case Axis.RotZ:
        dir.Set (dir.x, dir.y, configuration.ConfigurableValue);
        break;
      default:
        break;
      }
      var inv_pos = _environment_manager.InverseTransformPosition (pos);
      var inv_dir = _environment_manager.InverseTransformDirection (dir);
      transform.position = inv_pos;
      transform.rotation = Quaternion.identity;
      transform.Rotate (inv_dir);
    }

    public override string GetConfigurableIdentifier () {
      return name + "SingleAxisConfigurable" + _axis_of_configuration.ToString ();
      ;
    }
  }
}

