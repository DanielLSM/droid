﻿using Neodroid.Models.Environments;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Configurables.General {
  public class ConfigurableGameObject : Configurable {
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    public InputRange ValidInput { get { return this._valid_input; } set { this._valid_input = value; } }

    public LearningEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public override string ConfigurableIdentifier { get { return this.name + "Configurable"; } }

    public virtual void UpdateObservation() { }

    protected virtual void Start() { this.UpdateObservation(); }

    protected virtual void Awake() { this.AddToEnvironment(); }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { this.Start(); }

    protected virtual void AddToEnvironment() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    public override void ApplyConfiguration(Configuration configuration) { }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    InputRange _valid_input = new InputRange {DecimalGranularity = 0, MinValue = 10, MaxValue = 10};

    [SerializeField] bool _relative_to_existing_value;

    #endregion
  }
}
