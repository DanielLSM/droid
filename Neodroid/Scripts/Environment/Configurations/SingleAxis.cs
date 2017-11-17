using System;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Configurations {
  public class SingleAxis : Configurable {

    public Axis _axis_of_configuration;

    public SingleAxis () {
    }

    public override void ApplyConfiguration (float value) { 
      if (_debug)
        Debug.Log ("Applying " + value + " To " + name);
      var e = transform.rotation.eulerAngles;
      switch (_axis_of_configuration) {
      case Axis.X:
        transform.position.Set (value, transform.position.y, transform.position.z);
        break;
      case Axis.Y:
        transform.position.Set (transform.position.x, value, transform.position.z);
        break;
      case Axis.Z:
        transform.position.Set (transform.position.x, transform.position.y, value);
        break;
      case Axis.RotX:
        e.Set (value, e.y, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      case Axis.RotY:
        e.Set (e.x, value, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      case Axis.RotZ:
        e.Set (e.x, e.y, value);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
        break;
      default:
        break;
      }
    }
  }
}

