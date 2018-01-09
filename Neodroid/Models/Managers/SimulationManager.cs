using System;
using Neodroid.Models.Managers.General;
using Neodroid.Scripts.Utilities.Enums;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Managers {
  public class SimulationManager : NeodroidManager {
    #region Fields

    [Header ("Specific", order = 100)]
    [SerializeField]
    SimulatorConfiguration _configuration;

    // When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField] bool _update_fixed_time_scale;

    public SimulatorConfiguration Configuration {
      get {
        if (this._configuration == null) {
          this.Configuration = new SimulatorConfiguration ();
        }
        return this._configuration; 
      }
      set { this._configuration = value; }
    }

    public WaitOn WaitOnEvery {
      get { return this._configuration.WaitEvery; }
      set { this._configuration.WaitEvery = value; }
    }

    public void SetWaitOnEveryOnIndex (int wait_on) {
      this.WaitOnEvery = (WaitOn)wait_on;
    }

    #endregion

    #region UnityCallbacks

    void FixedUpdate () {
      if (this._configuration.WaitEvery == WaitOn.FixedUpdate)
        this.PauseSimulation ();
    }

    protected override void InnerUpdate () {
      if (this.Configuration.WaitEvery == WaitOn.Update)
        this.PauseSimulation ();
      if (this.TestMotors) {
        this.ResumeSimulation (this._configuration.SimulationTimeScale);
        this.ReactInEnvironments (this.SampleTestReaction ());
        return;
      }

      if (this._configuration.WaitEvery == WaitOn.Never || this.CurrentReaction.Parameters.Step)
        this.ResumeSimulation (this._configuration.SimulationTimeScale);
    }

    protected override void InnerAwake () {
      if (this.Configuration == null)
        this.Configuration = new SimulatorConfiguration ();
    }

    protected override void InnerStart () {


      QualitySettings.SetQualityLevel (this._configuration.QualityLevel, true);
      Application.targetFrameRate = this._configuration.TargetFrameRate;

      Time.timeScale = this._configuration.TimeScale;
      #if !UNITY_EDITOR
      Screen.SetResolution (
        width : this._configuration.Width,
        height : this._configuration.Height,
        fullscreen : this._configuration.FullScreen);
      #endif
    }

    #endregion

    #region PrivateMethods

    public bool IsSimulationPaused () {
      return Math.Abs (Time.timeScale) < Double.Epsilon;
    }

    void PauseSimulation () {
      Time.timeScale = 0;
      if (this._update_fixed_time_scale)
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    void ResumeSimulation (float simulation_time_scale) {
      if (simulation_time_scale > 0) {
        Time.timeScale = simulation_time_scale;
        if (this._update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      } else {
        Time.timeScale = 1;
        if (this._update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      }
    }

    #endregion
  }
}
