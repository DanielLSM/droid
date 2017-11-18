using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public class SingleAxisConfigurable : ConfigurableGameObject {

    public Axis _axis_of_configuration;

    public SingleAxisConfigurable () {
    }

    public override void ApplyConfiguration (Configuration configuration) { 
      if (_debug)
        Debug.Log ("Applying " + configuration.ConfigurableValue + " To " + GetConfigurableIdentifier ());
      var e = transform.rotation.eulerAngles;
      var pos = transform.position;
      switch (_axis_of_configuration) {
      case Axis.X:
        pos.Set (configuration.ConfigurableValue, transform.position.y, transform.position.z);
        transform.position = pos;
        break;
      case Axis.Y:
        transform.position.Set (transform.position.x, configuration.ConfigurableValue, transform.position.z);
        break;
      case Axis.Z:
        transform.position.Set (transform.position.x, transform.position.y, configuration.ConfigurableValue);
        break;
      case Axis.RotX:
        e.Set (configuration.ConfigurableValue, e.y, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      case Axis.RotY:
        e.Set (e.x, configuration.ConfigurableValue, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      case Axis.RotZ:
        e.Set (e.x, e.y, configuration.ConfigurableValue);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      default:
        break;
      }
    }

    public override string GetConfigurableIdentifier () {
      return name + "SingleAxisConfigurable" + _axis_of_configuration.ToString ();
      ;
    }
  }
}

