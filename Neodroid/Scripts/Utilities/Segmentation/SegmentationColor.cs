using UnityEngine;

namespace Neodroid.Segmentation {
  [System.Serializable]
  public struct SegmentationColorByTag {
    public string tag;
    public Color color;
  }

  [System.Serializable]
  public struct SegmentationColorByInstance {
    public GameObject game_object;
    public Color color;
  }
}
