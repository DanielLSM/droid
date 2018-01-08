using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

namespace UnityStandardAssets.CrossPlatformInput {
  public static class CrossPlatformInputManager {
    public enum ActiveInputMethod {
      Hardware,
      Touch
    }

    static VirtualInput activeInput;

    static readonly VirtualInput s_TouchInput;
    static readonly VirtualInput s_HardwareInput;

    static CrossPlatformInputManager() {
      s_TouchInput = new MobileInput();
      s_HardwareInput = new StandaloneInput();
      #if MOBILE_INPUT
            activeInput = s_TouchInput;
            #else
      activeInput = s_HardwareInput;
      #endif
    }

    public static Vector3 mousePosition { get { return activeInput.MousePosition(); } }

    public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod) {
      switch (activeInputMethod) {
        case ActiveInputMethod.Hardware:
          activeInput = s_HardwareInput;
          break;

        case ActiveInputMethod.Touch:
          activeInput = s_TouchInput;
          break;
      }
    }

    public static bool AxisExists(string name) { return activeInput.AxisExists(name : name); }

    public static bool ButtonExists(string name) { return activeInput.ButtonExists(name : name); }

    public static void RegisterVirtualAxis(VirtualAxis axis) { activeInput.RegisterVirtualAxis(axis : axis); }

    public static void RegisterVirtualButton(VirtualButton button) {
      activeInput.RegisterVirtualButton(button : button);
    }

    public static void UnRegisterVirtualAxis(string name) {
      if (name == null)
        throw new ArgumentNullException(paramName : "name");
      activeInput.UnRegisterVirtualAxis(name : name);
    }

    public static void UnRegisterVirtualButton(string name) {
      activeInput.UnRegisterVirtualButton(name : name);
    }

    // returns a reference to a named virtual axis if it exists otherwise null
    public static VirtualAxis VirtualAxisReference(string name) {
      return activeInput.VirtualAxisReference(name : name);
    }

    // returns the platform appropriate axis for the given name
    public static float GetAxis(string name) {
      return GetAxis(
                     name : name,
                     raw : false);
    }

    public static float GetAxisRaw(string name) {
      return GetAxis(
                     name : name,
                     raw : true);
    }

    // private function handles both types of axis (raw and not raw)
    static float GetAxis(string name, bool raw) {
      return activeInput.GetAxis(
                                 name : name,
                                 raw : raw);
    }

    // -- Button handling --
    public static bool GetButton(string name) { return activeInput.GetButton(name : name); }

    public static bool GetButtonDown(string name) { return activeInput.GetButtonDown(name : name); }

    public static bool GetButtonUp(string name) { return activeInput.GetButtonUp(name : name); }

    public static void SetButtonDown(string name) { activeInput.SetButtonDown(name : name); }

    public static void SetButtonUp(string name) { activeInput.SetButtonUp(name : name); }

    public static void SetAxisPositive(string name) { activeInput.SetAxisPositive(name : name); }

    public static void SetAxisNegative(string name) { activeInput.SetAxisNegative(name : name); }

    public static void SetAxisZero(string name) { activeInput.SetAxisZero(name : name); }

    public static void SetAxis(string name, float value) {
      activeInput.SetAxis(
                          name : name,
                          value : value);
    }

    public static void SetVirtualMousePositionX(float f) { activeInput.SetVirtualMousePositionX(f : f); }

    public static void SetVirtualMousePositionY(float f) { activeInput.SetVirtualMousePositionY(f : f); }

    public static void SetVirtualMousePositionZ(float f) { activeInput.SetVirtualMousePositionZ(f : f); }

    // virtual axis and button classes - applies to mobile input
    // Can be mapped to touch joysticks, tilt, gyro, etc, depending on desired implementation.
    // Could also be implemented by other input devices - kinect, electronic sensors, etc
    public class VirtualAxis {
      public VirtualAxis(string name)
        : this(
               name : name,
               matchToInputSettings : true) { }

      public VirtualAxis(string name, bool matchToInputSettings) {
        this.name = name;
        this.matchWithInputManager = matchToInputSettings;
      }

      public string name { get; private set; }

      public bool matchWithInputManager { get; private set; }

      public float GetValue { get; private set; }

      public float GetValueRaw { get { return this.GetValue; } }

      // removes an axes from the cross platform input system
      public void Remove() { UnRegisterVirtualAxis(name : this.name); }

      // a controller gameobject (eg. a virtual thumbstick) should update this class
      public void Update(float value) { this.GetValue = value; }
    }

    // a controller gameobject (eg. a virtual GUI button) should call the
    // 'pressed' function of this class. Other objects can then read the
    // Get/Down/Up state of this button.
    public class VirtualButton {
      int m_LastPressedFrame = -5;
      int m_ReleasedFrame = -5;

      public VirtualButton(string name)
        : this(
               name : name,
               matchToInputSettings : true) { }

      public VirtualButton(string name, bool matchToInputSettings) {
        this.name = name;
        this.matchWithInputManager = matchToInputSettings;
      }

      public string name { get; private set; }

      public bool matchWithInputManager { get; private set; }

      // these are the states of the button which can be read via the cross platform input system
      public bool GetButton { get; private set; }

      public bool GetButtonDown { get { return this.m_LastPressedFrame - Time.frameCount == -1; } }

      public bool GetButtonUp { get { return this.m_ReleasedFrame == Time.frameCount - 1; } }

      // A controller gameobject should call this function when the button is pressed down
      public void Pressed() {
        if (this.GetButton)
          return;
        this.GetButton = true;
        this.m_LastPressedFrame = Time.frameCount;
      }

      // A controller gameobject should call this function when the button is released
      public void Released() {
        this.GetButton = false;
        this.m_ReleasedFrame = Time.frameCount;
      }

      // the controller gameobject should call Remove when the button is destroyed or disabled
      public void Remove() { UnRegisterVirtualButton(name : this.name); }
    }
  }
}
