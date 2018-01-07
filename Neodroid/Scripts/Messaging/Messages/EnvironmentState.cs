
using System.Collections.Generic;
using Neodroid.Observers;
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [System.Serializable]
  public class EnvironmentState {
    public EnvironmentState(
      string environment_name,
      float total_energy_spent_since_reset,
      Dictionary<string, Observer> observers,
      int frame_number,
      float reward,
      bool terminated,
      Rigidbody[] bodies,
      Transform[] poses,
      EnvironmentDescription description = null,
      string debug_message = "") {
      DebugMessage = debug_message;
      EnvironmentName = environment_name;
      TotalEnergySpentSinceReset = total_energy_spent_since_reset;
      Observers = observers;
      Reward = reward;
      FrameNumber = frame_number;
      Terminated = terminated;
      Description = description;
      Unobservables = new Unobservables(
                                        bodies,
                                        poses);
    }

    public string EnvironmentName { get; private set; }

    public float TotalEnergySpentSinceReset { get; private set; }

    public int FrameNumber { get; private set; }

    public bool Terminated { get; private set; }

    public string DebugMessage { get; private set; }

    public Dictionary<string, Observer> Observers { get; private set; }

    public EnvironmentDescription Description { get; private set; }

    public float Reward { get; private set; }

    public Unobservables Unobservables { get; private set; }
  }
}
