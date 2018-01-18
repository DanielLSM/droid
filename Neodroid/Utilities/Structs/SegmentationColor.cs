using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Structs {
  [Serializable]
  public struct ColorByTag {
    public string Tag;
    public Color Col;
  }

  [Serializable]
  public struct ColorByInstance {
    public GameObject Obj;
    public Color Col;
  }
}
