using Neodroid.Messaging.Messages;
using UnityEngine;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Actors;
using System;

namespace Neodroid.NeodroidEnvironment.Motors {
  [Serializable]
  public class Motor : MonoBehaviour {
    public bool _debug = false;
    public bool _bidirectional = true;
    public float _energy_cost = 1;
    protected float _energy_spend_since_reset = 0;
    public Actor _actor_game_object;
    public string _motor_identifier = "";

    private void Start () {
      RegisterComponent ();
    }

    public void RegisterComponent () {
      if (_motor_identifier == null || _motor_identifier == "")
        _motor_identifier = GetMotorIdentifier ();
      NeodroidUtilities.MaybeRegisterComponent (_actor_game_object, this);
    }

    private void Update () {
    }

    public Motor () {
    }

    public virtual string GetMotorIdentifier () {
      return _motor_identifier;
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
