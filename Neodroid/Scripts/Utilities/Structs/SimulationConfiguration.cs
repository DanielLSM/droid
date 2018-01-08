using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Structs {
  [Serializable]
  public struct SimulationConfiguration {
    public int Width;
    public int Height;

    [Range(
      min : 0,
      max : 4)]
    public int QualityLevel;

    [Range(
      min : 1f,
      max : 100f)]
    public float TimeScale;

    public int TargetFrameRate;
  }
}
