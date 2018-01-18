namespace Neodroid.Utilities.Structs {
  /*public struct Space4 {
    [System.Serializable]
    public struct QuadSpace {
      public int DecimalGranularity;
      public Vector3 MinValues;
      public Vector3 MaxValues;

      public QuadSpace(int decimal_granularity=Int32.MaxValue) {
        this.DecimalGranularity = decimal_granularity;
        this.MinValues = Vector3.negativeInfinity;
        this.MaxValues = Vector3.positiveInfinity;
      }

      public Vector3 Span { get { return this.MaxValues - this.MinValues; } }

      public Vector3 ClipNormalise(Vector3 v) {
        if (v.x > this.MaxValues.x) {
          v.x = this.MaxValues.x;
        } else if (v.x < this.MinValues.x) {
          v = this.MinValues;
        }
        v.x = (v.x - this.MinValues.x) / this.Span.x;

        if (v.y > this.MaxValues.y) {
          v.y = this.MaxValues.y;
        } else if (v.y < this.MinValues.y) {
          v = this.MinValues;
        }
        v.y = (v.y - this.MinValues.y) / this.Span.y;

        if (v.z > this.MaxValues.z) {
          v.z = this.MaxValues.z;
        } else if (v.z < this.MinValues.z) {
          v = this.MinValues;
        }
        v.z = (v.z - this.MinValues.z) / this.Span.z;
        return v;
      }

      public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }
    }
  }*/
}
