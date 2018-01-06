using Neodroid.Messaging.Messages;
using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Actors;
using System;

namespace Neodroid.Motors {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Motor : MonoBehaviour {
    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    Actor _actor;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging = false;

    [Header ("General", order = 101)]
    [SerializeField]
    InputRange _valid_input = new InputRange { decimal_granularity = 0, min_value = -10, max_value = 10 };
    [SerializeField]
    float _energy_spend_since_reset = 0;
    [SerializeField]
    float _energy_cost = 0;


    #endregion

    public Actor ParentActor {
      get {
        return _actor;
      }
      set {
        _actor = value;
      }
    }

    public float EnergySpendSinceReset {
      get {
        return _energy_spend_since_reset;
      }
      set {
        _energy_spend_since_reset = value;
      }
    }

    public float EnergyCost {
      get {
        return _energy_cost;
      }
      set {
        _energy_cost = value;
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

    protected virtual  void Awake () {
      RegisterComponent ();
    }

    public virtual void RegisterComponent () {
      _actor = NeodroidUtilities.MaybeRegisterComponent (_actor, this);
    }

    #if UNITY_EDITOR
    void OnValidate () { // Only called in the editor
      //RegisterComponent ();
    }
    #endif

    protected virtual void Start () {

    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set {
        _debugging = value;
      }
    }

    public void RefreshAwake () {
      Awake ();
    }


    public void RefreshStart () {
      Start ();
    }

    public Motor () {
    }

    public virtual string GetMotorIdentifier () {
      return name + "Motor";
    }

    public void ApplyMotion (MotorMotion motion) {
      if (Debugging)
        print ("Applying " + motion.ToString () + " To " + name);
      if (motion.Strength < ValidInput.min_value || motion.Strength > ValidInput.max_value) {
        print (System.String.Format ("It does not accept input {0}, outside allowed range {1} to {2}", motion.Strength, ValidInput.min_value, ValidInput.max_value));
        return; // Do nothing
      }
      InnerApplyMotion (motion);
      EnergySpendSinceReset += Mathf.Abs (EnergyCost * motion.Strength);
    }

    public virtual void InnerApplyMotion (MotorMotion motion) {
    }

    public virtual float GetEnergySpend () {
      return _energy_spend_since_reset;
    }

    public override string ToString () {
      return GetMotorIdentifier ();
    }

    public virtual void Reset () {
      _energy_spend_since_reset = 0;
    }
  }
}
