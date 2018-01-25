using System;
using System.Threading;
using Neodroid.Managers.General;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers {
  public class PausableManager : NeodroidManager {
    #region Fields

    [SerializeField] bool _blocked;


    #endregion

    #region UnityCallbacks

    protected void Awake () {

      if (this.Configuration.SimulationType == SimulationType.FrameDependent) {
        this.EarlyUpdateEvent += this.PauseSimulation;
        this.UpdateEvent += this.MaybeResume;
      }
      if (this.Configuration.SimulationType == SimulationType.PhysicsDependent) {
        this.EarlyFixedUpdateEvent += this.Receive;
      }

    }

    #endregion

    #region PrivateMethods

    void MaybeResume () {
      if (this.TestMotors || this.CurrentReaction.Parameters.Step)
        this.ResumeSimulation (this._configuration.TimeScale);
    }

    public Boolean IsSimulationPaused { get { return !(this.SimulationTime > 0); } }

    void PauseSimulation () {
      this.SimulationTime = 0;
    }

    void ResumeSimulation (float simulation_time_scale) {
      if (simulation_time_scale > 0)
        this.SimulationTime = simulation_time_scale;
      else
        this.SimulationTime = 1;
    }

    void Receive () {
      var reaction = this._message_server.Receive (TimeSpan.Zero);
      this.SetReactionFromExternalSource(reaction);
    }

    #endregion
  }
}
