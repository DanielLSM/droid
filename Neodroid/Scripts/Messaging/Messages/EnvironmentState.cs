using System;
using System.Collections.Generic;
using Neodroid.Actors;
using Neodroid.Observers;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public float _time_since_reset;
    public float _total_energy_spent_since_reset;
    public int _last_steps_frame_number;
    public bool _interrupted;

    public Dictionary<string, Actor> _actors;

    public Dictionary<string, Observer> _observers;

    public float _reward_for_last_step;

    public EnvironmentState (
      float time_since_reset,
      float total_energy_spent_since_reset,
      Dictionary<string, Actor> actors,
      Dictionary<string, Observer> observers, 
      int last_steps_frame_number,
      float reward_for_last_step, 
      bool interrupted) {
      _time_since_reset = time_since_reset;
      _total_energy_spent_since_reset = total_energy_spent_since_reset;
      _actors = actors;
      _observers = observers;
      _reward_for_last_step = reward_for_last_step;
      _last_steps_frame_number = last_steps_frame_number;
      _interrupted = interrupted;
    }

    public EnvironmentState () {

    }
  }
}
