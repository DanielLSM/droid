using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput {
  public class ButtonHandler : MonoBehaviour {
    public string Name;

    void OnEnable() { }

    public void SetDownState() { CrossPlatformInputManager.SetButtonDown(name : this.Name); }

    public void SetUpState() { CrossPlatformInputManager.SetButtonUp(name : this.Name); }

    public void SetAxisPositiveState() { CrossPlatformInputManager.SetAxisPositive(name : this.Name); }

    public void SetAxisNeutralState() { CrossPlatformInputManager.SetAxisZero(name : this.Name); }

    public void SetAxisNegativeState() { CrossPlatformInputManager.SetAxisNegative(name : this.Name); }

    public void Update() { }
  }
}
