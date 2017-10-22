using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Actors;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.Evaluation {
  enum ActorOverlapping {
    INSIDE_AREA,
    OUTSIDE_AREA
  }

  enum ActorColliding {
    NOT_COLLIDING,
    COLLIDING
  }


  public class OverlappingArea : ObjectiveFunction {

    public bool _debug = false;
    public Collider _area;
    public Actor _actor;
    public EnvironmentManager _environment;

    ActorOverlapping _overlapping = ActorOverlapping.OUTSIDE_AREA;
    ActorColliding _colliding = ActorColliding.NOT_COLLIDING;

    public override float Evaluate () {
      var reward = 0f;

      reward += -Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position));

      if (_overlapping == ActorOverlapping.INSIDE_AREA) {
        reward += 10f; 
        _environment.InterruptEnvironment ();
      } else {
        //reward += 0f;
      }

      if (_colliding == ActorColliding.COLLIDING) {
        reward += -10f; 
        _environment.InterruptEnvironment ();
      } else {
        //reward += 0;
      }

      return reward;
    }

    private void Start () {
      if (!_area) {
        //_area = FindObjectOfType<Collider> ();
      }
      if (!_actor) {
        _actor = FindObjectOfType<Actor> ();
      }
      if (!_environment) {
        _environment = FindObjectOfType<EnvironmentManager> ();
      }

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
        _area.transform, 
        OnCollisionEnterChild, 
        OnTriggerEnterChild, 
        OnCollisionExitChild, 
        OnTriggerExitChild, 
        OnCollisionStayChild, 
        OnTriggerStayChild);
      
      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
        _actor.transform, 
        OnCollisionEnterChild, 
        OnTriggerEnterChild, 
        OnCollisionExitChild, 
        OnTriggerExitChild, 
        OnCollisionStayChild, 
        OnTriggerStayChild);
    }

    void OnTriggerEnterChild (GameObject child_game_object, Collider other_game_object) {
      if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
        if (_debug)
          Debug.Log ("Actor is inside area");
        _overlapping = ActorOverlapping.INSIDE_AREA;
      }
      if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
        if (_debug)
          Debug.Log ("Actor is colliding");
        _colliding = ActorColliding.COLLIDING;
      }
    }

    void OnTriggerStayChild (GameObject child_game_object, Collider other_game_object) {
      if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
        if (_debug)
          Debug.Log ("Actor is inside area");
        _overlapping = ActorOverlapping.INSIDE_AREA;
      }
      if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
        if (_debug)
          Debug.Log ("Actor is colliding");
        _colliding = ActorColliding.COLLIDING;
      }
    }

    void OnTriggerExitChild (GameObject child_game_object, Collider other_game_object) {
      if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
        if (_debug)
          Debug.Log ("Actor is outside area");
        _overlapping = ActorOverlapping.OUTSIDE_AREA;
      }
      if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
        if (_debug)
          Debug.Log ("Actor is not colliding");
        _colliding = ActorColliding.NOT_COLLIDING;
      }
    }

    void OnCollisionEnterChild (GameObject child_game_object, Collision collision) {
    }

    void OnCollisionStayChild (GameObject child_game_object, Collision collision) {
    }

    void OnCollisionExitChild (GameObject child_game_object, Collision collision) {
    }
  }
}
