using UnityEngine;

namespace Neodroid.Scripts.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "NeodroidTask",
      menuName = "Neodroid/ScriptableObjects/NeodroidTask",
      order = 1)]
  public class NeodroidTask : ScriptableObject {
    public Vector3 Position;
    public float Radius;
  }
}
