using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public class HexTransformConfigurable : EulerTransformConfigurable {
    string _X;
    string _Y;
    string _Z;
    string _RotX;
    string _RotY;
    string _RotZ;

    protected override void AddToEnvironment () {
      _X = GetConfigurableIdentifier () + "X";
      _Y = GetConfigurableIdentifier () + "Y";
      _Z = GetConfigurableIdentifier () + "Z";
      _RotX = GetConfigurableIdentifier () + "RotX";
      _RotY = GetConfigurableIdentifier () + "RotY";
      _RotZ = GetConfigurableIdentifier () + "RotZ";
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _X);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _Y);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _Z);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _RotX);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _RotY);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _RotZ);
    }

    public override void ApplyConfiguration (Configuration configuration) {
      var pos = _environment.TransformPosition (this.transform.position);
      var dir = _environment.TransformDirection (this.transform.forward);
      var v = configuration.ConfigurableValue;
      if (_decimal_granularity >= 0) {
        v = (float)Math.Round (v, _decimal_granularity);
      }
      if (_min_value.CompareTo (_max_value) != 0) {
        if (v < _min_value || v > _max_value) {
          Debug.Log (String.Format ("Configurable does not accept input{2}, outside allowed range {0} to {1}", _min_value, _max_value, v));
          return; // Do nothing
        }
      }
      if (_debug)
        Debug.Log ("Applying " + v.ToString () + " To " + GetConfigurableIdentifier ());
      if (_relative_to_existing_value) {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v - pos.x, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v - pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v - pos.z);
        } else if (configuration.ConfigurableName == _RotX) {
          dir.Set (v - dir.x, dir.y, dir.z);
        } else if (configuration.ConfigurableName == _RotY) {
          dir.Set (dir.x, v - dir.y, dir.z);
        } else if (configuration.ConfigurableName == _RotZ) {
          dir.Set (dir.x, dir.y, v - dir.z);
        }
      } else {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v);
        } else if (configuration.ConfigurableName == _RotX) {
          dir.Set (v, dir.y, dir.z);
        } else if (configuration.ConfigurableName == _RotY) {
          dir.Set (dir.x, v, dir.z);
        } else if (configuration.ConfigurableName == _RotZ) {
          dir.Set (dir.x, dir.y, v);
        }
      }
      var inv_pos = _environment.InverseTransformPosition (pos);
      var inv_dir = _environment.InverseTransformDirection (dir);
      transform.position = inv_pos;
      transform.rotation = Quaternion.identity;
      transform.Rotate (inv_dir);
    }

    public override string GetConfigurableIdentifier () {
      return name + "Transform";
    }
  }
}