using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurables {
  
  public class ConfigurableGameObject : Configurable {
    
    public bool _has_observer = false;
    public string _observer_name = "";
    public LearningEnvironment _environment;
    public bool _debug = false;
    public int _decimal_granularity = 0;
    public float _min_value = -100;
    public float _max_value = 100;
    public bool _relative_to_existing_value = false;

    protected virtual void Start () {
    }

    protected virtual void Awake () {
      _has_observer = false;
      _observer_name = "";
      AddToEnvironment ();
    }

    public void RefreshAwake () {
      Awake ();
    }

    public void RefreshStart () {
      Start ();
    }

    public void SetHasObserver (bool val, string str) {
      _has_observer = true;
      _observer_name = str;
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