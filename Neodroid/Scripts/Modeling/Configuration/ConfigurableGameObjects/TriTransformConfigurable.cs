using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;
using Neodroid.Observers;

namespace Neodroid.Configurations {

  public class TriTransformConfigurable : EulerTransformConfigurable {

    string _X;
    string _Y;
    string _Z;

    protected override void AddToEnvironment () {
      _X = GetConfigurableIdentifier () + "X";
      _Y = GetConfigurableIdentifier () + "Y";
      _Z = GetConfigurableIdentifier () + "Z";
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _X);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _Y);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _Z);
    }

    protected override void Start () {
      var observer = GetComponent<EulerTransformObserver> ();
      if (observer) {
        observer.SetHasConfigurable (true, GetConfigurableIdentifier ());
        SetHasObserver (true, observer.GetObserverIdentifier ());
      }
    }

    public override void ApplyConfiguration (Configuration configuration) {
      var pos = _environment.TransformPosition (this.transform.position);
      var v = configuration.ConfigurableValue;
      if (_decimal_granularity >= 0) {
        v = (float)Math.Round (v, _decimal_granularity);
      }
      if (_min_value != _max_value) {
        if (v < _min_value || v > _max_value) {
          Debug.Log (String.Format ("Configurable does not accept input{2}, outside allowed range {0} to {1}", _min_value, _max_value, v));
          return; // Do nothing
        }
      }
      if (_debug)
        Debug.Log (String.Format ("Applying {0} to {1} configurable", v.ToString (), configuration.ConfigurableName));
      if (_relative_to_existing_value) {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v - pos.x, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v - pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v - pos.z);
        }
      } else {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v);
        }
      }
      var inv_pos = _environment.InverseTransformPosition (pos);
      this.transform.position = inv_pos;
      
    }

    public override string GetConfigurableIdentifier () {
      return name + "Transform";
    }
  }
}