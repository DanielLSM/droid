using System;
using Neodroid.Models.Managers.General;
using UnityEngine;

namespace Neodroid.Models.Managers {
  public enum WaitOn {
    Never,
    // Dont wait from reactions from agent
    FixedUpdate,
    // Note: unstable physics with the FixedUpdate setting
    Update
    // Frame
  }

  public class SimulationManager : NeodroidManager {
    #region Fields

    [Header (
      header : "Specific",
      order = 100)]
    [SerializeField]
    WaitOn _wait_every = WaitOn.Update;

    [SerializeField] bool _update_fixed_time_scale;

    // When true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField] int _frame_skips;

    [SerializeField] float _simulation_time_scale = 1;

    [SerializeField] int _reset_iterations = 10;

    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects

    #endregion

    #region Getter Setters

    public int FrameSkips { get { return this._frame_skips; } set { this._frame_skips = value; } }

    public float SimulationTimeScale {
      get { return this._simulation_time_scale; }
      set { this._simulation_time_scale = value; }
    }

    public int ResetIterations {
      get { return this._reset_iterations; }
      set { this._reset_iterations = value; }
    }

    public WaitOn WaitEvery { get { return this._wait_every; } set { this._wait_every = value; } }

    public bool IsSimulationPaused () {
      return Math.Abs (Time.timeScale) < Double.Epsilon;
    }

    #endregion

    #region UnityCallbacks

    void FixedUpdate () {
      if (this.WaitEvery == WaitOn.FixedUpdate)
        this.PauseSimulation ();
    }

    protected override void InnerUpdate () {
      if (this.WaitEvery == WaitOn.Update)
        this.PauseSimulation ();
      if (this.TestMotors) {
        this.ResumeSimulation (simulation_time_scale : this._simulation_time_scale);
        this.ReactInEnvironments (reaction : this.SampleTestReaction ());
        return;
      }

      if (this.WaitEvery == WaitOn.Never || this.CurrentReaction.Parameters.Step)
        this.ResumeSimulation (simulation_time_scale : this._simulation_time_scale);
    }

    #endregion

    #region PrivateMethods

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
