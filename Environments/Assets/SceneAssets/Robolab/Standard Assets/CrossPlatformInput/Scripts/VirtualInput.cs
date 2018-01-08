using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput {
  public abstract class VirtualInput {
    protected List<string> m_AlwaysUseVirtual = new List<string>();

    protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> m_VirtualAxes =
      new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

    // Dictionary to store the name relating to the virtual axes
    protected Dictionary<string, CrossPlatformInputManager.VirtualButton> m_VirtualButtons =
      new Dictionary<string, CrossPlatformInputManager.VirtualButton>();

    public Vector3 virtualMousePosition { get; private set; }
    // list of the axis and button names that have been flagged to always use a virtual axis or button

    public bool AxisExists(string name) { return this.m_VirtualAxes.ContainsKey(key : name); }

    public bool ButtonExists(string name) { return this.m_VirtualButtons.ContainsKey(key : name); }

    public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis) {
      // check if we already have an axis with that name and log and error if we do
      if (this.m_VirtualAxes.ContainsKey(key : axis.name)) {
        Debug.LogError(message : "There is already a virtual axis named " + axis.name + " registered.");
      } else {
        // add any new axes
        this.m_VirtualAxes.Add(
                               key : axis.name,
                               value : axis);

        // if we dont want to match with the input manager setting then revert to always using virtual
        if (!axis.matchWithInputManager) this.m_AlwaysUseVirtual.Add(item : axis.name);
      }
    }

    public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button) {
      // check if already have a buttin with that name and log an error if we do
      if (this.m_VirtualButtons.ContainsKey(key : button.name)) {
        Debug.LogError(message : "There is already a virtual button named " + button.name + " registered.");
      } else {
        // add any new buttons
        this.m_VirtualButtons.Add(
                                  key : button.name,
                                  value : button);

        // if we dont want to match to the input manager then always use a virtual axis
        if (!button.matchWithInputManager) this.m_AlwaysUseVirtual.Add(item : button.name);
      }
    }

    public void UnRegisterVirtualAxis(string name) {
      // if we have an axis with that name then remove it from our dictionary of registered axes
      if (this.m_VirtualAxes.ContainsKey(key : name)) this.m_VirtualAxes.Remove(key : name);
    }

    public void UnRegisterVirtualButton(string name) {
      // if we have a button with this name then remove it from our dictionary of registered buttons
      if (this.m_VirtualButtons.ContainsKey(key : name)) this.m_VirtualButtons.Remove(key : name);
    }

    // returns a reference to a named virtual axis if it exists otherwise null
    public CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name) {
      return this.m_VirtualAxes.ContainsKey(key : name) ? this.m_VirtualAxes[key : name] : null;
    }

    public void SetVirtualMousePositionX(float f) {
      this.virtualMousePosition = new Vector3(
                                              x : f,
                                              y : this.virtualMousePosition.y,
                                              z : this.virtualMousePosition.z);
    }

    public void SetVirtualMousePositionY(float f) {
      this.virtualMousePosition = new Vector3(
                                              x : this.virtualMousePosition.x,
                                              y : f,
                                              z : this.virtualMousePosition.z);
    }

    public void SetVirtualMousePositionZ(float f) {
      this.virtualMousePosition = new Vector3(
                                              x : this.virtualMousePosition.x,
                                              y : this.virtualMousePosition.y,
                                              z : f);
    }

    public abstract float GetAxis(string name, bool raw);

    public abstract bool GetButton(string name);
    public abstract bool GetButtonDown(string name);
    public abstract bool GetButtonUp(string name);

    public abstract void SetButtonDown(string name);
    public abstract void SetButtonUp(string name);
    public abstract void SetAxisPositive(string name);
    public abstract void SetAxisNegative(string name);
    public abstract void SetAxisZero(string name);
    public abstract void SetAxis(string name, float value);
    public abstract Vector3 MousePosition();
  }
}
