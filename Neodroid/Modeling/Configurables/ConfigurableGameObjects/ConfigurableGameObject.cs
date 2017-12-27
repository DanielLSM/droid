using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurables {
  
  public class ConfigurableGameObject : Configurable {
    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging = false;

    [Header ("General", order = 101)]
    [SerializeField]
    InputRange _valid_input = new InputRange { decimal_granularity = 0, min_value = -10, max_value = 10 };
    [SerializeField]
    bool _relative_to_existing_value = false;


    #endregion

    public bool RelativeToExistingValue {
      get {
        return _relative_to_existing_value;
      }
    }

    public InputRange ValidInput {
      get {
        return _valid_input;
      }
      set {
        _valid_input = value;
      }
    }

    public LearningEnvironment ParentEnvironment {
      get {
        return _environment;
      }
      set {
        _environment = value;
      }
    }

    protected virtual void Start () {
    }

    protected virtual void Awake () {
      AddToEnvironment ();
    }

    public void RefreshAwake () {
      Awake ();
    }

    public void RefreshStart () {
      Start ();
    }


    protected virtual void AddToEnvironment () {
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent (ParentEnvironment, this);
    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set {
        _debugging = value;
      }
    }


    public override void ApplyConfiguration (Configuration configuration) {
    }

    public override string ConfigurableIdentifier{ get { return name + "Configurable"; } }
  }
}