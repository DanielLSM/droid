using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility {
  public class SimpleMouseRotator : MonoBehaviour {
    public bool autoZeroHorizontalOnMobile;
    public bool autoZeroVerticalOnMobile = true;
    public float dampingTime = 0.2f;
    Vector3 m_FollowAngles;
    Vector3 m_FollowVelocity;
    Quaternion m_OriginalRotation;

    Vector3 m_TargetAngles;

    public bool relative = true;

    // A mouselook behaviour with constraints which operate relative to
    // this gameobject's initial rotation.
    // Only rotates around local X and Y.
    // Works in local coordinates, so if this object is parented
    // to another moving gameobject, its local constraints will
    // operate correctly
    // (Think: looking out the side window of a car, or a gun turret
    // on a moving spaceship with a limited angular range)
    // to have no constraints on an axis, set the rotationRange to 360 or greater.
    public Vector2 rotationRange = new Vector3(
                                               x : 70,
                                               y : 70);

    public float rotationSpeed = 10;

    void Start() { this.m_OriginalRotation = this.transform.localRotation; }

    void Update() {
      // we make initial calculations from the original local rotation
      this.transform.localRotation = this.m_OriginalRotation;

      // read input from mouse or mobile controls
      float inputH;
      float inputV;
      if (this.relative) {
        inputH = CrossPlatformInputManager.GetAxis(name : "Mouse X");
        inputV = CrossPlatformInputManager.GetAxis(name : "Mouse Y");

        // wrap values to avoid springing quickly the wrong way from positive to negative
        if (this.m_TargetAngles.y > 180) {
          this.m_TargetAngles.y -= 360;
          this.m_FollowAngles.y -= 360;
        }

        if (this.m_TargetAngles.x > 180) {
          this.m_TargetAngles.x -= 360;
          this.m_FollowAngles.x -= 360;
        }

        if (this.m_TargetAngles.y < -180) {
          this.m_TargetAngles.y += 360;
          this.m_FollowAngles.y += 360;
        }

        if (this.m_TargetAngles.x < -180) {
          this.m_TargetAngles.x += 360;
          this.m_FollowAngles.x += 360;
        }

        #if MOBILE_INPUT
// on mobile, sometimes we want input mapped directly to tilt value,
// so it springs back automatically when the look input is released.
			if (autoZeroHorizontalOnMobile) {
				m_TargetAngles.y = Mathf.Lerp (-rotationRange.y * 0.5f, rotationRange.y * 0.5f, inputH * .5f + .5f);
			} else {
				m_TargetAngles.y += inputH * rotationSpeed;
			}
			if (autoZeroVerticalOnMobile) {
				m_TargetAngles.x = Mathf.Lerp (-rotationRange.x * 0.5f, rotationRange.x * 0.5f, inputV * .5f + .5f);
			} else {
				m_TargetAngles.x += inputV * rotationSpeed;
			}
                #else
        // with mouse input, we have direct control with no springback required.
        this.m_TargetAngles.y += inputH * this.rotationSpeed;
        this.m_TargetAngles.x += inputV * this.rotationSpeed;
        #endif

        // clamp values to allowed range
        this.m_TargetAngles.y = Mathf.Clamp(
                                            value : this.m_TargetAngles.y,
                                            min : -this.rotationRange.y * 0.5f,
                                            max : this.rotationRange.y * 0.5f);
        this.m_TargetAngles.x = Mathf.Clamp(
                                            value : this.m_TargetAngles.x,
                                            min : -this.rotationRange.x * 0.5f,
                                            max : this.rotationRange.x * 0.5f);
      } else {
        inputH = Input.mousePosition.x;
        inputV = Input.mousePosition.y;

        // set values to allowed range
        this.m_TargetAngles.y = Mathf.Lerp(
                                           a : -this.rotationRange.y * 0.5f,
                                           b : this.rotationRange.y * 0.5f,
                                           t : inputH / Screen.width);
        this.m_TargetAngles.x = Mathf.Lerp(
                                           a : -this.rotationRange.x * 0.5f,
                                           b : this.rotationRange.x * 0.5f,
                                           t : inputV / Screen.height);
      }

      // smoothly interpolate current values to target angles
      this.m_FollowAngles =
        Vector3.SmoothDamp(
                           current : this.m_FollowAngles,
                           target : this.m_TargetAngles,
                           currentVelocity : ref this.m_FollowVelocity,
                           smoothTime : this.dampingTime);

      // update the actual gameobject's rotation
      this.transform.localRotation = this.m_OriginalRotation
                                     * Quaternion.Euler(
                                                        x : -this.m_FollowAngles.x,
                                                        y : this.m_FollowAngles.y,
                                                        z : 0);
    }
  }
}
