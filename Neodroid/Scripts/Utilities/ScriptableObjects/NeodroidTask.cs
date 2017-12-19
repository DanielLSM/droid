using UnityEngine;

namespace Neodroid.Utilities {
  [CreateAssetMenu (fileName = "NeodroidTask", menuName = "Neodroid/ScriptableObjects/NeodroidTask", order = 1)]
  public class NeodroidTask : ScriptableObject {
    public Vector3 position;
    public float radius;
  }

}