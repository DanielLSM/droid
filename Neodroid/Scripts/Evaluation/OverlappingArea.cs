using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Actors;

namespace Neodroid.Evaluation {
  public class OverlappingArea : ObjectiveFunction {

    public bool _debug;
    public Collider _area;
    public Actor _actor;

    bool _overlapping = false;

    public override float Evaluate () {
      if (_overlapping) {
        return 1; 
      } else {
        return -1;
      }
    }

    private void Start () {
      if (!_area) {
        _area = GetComponent<Collider> ();
      }
      if (!_actor) {
        _actor = FindObjectOfType<Actor> ();
      }
    }

    void OnTriggerEnter (Collider other) {
      if (other.tag == _actor.tag) {
        if (_debug)
          Debug.Log ("Actor is inside area");
        _overlapping = true;
      }
    }

    void OnTriggerExit (Collider other) {
      if (other.tag == _actor.tag) {
        if (_debug)
          Debug.Log ("Actor is outside area");
        _overlapping = false;
      }
    }
  }
}
