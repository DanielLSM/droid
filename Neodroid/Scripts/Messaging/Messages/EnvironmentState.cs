using System;
using System.Collections.Generic;
using Neodroid.Models.Observers.General;
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [Serializable]
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
      this.DebugMessage = debug_message;
      this.EnvironmentName = environment_name;
      this.TotalEnergySpentSinceReset = total_energy_spent_since_reset;
      this.Observers = observers;
      this.Reward = reward;
      this.FrameNumber = frame_number;
      this.Terminated = terminated;
      this.Description = description;
      this.Unobservables = new Unobservables(
                                             rigidbodies : bodies,
                                             transforms : poses);
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
