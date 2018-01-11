using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Structs {
  [System.Serializable]
  public class SimulatorConfiguration {
    //: ScriptableObject {
    [SerializeField] int _frame_skips;

    [SerializeField] bool _full_screen;

    [SerializeField] int _height;

    [SerializeField] [Range (1, 4)] int _quality_level;

    [SerializeField] int _reset_iterations;
    [SerializeField] float _simulation_time_scale;
    [SerializeField] int _target_frame_rate;

    [SerializeField] [Range (1f, 100f)] float _time_scale;

    [SerializeField] WaitOn _wait_every;

    [SerializeField] int _width;

    public SimulatorConfiguration () {
      this._width = 500;
      this._height = 500;
      this._full_screen = false;
      this._quality_level = 1;
      this._time_scale = 1;
      this._target_frame_rate = -1;
      this.WaitEvery = WaitOn.Update;
      this.FrameSkips = 0;
      this.ResetIterations = 10;
    }

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
    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects

    public WaitOn WaitEvery { get { return this._wait_every; } set { this._wait_every = value; } }

    public int Width { get { return this._width; } set { this._width = value; } }

    public int Height { get { return this._height; } set { this._height = value; } }

    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    public int TargetFrameRate {
      get { return this._target_frame_rate; }
      set { this._target_frame_rate = value; }
    }

    public int QualityLevel { get { return this._quality_level; } set { this._quality_level = value; } }

    public float TimeScale { get { return this._time_scale; } set { this._time_scale = value; } }

    #endregion
  }
}
