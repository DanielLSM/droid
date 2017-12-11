using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurations {
  
  public class ConfigurableGameObject : Configurable {

    public LearningEnvironment _environment;
    public bool _debug = false;

    protected virtual void Start () {
    }

    void Awake () {
      AddToEnvironment ();
    }

    public void Refresh () {
      Awake ();
    }


    protected virtual void AddToEnvironment () {
      _environment = NeodroidUtilities.MaybeRegisterComponent (_environment, this);
    }

    public override void ApplyConfiguration (Configuration configuration) {
    }

    public override string GetConfigurableIdentifier () {
      return name + "Configurable";
    }
  }
}