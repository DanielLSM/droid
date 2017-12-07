using UnityEngine;
using System;

namespace Neodroid.Segmentation {

  [Serializable]
  public struct SegmentationColorByTag {
    public string tag;
    public Color color;
  }

  [Serializable]
  public struct SegmentationColorByInstance {
    public GameObject game_object;
    public Color color;
  }

}