using UnityEngine;

namespace Neodroid.Utilities {
  [CreateAssetMenu (
    fileName = "PlayerMotions",
    menuName = "Neodroid/ScriptableObjects/PlayerMotions",
    order = 1)]
  public class PlayerMotions : ScriptableObject {
    public PlayerMotion[] _player_motions;
  }

  [System.Serializable]
  public struct PlayerMotion {
    public KeyCode key;
    public string actor;
    public string motor;
    public float strength;
  }
}
