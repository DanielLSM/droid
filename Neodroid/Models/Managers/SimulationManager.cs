using UnityEngine;

namespace Neodroid.Managers {
  public enum WaitOn {
    Never,
    FixedUpdate,

    Update
    // Frame
  }

  public class SimulationManager : NeodroidManager {
    #region Fields

    [Header (
      "Specific",
      order = 100)]
    [SerializeField]
    private WaitOn _wait_every = WaitOn.FixedUpdate;

    [SerializeField]
    private bool _update_fixed_time_scale;

    // When true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField]
    private int _frame_skips;

    [SerializeField]
    private float _simulation_time_scale = 1;

    [SerializeField]
    private int _reset_iterations = 10;

    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects

    #endregion

    #region Getter Setters

    public int FrameSkips { get { return _frame_skips; } set { _frame_skips = value; } }

    public float SimulationTimeScale {
      get { return _simulation_time_scale; }
      set { _simulation_time_scale = value; }
    }

    public int ResetIterations { get { return _reset_iterations; } set { _reset_iterations = value; } }

    public WaitOn WaitEvery { get { return _wait_every; } set { _wait_every = value; } }

    public bool IsSimulationPaused () {
      return Time.timeScale == 0;
    }

    #endregion

    #region UnityCallbacks

    private void FixedUpdate () {
      if (WaitEvery == WaitOn.FixedUpdate)
        PauseSimulation ();
    }

    protected override void InnerUpdate () {
      if (WaitEvery == WaitOn.Update)
        PauseSimulation ();
      if (TestMotors) {
        ResumeSimulation (_simulation_time_scale);
        ReactInEnvironments (SampleTestReaction ());
        return;
      }
      if (WaitEvery == WaitOn.Never || CurrentReaction.Parameters.Step)
        ResumeSimulation (_simulation_time_scale);
    }

    #endregion

    #region PrivateMethods

    private void PauseSimulation () {
      Time.timeScale = 0;
      if (_update_fixed_time_scale)
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    private void ResumeSimulation (float simulation_time_scale) {
      if (simulation_time_scale > 0) {
        Time.timeScale = simulation_time_scale;
        if (_update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      } else {
        Time.timeScale = 1;
        if (_update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      }
    }

    #endregion
  }
}
