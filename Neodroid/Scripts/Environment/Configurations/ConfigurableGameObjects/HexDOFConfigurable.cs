using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public class HexDOFConfigurable : ConfigurableGameObject {
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
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _X);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _Y);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _Z);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _RotX);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _RotY);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _RotZ);
    }

    public override void ApplyConfiguration (Configuration configuration) { 
      if (_debug)
        Debug.Log ("Applying " + configuration.ToString () + " To " + GetConfigurableIdentifier ());
      var e = transform.rotation.eulerAngles;
      var pos = transform.position;
      if (configuration.ConfigurableName == _X) {
        pos.Set (configuration.ConfigurableValue, transform.position.y, transform.position.z);
        transform.position = pos;
      } else if (configuration.ConfigurableName == _Y) {
        pos.Set (transform.position.x, configuration.ConfigurableValue, transform.position.z);
        transform.position = pos;
      } else if (configuration.ConfigurableName == _Z) {
        pos.Set (transform.position.x, transform.position.y, configuration.ConfigurableValue);
        transform.position = pos;
      } else if (configuration.ConfigurableName == _RotX) {
        e.Set (configuration.ConfigurableValue, e.y, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
      } else if (configuration.ConfigurableName == _RotY) {
        e.Set (e.x, configuration.ConfigurableValue, e.z);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
      } else if (configuration.ConfigurableName == _RotZ) {
        e.Set (e.x, e.y, configuration.ConfigurableValue);
        transform.rotation.SetLookRotation (Vector3.zero);
        transform.Rotate (e);
      }
    }

    public override string GetConfigurableIdentifier () {
      return name + "HexDOFConfigurable";
    }
  }
}