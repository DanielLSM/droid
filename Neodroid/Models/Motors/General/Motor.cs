﻿using Neodroid.Messaging.Messages;
using Neodroid.Models.Actors;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Motors.General {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Motor : MonoBehaviour {
    public Actor ParentActor { get { return this._actor; } set { this._actor = value; } }

    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    public InputRange ValidInput { get { return this._valid_input; } set { this._valid_input = value; } }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    protected virtual void Awake () {
      this.RegisterComponent ();
    }

    public virtual void RegisterComponent () {
      this._actor = NeodroidUtilities.MaybeRegisterComponent (
        r : this._actor,
        c : this);
    }

    #if UNITY_EDITOR
    void OnValidate() {
      // Only called in the editor
      //RegisterComponent ();
    }
    #endif

    protected virtual void Start () {
    }

    public void RefreshAwake () {
      this.Awake ();
    }

    public void RefreshStart () {
      this.Start ();
    }

    public virtual string GetMotorIdentifier () {
      return this.name + "Motor";
    }

    public void ApplyMotion (MotorMotion motion) {
      if (this.Debugging)
        print (message : "Applying " + motion + " To " + this.name);
      if (motion.Strength < this.ValidInput.MinValue || motion.Strength > this.ValidInput.MaxValue) {
        print (
          message : string.Format (
            format :
                                      "It does not accept input {0}, outside allowed range {1} to {2}",
            arg0 : motion.Strength,
            arg1 : this.ValidInput.MinValue,
            arg2 : this.ValidInput.MaxValue));
        return; // Do nothing
      }

      this.InnerApplyMotion (motion : motion);
      this.EnergySpendSinceReset += Mathf.Abs (f : this.EnergyCost * motion.Strength);
    }

    public virtual void InnerApplyMotion (MotorMotion motion) {
    }

    public virtual float GetEnergySpend () {
      return this._energy_spend_since_reset;
    }

    public override string ToString () {
      return this.GetMotorIdentifier ();
    }

    public virtual void Reset () {
      this._energy_spend_since_reset = 0;
    }

    #region Fields

    [Header (
      header : "References",
      order = 99)]
    [SerializeField]
    Actor _actor;

    [Header (
      header : "Development",
      order = 100)]
    [SerializeField]
    bool _debugging;

    [Header (
      header : "General",
      order = 101)]
    [SerializeField]
    InputRange _valid_input =
      new InputRange {
        DecimalGranularity = 0,
        MinValue = -10,
        MaxValue = 10
      };

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion
  }
}
