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
    public int _decimal_granularity = 0;
    public float _min_value = -100;
    public float _max_value = 100;
    public bool _relative_to_default_value = false;

    protected virtual void Start () {
    }

    protected virtual void Awake () {
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