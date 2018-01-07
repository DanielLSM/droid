using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  public class IgnoreLightSource : MonoBehaviour {
    [SerializeField]
    bool _ignore_infrared_if_empty = true;
    [SerializeField]
    Light[] _lights_to_ignore;

    // Use this for initialization
    private void Start() {
      if (_lights_to_ignore == null || _lights_to_ignore.Length == 0 && _ignore_infrared_if_empty) {
        var infrared_light_sources = FindObjectsOfType<InfraredLightSource>();
        var lights = new List<Light>();
        foreach (var ils in infrared_light_sources) lights.Add(ils.GetComponent<Light>());
        _lights_to_ignore = lights.ToArray();
      }
    }

    // Update is called once per frame
    private void Update() { }

    private void OnPreCull() {
      if (_lights_to_ignore != null)
        foreach (var l in _lights_to_ignore)
          if (l)
            l.enabled = false;
    }

    private void OnPreRender() {
      if (_lights_to_ignore != null)
        foreach (var l in _lights_to_ignore)
          if (l)
            l.enabled = false;
    }

    private void OnPostRender() {
      if (_lights_to_ignore != null)
        foreach (var l in _lights_to_ignore)
          if (l)
            l.enabled = true;
    }
  }
}
