using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurations {
  public class ConfigurableGameObject : MonoBehaviour, Configurable {

    public LearningEnvironment _environment;
    public bool _debug = false;

    protected virtual void Start () {
      AddToEnvironment ();
    }


    protected virtual void AddToEnvironment () {
      _environment = NeodroidUtilities.MaybeRegisterComponent (_environment, this);
    }

    public virtual void ApplyConfiguration (Configuration configuration) {
    }

    public virtual string GetConfigurableIdentifier () {
      return name + "Configurable";
    }
  }
}