using System;
using UnityEngine;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct Space2 {
    public int DecimalGranularity;
    public Vector2 MinValues;
    public Vector2 MaxValues;

    public Space2 (Int32 decimal_granularity = 10) : this () {
      this.MinValues = Vector2.one * -100f; //Vector2.negativeInfinity;
      this.MaxValues = Vector2.one * 100f; //Vector2.positiveInfinity;
      this.DecimalGranularity = decimal_granularity;
    }

    public Vector2 Span {
      get {
        return this.MaxValues - this.MinValues;
      }
    }

    public Vector2 ClipNormalise (Vector2 v) {
      if (v.x > this.MaxValues.x)
        v.x = this.MaxValues.x;
      else if (v.x < this.MinValues.x)
        v.x = this.MinValues.x;
      v.x = (v.x - this.MinValues.x) / this.Span.x;

      if (v.y > this.MaxValues.y)
        v.y = this.MaxValues.y;
      else if (v.y < this.MinValues.y)
        v.y = this.MinValues.y;
      v.y = (v.y - this.MinValues.y) / this.Span.y;

      return v;
    }

    public float Round (float v) {
      return (float)Math.Round (v, this.DecimalGranularity);
    }
  }
}
