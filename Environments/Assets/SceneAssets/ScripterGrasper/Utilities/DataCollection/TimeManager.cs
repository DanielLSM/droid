using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities.DataCollection {
  public class TimeManager : MonoBehaviour {
    [Range(
      0.0f,
      10.0f)]
    public float _time_scale = 1f;

    private readonly float interval_size = 0.02f;

    // Use this for initialization
    private void Start() {
      Time.timeScale = _time_scale;
      Time.fixedDeltaTime = interval_size * Time.timeScale;
    }

    // Update is called once per frame
    private void Update() { }
  }
}
