using System;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct ValueSpace {
    public int DecimalGranularity;
    public float MinValue;
    public float MaxValue;

    public ValueSpace (int decimal_granularity = 10) {
      this.DecimalGranularity = decimal_granularity;
      this.MinValue = -100f; //float.NegativeInfinity;
      this.MaxValue = 100f; //float.PositiveInfinity;
    }

    public float Span { get { return this.MaxValue - this.MinValue; } }

    public float ClipNormaliseRound (float v) {
      if (v > this.MaxValue)
        v = this.MaxValue;
      else if (v < this.MinValue)
        v = this.MinValue;
      return this.Round ((v - this.MinValue) / this.Span);
    }

    public float Round (float v) {
      return (float)Math.Round (v, this.DecimalGranularity);
    }
  }
}
