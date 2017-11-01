using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Actors;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Managers;
using SceneSpecificAssets.Grasping;
using Neodroid.Utilities.BoundingBoxes;

namespace Neodroid.Evaluation {
  enum ActorOverlapping {
    INSIDE_AREA,
    OUTSIDE_AREA
  }

  enum ActorColliding {
    NOT_COLLIDING,
    COLLIDING
  }


  public class GotoArea : ObjectiveFunction {

    public bool _debug = false;
    public Collider _area;
    public Actor _actor;
    public EnvironmentManager _environment;
    public Obstruction[] _obstructions;
    public BoundingBox _playable_area;
    //Used for.. if outside playable area then reset

    ActorOverlapping _overlapping = ActorOverlapping.OUTSIDE_AREA;
    ActorColliding _colliding = ActorColliding.NOT_COLLIDING;

    public override float Evaluate () {
      var reward = 0f;



      /*var regularising_term = 0f;

      foreach (var ob in _obstructions) {
        RaycastHit ray_hit;
        Physics.Raycast (_actor.transform.position, (ob.transform.position - _actor.transform.position).normalized, out ray_hit, LayerMask.NameToLayer ("Obstruction"));
        regularising_term += -Mathf.Abs (Vector3.Distance (ray_hit.point, _actor.transform.position));
        //regularising_term += -Mathf.Abs (Vector3.Distance (ob.transform.position, _actor.transform.position));
      }

      reward += 0.2 * regularising_term;*/

      reward += 1 / Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position)); // Inversely porpotional to the absolute distance, closer higher reward

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
      if (_obstructions.Length <= 0) {
        _obstructions = FindObjectsOfType<Obstruction> ();
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
