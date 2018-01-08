using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput {
  public class InputAxisScrollbar : MonoBehaviour {
    public string axis;

    void Update() { }

    public void HandleInput(float value) {
      CrossPlatformInputManager.SetAxis(
                                        name : this.axis,
                                        value : value * 2f - 1f);
    }
  }
}
