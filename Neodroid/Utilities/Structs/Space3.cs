using System;
using UnityEngine;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct Space3 {
    public int DecimalGranularity;
    public Vector3 MinValues;
    public Vector3 MaxValues;

    public Space3(int decimal_granularity = Int32.MaxValue) {
      this.DecimalGranularity = decimal_granularity;
      this.MinValues = Vector3.one * -100f;
      this.MaxValues = Vector3.one * 100f; //Vector3.positiveInfinity;
    }

    public Vector3 Span { get { return this.MaxValues - this.MinValues; } }

    public Vector3 ClipNormalise(Vector3 v) {
      if (v.x > this.MaxValues.x)
        v.x = this.MaxValues.x;
      else if (v.x < this.MinValues.x)
        v.x = this.MinValues.x;
      if(this.Span.x > 0)
        v.x = (v.x - this.MinValues.x) / this.Span.x;
      else {
        v.x = 0;
      }

      if (v.y > this.MaxValues.y)
        v.y = this.MaxValues.y;
      else if (v.y < this.MinValues.y)
        v.y = this.MinValues.y;
      if (this.Span.y > 0) {
        v.y = (v.y - this.MinValues.y) / this.Span.y;
      } else {
          v.y = 0;
        }

      if (v.z > this.MaxValues.z)
        v.z = this.MaxValues.z;
      else if (v.z < this.MinValues.z)
        v.z = this.MinValues.z;
      if (this.Span.z > 0) {
        v.z = (v.z - this.MinValues.z) / this.Span.z;
      } else {
        v.z = 0;
      }

      return v;
    }

    public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }
  }
}
