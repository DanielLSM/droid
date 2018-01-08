using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific {
  public class StandaloneInput : VirtualInput {
    public override float GetAxis(string name, bool raw) {
      return raw ? Input.GetAxisRaw(axisName : name) : Input.GetAxis(axisName : name);
    }

    public override bool GetButton(string name) { return Input.GetButton(buttonName : name); }

    public override bool GetButtonDown(string name) { return Input.GetButtonDown(buttonName : name); }

    public override bool GetButtonUp(string name) { return Input.GetButtonUp(buttonName : name); }

    public override void SetButtonDown(string name) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override void SetButtonUp(string name) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override void SetAxisPositive(string name) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override void SetAxisNegative(string name) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override void SetAxisZero(string name) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override void SetAxis(string name, float value) {
      throw new Exception(
                          message :
                          " This is not possible to be called for standalone input. Please check your platform and code where this is called");
    }

    public override Vector3 MousePosition() { return Input.mousePosition; }
  }
}
