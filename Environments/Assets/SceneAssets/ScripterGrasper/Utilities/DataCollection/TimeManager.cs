using UnityEngine;

namespace SceneAssets.ScripterGrasper.Utilities.DataCollection {
  public class TimeManager : MonoBehaviour {
    readonly float interval_size = 0.02f;

    [Range(
      min : 0.0f,
      max : 10.0f)]
    [SerializeField]  float _time_scale = 1f;

    // Use this for initialization
    void Start() {
      Time.timeScale = this._time_scale;
      Time.fixedDeltaTime = this.interval_size * Time.timeScale;
    }

    // Update is called once per frame
    void Update() { }
  }
}
