﻿using Neodroid.Messaging.Messages;
using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Actors;
using System;

namespace Neodroid.Motors {
  [ExecuteInEditMode]
  [Serializable]
  public class Motor : MonoBehaviour {
    public bool _debug = false;
    public int _decimal_granularity = 0;
    public float _min_strength = -100;
    public float _max_strength = 100;
    public float _energy_cost = 1;
    protected float _energy_spend_since_reset = 0;
    public Actor _actor_game_object;

    protected virtual  void Awake () {
      RegisterComponent ();
    }

    public virtual void RegisterComponent () {
      _actor_game_object = NeodroidUtilities.MaybeRegisterComponent (_actor_game_object, this);
    }

    #if UNITY_EDITOR
    void OnValidate () { // Only called in the editor
      //RegisterComponent ();
    }
    #endif

    protected virtual void Start () {

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

    public virtual void ApplyMotion (MotorMotion motion) {
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
