using System;
using Neodroid.Managers.General;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers {
  public class PausableManager : NeodroidManager {
    #region Fields

    #endregion

    #region UnityCallbacks

    protected void Awake() {

      if (this.Configuration.SimulationType == SimulationType.FrameDependent) {
        this.EarlyUpdateEvent += this.PauseSimulation;
        this.UpdateEvent += this.MaybeResume;
      }
    }

    #endregion

    #region PrivateMethods

    void MaybeResume() {
      if (this.TestMotors || this.CurrentCurrentReaction.Parameters.Step)
        this.ResumeSimulation(this._configuration.TimeScale);
    }

    public Boolean IsSimulationPaused { get { return !(this.SimulationTime > 0); } }

    void PauseSimulation() { this.SimulationTime = 0; }

    void ResumeSimulation(float simulation_time_scale) {
      if (simulation_time_scale > 0)
        this.SimulationTime = simulation_time_scale;
      else
        this.SimulationTime = 1;
    }

    #endregion
  }
}
