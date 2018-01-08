using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "PlayerMotions",
      menuName = "Neodroid/ScriptableObjects/PlayerMotions",
      order = 1)]
  public class PlayerMotions : ScriptableObject {
    public PlayerMotion[] Motions;
  }

  [Serializable]
  public struct PlayerMotion {
    public KeyCode Key;
    public string Actor;
    public string Motor;
    public float Strength;
  }
}
