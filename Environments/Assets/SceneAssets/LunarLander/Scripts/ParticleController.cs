using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {
  private ParticleSystem _particle_system;

  // Use this for initialization
  private void Start() { _particle_system = GetComponent<ParticleSystem>(); }

  // Update is called once per frame
  private void Update() {
    if (Input.GetKey(KeyCode.Space)) {
      if (_particle_system.isPlaying) return;
      _particle_system.Play(true);
    } else {
      //_particle_system.Pause (true);
      _particle_system.Stop(true);
    }
  }
}
