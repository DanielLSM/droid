using UnityEngine;


namespace Neodroid.Utilities {
  [System.Serializable]
  public struct SimulationConfiguration {
    public int width;
    public int height;
    [Range (0, 4)]
    public int quality_level;
    [Range (1f, 100f)]
    public float time_scale;
    public int target_frame_rate;
  }
}

