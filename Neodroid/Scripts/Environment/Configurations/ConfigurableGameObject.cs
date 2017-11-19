using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Agents;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurations {
  public class ConfigurableGameObject : MonoBehaviour, Configurable {

    public EnvironmentManager _environment_manager;
    public bool _debug = false;

    protected virtual void Start () {
      Setup ();
      AddToEnvironment ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    protected void AddToEnvironment () {
      _environment_manager = NeodroidUtilities.MaybeRegisterComponent (_environment_manager, this);
    }

    public virtual void ApplyConfiguration (Configuration configuration) {
    }

    public virtual string GetConfigurableIdentifier () {
      return name + "BaseConfigurable";
    }
  }
}