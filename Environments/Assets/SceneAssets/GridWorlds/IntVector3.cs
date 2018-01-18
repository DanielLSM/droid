using System;
using UnityEngine;

namespace SceneAssets.GridWorlds {
  [Serializable]
  public struct IntVector3 {
    [SerializeField] public int x;
    [SerializeField] public int y;
    [SerializeField] public int z;

    public IntVector3(Vector3 vec3) {
      this.x = Mathf.RoundToInt(vec3.x);
      this.y = Mathf.RoundToInt(vec3.y);
      this.z = Mathf.RoundToInt(vec3.z);
    }
      
    public static IntVector3 operator + (IntVector3 a, IntVector3 b) {
      a.x += b.x;
      a.y += b.y;
      a.z += b.z;
      return a;
    }
      
    public IntVector3 (int x, int y, int z) {
      this.x = x;
      this.y = y;
      this.z = z;
    }
  }
}
