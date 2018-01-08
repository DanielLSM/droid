using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Segmentation {
  [Serializable]
  public struct SegmentationColorByTag {
    public string Tag;
    public Color Col;
  }

  [Serializable]
  public struct SegmentationColorByInstance {
    public GameObject Obj;
    public Color Col;
  }
}
