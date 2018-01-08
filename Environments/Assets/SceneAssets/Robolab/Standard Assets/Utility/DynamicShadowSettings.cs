using UnityEngine;

namespace UnityStandardAssets.Utility {
  public class DynamicShadowSettings : MonoBehaviour {
    public float adaptTime = 1;
    float m_ChangeSpeed;
    float m_OriginalStrength = 1;

    float m_SmoothHeight;
    public float maxHeight = 1000;
    public float maxShadowBias = 0.1f;
    public float maxShadowDistance = 10000;
    public float minHeight = 10;
    public float minShadowBias = 1;
    public float minShadowDistance = 80;
    public Light sunLight;

    void Start() { this.m_OriginalStrength = this.sunLight.shadowStrength; }

    // Update is called once per frame
    void Update() {
      var ray = new Ray(
                        origin : Camera.main.transform.position,
                        direction : -Vector3.up);
      RaycastHit hit;
      var height = this.transform.position.y;
      if (Physics.Raycast(
                          ray : ray,
                          hitInfo : out hit)) height = hit.distance;

      if (Mathf.Abs(f : height - this.m_SmoothHeight) > 1)
        this.m_SmoothHeight = Mathf.SmoothDamp(
                                               current : this.m_SmoothHeight,
                                               target : height,
                                               currentVelocity : ref this.m_ChangeSpeed,
                                               smoothTime : this.adaptTime);

      var i = Mathf.InverseLerp(
                                a : this.minHeight,
                                b : this.maxHeight,
                                value : this.m_SmoothHeight);

      QualitySettings.shadowDistance = Mathf.Lerp(
                                                  a : this.minShadowDistance,
                                                  b : this.maxShadowDistance,
                                                  t : i);
      this.sunLight.shadowBias = Mathf.Lerp(
                                            a : this.minShadowBias,
                                            b : this.maxShadowBias,
                                            t : 1 - (1 - i) * (1 - i));
      this.sunLight.shadowStrength = Mathf.Lerp(
                                                a : this.m_OriginalStrength,
                                                b : 0,
                                                t : i);
    }
  }
}
