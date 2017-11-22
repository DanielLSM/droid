using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neodroid.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  public class IgnoreLightSource : MonoBehaviour {

    public Light[] _lights_to_ignore;

    // Use this for initialization
    void Start () {
      if (_lights_to_ignore == null) {
        _lights_to_ignore = new Light[1];
      }
    }
	
    // Update is called once per frame
    void Update () {
		
    }

    void OnPreCull () {
      if (_lights_to_ignore != null) {
        foreach (var light in _lights_to_ignore) {
          if (light)
            light.enabled = false;
        }
      }
    }

    void OnPreRender () {
      if (_lights_to_ignore != null) {
        foreach (var light in _lights_to_ignore) {
          if (light)
            light.enabled = false;
        }
      }
    }

    void OnPostRender () {
      if (_lights_to_ignore != null) {
        foreach (var light in _lights_to_ignore) {
          if (light)
            light.enabled = true;
        }
      }
    }
  }
}