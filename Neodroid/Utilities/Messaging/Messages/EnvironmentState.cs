using System;
using System.Collections.Generic;
using Neodroid.Models.Observers.General;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public EnvironmentState(
        string environment_name,
        float total_energy_spent_since_reset,
        Dictionary<string, Observer> observations,
        int frame_number,
        float signal,
        bool terminated,
        float[] observables,
        Rigidbody[] bodies,
        Transform[] poses,
        string termination_reason = "",
        EnvironmentDescription description = null,
        string debug_message = "") {
      this.Observables = observables;
      this.DebugMessage = debug_message;
      this.TerminationReason = termination_reason;
      this.EnvironmentName = environment_name;
      this.TotalEnergySpentSinceReset = total_energy_spent_since_reset;
      this.Observations = observations;
      this.Signal = signal;
      this.FrameNumber = frame_number;
      this.Terminated = terminated;
      this.Description = description;
      this.Unobservables = new Unobservables(bodies, poses);
    }

    public float[] Observables { get; private set; }

    public String TerminationReason { get; private set; }

    public string EnvironmentName { get; private set; }

    public float TotalEnergySpentSinceReset { get; private set; }

    public int FrameNumber { get; private set; }

    public bool Terminated { get; private set; }

    public string DebugMessage { get; private set; }

    public Dictionary<string, Observer> Observations { get; private set; }

    public EnvironmentDescription Description { get; private set; }

    public float Signal { get; private set; }

    public Unobservables Unobservables { get; private set; }
  }
}
