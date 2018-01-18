using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "Segmentations",
      menuName = "Neodroid/ScriptableObjects/Segmentations",
      order = 1)]
  public class Segmentations : ScriptableObject {
    public ColorByTag[] ColorByTags;
  }
}
