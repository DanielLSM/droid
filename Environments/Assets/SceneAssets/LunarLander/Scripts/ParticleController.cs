using UnityEngine;

//[ExecuteInEditMode]
namespace SceneAssets.LunarLander.Scripts {
  [RequireComponent( typeof(ParticleSystem))]
  public class ParticleController : MonoBehaviour {
    ParticleSystem _particle_system;

    // Use this for initialization
    void Start() { this._particle_system = this.GetComponent<ParticleSystem>(); }

    // Update is called once per frame
    void Update() {
      if (Input.GetKey(key : KeyCode.Space)) {
        if (this._particle_system.isPlaying) return;
        this._particle_system.Play(withChildren : true);
      } else {
        //_particle_system.Pause (true);
        this._particle_system.Stop(withChildren : true);
      }
    }
  }
}
