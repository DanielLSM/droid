using System;
using UnityEngine;

namespace UnityStandardAssets.Utility {
  [Serializable]
  public class CurveControlledBob {
    public AnimationCurve Bobcurve = new AnimationCurve(
                                                        new Keyframe(
                                                                     time : 0f,
                                                                     value : 0f),
                                                        new Keyframe(
                                                                     time : 0.5f,
                                                                     value : 1f),
                                                        new Keyframe(
                                                                     time : 1f,
                                                                     value : 0f),
                                                        new Keyframe(
                                                                     time : 1.5f,
                                                                     value : -1f),
                                                        new Keyframe(
                                                                     time : 2f,
                                                                     value : 0f));

    public float HorizontalBobRange = 0.33f;
    float m_BobBaseInterval;

    float m_CyclePositionX;
    float m_CyclePositionY;
    Vector3 m_OriginalCameraPosition;
    float m_Time;

    public float VerticalBobRange = 0.33f;

    // sin curve for head bob
    public float VerticaltoHorizontalRatio = 1f;

    public void Setup(Camera camera, float bobBaseInterval) {
      this.m_BobBaseInterval = bobBaseInterval;
      this.m_OriginalCameraPosition = camera.transform.localPosition;

      // get the length of the curve in time
      this.m_Time = this.Bobcurve[index : this.Bobcurve.length - 1].time;
    }

    public Vector3 DoHeadBob(float speed) {
      var xPos = this.m_OriginalCameraPosition.x
                 + this.Bobcurve.Evaluate(time : this.m_CyclePositionX) * this.HorizontalBobRange;
      var yPos = this.m_OriginalCameraPosition.y
                 + this.Bobcurve.Evaluate(time : this.m_CyclePositionY) * this.VerticalBobRange;

      this.m_CyclePositionX += speed * Time.deltaTime / this.m_BobBaseInterval;
      this.m_CyclePositionY +=
        speed * Time.deltaTime / this.m_BobBaseInterval * this.VerticaltoHorizontalRatio;

      if (this.m_CyclePositionX > this.m_Time) this.m_CyclePositionX = this.m_CyclePositionX - this.m_Time;
      if (this.m_CyclePositionY > this.m_Time) this.m_CyclePositionY = this.m_CyclePositionY - this.m_Time;

      return new Vector3(
                         x : xPos,
                         y : yPos,
                         z : 0f);
    }
  }
}
