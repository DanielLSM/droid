﻿using System;
using Neodroid.Models.Actors;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Motors.General {
  [ExecuteInEditMode]
  [Serializable]
  public class Motor : MonoBehaviour {
    public Actor ParentActor { get { return this._actor; } set { this._actor = value; } }

    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    public ValueSpace MotionValueSpace {
      get { return this._motion_value_space; }
      set { this._motion_value_space = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual String MotorIdentifier { get { return this.name + "Motor"; } }

    protected virtual void Awake() { this.RegisterComponent(); }

    public virtual void RegisterComponent() {
      this._actor = NeodroidUtilities.MaybeRegisterComponent(this._actor, this);
    }

    #if UNITY_EDITOR
    void OnValidate() {
      // Only called in the editor
      //RegisterComponent ();
    }
    #endif

    protected virtual void Start() { }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { this.Start(); }

    public void ApplyMotion(MotorMotion motion) {
      if (this.Debugging)
        print("Applying " + motion + " To " + this.name);
      if (motion.Strength < this.MotionValueSpace.MinValue
          || motion.Strength > this.MotionValueSpace.MaxValue) {
        print(
            string.Format(
                "It does not accept input {0}, outside allowed range {1} to {2}",
                motion.Strength,
                this.MotionValueSpace.MinValue,
                this.MotionValueSpace.MaxValue));
        return; // Do nothing
      }

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    protected virtual void InnerApplyMotion(MotorMotion motion) { }

    public virtual float GetEnergySpend() { return this._energy_spend_since_reset; }

    public override string ToString() { return this.MotorIdentifier; }

    public virtual void Reset() { this._energy_spend_since_reset = 0; }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    Actor _actor;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    ValueSpace _motion_value_space = new ValueSpace {DecimalGranularity = 0, MinValue = -10, MaxValue = 10};

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion
  }
}
