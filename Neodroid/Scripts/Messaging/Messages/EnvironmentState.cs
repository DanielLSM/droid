using System;
using System.Collections.Generic;
using Neodroid.Actors;
using Neodroid.Observers;
using Neodroid.Configurables;
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public string _environment_name;
    public float _total_energy_spent_since_reset;
    public int _last_steps_frame_number;
    public bool _interrupted;
    public string _debug_message;

    public Rigidbody[] _bodies;
    public Transform[] _poses;

    public Dictionary<string, Observer> _observers;

    public EnvironmentDescription _description;

    public float _reward_for_last_step;

    public EnvironmentState (
      string environment_name,
      float total_energy_spent_since_reset,
      Dictionary<string, Observer> observers,
      int last_steps_frame_number,
      float reward_for_last_step,
      bool interrupted,
      Rigidbody[] bodies,
      Transform[] poses,
      EnvironmentDescription description = null,
      string debug_message = "") {
      _debug_message = debug_message;
      _environment_name = environment_name;
      _total_energy_spent_since_reset = total_energy_spent_since_reset;
      _observers = observers;
      _reward_for_last_step = reward_for_last_step;
      _last_steps_frame_number = last_steps_frame_number;
      _interrupted = interrupted;
      _description = description;
      _bodies = bodies;
      _poses = poses;
    }

    public EnvironmentState () {

    }
  }
}
