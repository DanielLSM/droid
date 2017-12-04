using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {

  public class TriDOFConfigurable : ConfigurableGameObject {
    string _X;
    string _Y;
    string _Z;

    protected override void AddToEnvironment () {
      _X = GetConfigurableIdentifier () + "X";
      _Y = GetConfigurableIdentifier () + "Y";
      _Z = GetConfigurableIdentifier () + "Z";
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _X);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _Y);
      _environment_manager = NeodroidUtilities.MaybeRegisterNamedComponent (_environment_manager, (ConfigurableGameObject)this, _Z);
    }

    public override void ApplyConfiguration (Configuration configuration) { 
      if (_debug)
        Debug.Log ("Applying " + configuration.ToString () + " To " + GetConfigurableIdentifier ());
      var pos = _environment_manager.TransformPosition (this.transform.position);
      if (configuration.ConfigurableName == _X) {
        pos.Set (configuration.ConfigurableValue, pos.y, pos.z);
      } else if (configuration.ConfigurableName == _Y) {
        pos.Set (pos.x, configuration.ConfigurableValue, pos.z);
      } else if (configuration.ConfigurableName == _Z) {
        pos.Set (pos.x, pos.y, configuration.ConfigurableValue);
      }
      var inv_pos = _environment_manager.InverseTransformPosition (pos);
      transform.position = inv_pos;
    }

    public override string GetConfigurableIdentifier () {
      return name + "TriDOFConfigurable";
    }
  }
}