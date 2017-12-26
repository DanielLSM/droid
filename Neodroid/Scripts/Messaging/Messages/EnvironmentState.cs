
using System.Collections.Generic;
using Neodroid.Actors;
using Neodroid.Observers;
using Neodroid.Configurables;
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [System.Serializable]
  public class EnvironmentState {
    string _environment_name;

    public string EnvironmentName {
      get {
        return _environment_name;
      }
    }

    float _total_energy_spent_since_reset;

    public float TotalEnergySpentSinceReset {
      get {
        return _total_energy_spent_since_reset;
      }
    }

    int _frame_number;

    public int FrameNumber {
      get {
        return _frame_number;
      }
    }

    bool _interrupted;

    public bool Interrupted {
      get {
        return _interrupted;
      }
    }

    string _debug_message;

    public string DebugMessage {
      get {
        return _debug_message;
      }
    }

    Rigidbody[] _bodies;

    public Rigidbody[] Bodies {
      get {
        return _bodies;
      }
    }

    Transform[] _poses;

    public Transform[] Poses {
      get {
        return _poses;
      }
    }

    Dictionary<string, Observer> _observers;

    public Dictionary<string, Observer> Observers {
      get {
        return _observers;
      }
    }

    EnvironmentDescription _description;

    public EnvironmentDescription Description {
      get {
        return _description;
      }
    }

    float _reward;

    public float Reward {
      get {
        return _reward;
      }
    }

    public EnvironmentState (
      string environment_name,
      float total_energy_spent_since_reset,
      Dictionary<string, Observer> observers,
      int frame_number,
      float reward,
      bool interrupted,
      Rigidbody[] bodies,
      Transform[] poses,
      EnvironmentDescription description = null,
      string debug_message = "") {
      _debug_message = debug_message;
      _environment_name = environment_name;
      _total_energy_spent_since_reset = total_energy_spent_since_reset;
      _observers = observers;
      _reward = reward;
      _frame_number = frame_number;
      _interrupted = interrupted;
      _description = description;
      _bodies = bodies;
      _poses = poses;
    }
      
  }
}
