using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.ScriptableObjects {
  [CreateAssetMenu(fileName = "Curriculum", menuName = "Neodroid/ScriptableObjects/Curriculum", order = 1)]
  public class Curriculum : ScriptableObject {
    public Level[] Levels;
  }

  [Serializable]
  public struct Level {
    public ConfigurableEntry[] configurable_entries;
    public float MinReward;
    public float MaxReward;
  }

  [Serializable]
  public struct ConfigurableEntry {
    public string configurable_name;
    public float MinValue;
    public float MaxValue;
  }
}
