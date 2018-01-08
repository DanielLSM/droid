using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific {
  public class MobileInput : VirtualInput {
    void AddButton(string name) {
      // we have not registered this button yet so add it, happens in the constructor
      CrossPlatformInputManager.RegisterVirtualButton(
                                                      button : new CrossPlatformInputManager.VirtualButton(
                                                                                                           name
                                                                                                           : name));
    }

    void AddAxes(string name) {
      // we have not registered this button yet so add it, happens in the constructor
      CrossPlatformInputManager.RegisterVirtualAxis(
                                                    axis : new CrossPlatformInputManager.VirtualAxis(
                                                                                                     name :
                                                                                                     name));
    }

    public override float GetAxis(string name, bool raw) {
      if (!this.m_VirtualAxes.ContainsKey(key : name)) this.AddAxes(name : name);
      return this.m_VirtualAxes[key : name].GetValue;
    }

    public override void SetButtonDown(string name) {
      if (!this.m_VirtualButtons.ContainsKey(key : name)) this.AddButton(name : name);
      this.m_VirtualButtons[key : name].Pressed();
    }

    public override void SetButtonUp(string name) {
      if (!this.m_VirtualButtons.ContainsKey(key : name)) this.AddButton(name : name);
      this.m_VirtualButtons[key : name].Released();
    }

    public override void SetAxisPositive(string name) {
      if (!this.m_VirtualAxes.ContainsKey(key : name)) this.AddAxes(name : name);
      this.m_VirtualAxes[key : name].Update(value : 1f);
    }

    public override void SetAxisNegative(string name) {
      if (!this.m_VirtualAxes.ContainsKey(key : name)) this.AddAxes(name : name);
      this.m_VirtualAxes[key : name].Update(value : -1f);
    }

    public override void SetAxisZero(string name) {
      if (!this.m_VirtualAxes.ContainsKey(key : name)) this.AddAxes(name : name);
      this.m_VirtualAxes[key : name].Update(value : 0f);
    }

    public override void SetAxis(string name, float value) {
      if (!this.m_VirtualAxes.ContainsKey(key : name)) this.AddAxes(name : name);
      this.m_VirtualAxes[key : name].Update(value : value);
    }

    public override bool GetButtonDown(string name) {
      if (this.m_VirtualButtons.ContainsKey(key : name))
        return this.m_VirtualButtons[key : name].GetButtonDown;

      this.AddButton(name : name);
      return this.m_VirtualButtons[key : name].GetButtonDown;
    }

    public override bool GetButtonUp(string name) {
      if (this.m_VirtualButtons.ContainsKey(key : name)) return this.m_VirtualButtons[key : name].GetButtonUp;

      this.AddButton(name : name);
      return this.m_VirtualButtons[key : name].GetButtonUp;
    }

    public override bool GetButton(string name) {
      if (this.m_VirtualButtons.ContainsKey(key : name)) return this.m_VirtualButtons[key : name].GetButton;

      this.AddButton(name : name);
      return this.m_VirtualButtons[key : name].GetButton;
    }

    public override Vector3 MousePosition() { return this.virtualMousePosition; }
  }
}
