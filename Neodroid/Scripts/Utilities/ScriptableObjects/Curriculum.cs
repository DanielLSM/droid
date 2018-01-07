using UnityEngine;

namespace Neodroid.Utilities {
  [CreateAssetMenu (
    fileName = "Curriculum",
    menuName = "Neodroid/ScriptableObjects/Curriculum",
    order = 1)]
  public class Curriculum : ScriptableObject {
    public Level[] _levels;
  }

  [System.Serializable]
  public struct Level {
    public ConfigurableEntry[] configurable_entries;
    public float min_reward;
    public float max_reward;
  }

  [System.Serializable]
  public struct ConfigurableEntry {
    public string configurable_name;
    public float min_value;
    public float max_value;
  }
}
