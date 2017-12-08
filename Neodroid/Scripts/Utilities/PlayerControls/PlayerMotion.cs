using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neodroid.Utilities {
  [System.Serializable]
  public struct PlayerMotion {
    public KeyCode key;
    public string actor;
    public string motor;
    public float strength;
  }

}