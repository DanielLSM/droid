using Assets.Neodroid.Models.Actors;
using Neodroid.Environments;
using Neodroid.Observers;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using SceneSpecificAssets.Grasping;
using UnityEngine;

namespace Neodroid.Evaluation {
  internal enum ActorOverlapping {
    INSIDE_AREA,
    OUTSIDE_AREA
  }

  internal enum ActorColliding {
    NOT_COLLIDING,
    COLLIDING
  }

  //[RequireComponent (typeof(BoundingBox))]
  //[RequireComponent (typeof(BoxCollider))]
  public class ReachArea : ObjectiveFunction {
    public GameObject _actor;
    public Collider _area;
    public bool _based_on_tags;
    private ActorColliding _colliding = ActorColliding.NOT_COLLIDING;
    public LearningEnvironment _environment;

    public Obstruction[] _obstructions;
    //Used for.. if outside playable area then reset

    private ActorOverlapping _overlapping = ActorOverlapping.OUTSIDE_AREA;
    public BoundingBox _playable_area;

    public override float InternalEvaluate() {
      /*var regularising_term = 0f;

            foreach (var ob in _obstructions) {
              RaycastHit ray_hit;
              Physics.Raycast (_actor.transform.position, (ob.transform.position - _actor.transform.position).normalized, out ray_hit, LayerMask.NameToLayer ("Obstruction"));
              regularising_term += -Mathf.Abs (Vector3.Distance (ray_hit.point, _actor.transform.position));
              //regularising_term += -Mathf.Abs (Vector3.Distance (ob.transform.position, _actor.transform.position));
            }

            reward += 0.2 * regularising_term;*/

      //reward += 1 / Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position)); // Inversely porpotional to the absolute distance, closer higher reward

      if (_overlapping == ActorOverlapping.INSIDE_AREA) {
        _environment.Terminate("Inside goal area");
        return 1f;
      }

      if (_colliding == ActorColliding.COLLIDING)
        _environment.Terminate("Actor colliding with obstruction");
      if (_playable_area && _actor)
        if (!_playable_area._bounds.Intersects(_actor.GetComponent<Collider>().bounds))
          _environment.Terminate("Actor is outside playable area");

      return 0f;
    }

    private void Start() {
      if (!_area) _area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!_actor) _actor = FindObjectOfType<Actor>().gameObject;
      if (!_environment) _environment = FindObjectOfType<LearningEnvironment>();
      if (_obstructions.Length <= 0) _obstructions = FindObjectsOfType<Obstruction>();
      if (!_playable_area) _playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    this,
                                                                    _area.transform,
                                                                    OnCollisionEnterChild,
                                                                    OnTriggerEnterChild,
                                                                    OnCollisionExitChild,
                                                                    OnTriggerExitChild,
                                                                    OnCollisionStayChild,
                                                                    OnTriggerStayChild,
                                                                    Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    this,
                                                                    _actor.transform,
                                                                    OnCollisionEnterChild,
                                                                    OnTriggerEnterChild,
                                                                    OnCollisionExitChild,
                                                                    OnTriggerExitChild,
                                                                    OnCollisionStayChild,
                                                                    OnTriggerStayChild,
                                                                    Debugging);
    }

    private void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (_based_on_tags) {
          if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
            if (Debugging)
              Debug.Log("Actor is inside area");
            _overlapping = ActorOverlapping.INSIDE_AREA;
          }

          if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is colliding");
            _colliding = ActorColliding.COLLIDING;
          }
        } else {
          if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
            if (Debugging)
              Debug.Log("Actor is inside area");
            _overlapping = ActorOverlapping.INSIDE_AREA;
          }

          if (child_game_object == _actor.gameObject && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is colliding");
            _colliding = ActorColliding.COLLIDING;
          }
        }
    }

    private void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (_based_on_tags) {
          if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
            if (Debugging)
              Debug.Log("Actor is inside area");
            _overlapping = ActorOverlapping.INSIDE_AREA;
          }

          if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is colliding");
            _colliding = ActorColliding.COLLIDING;
          }
        } else {
          if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
            if (Debugging)
              Debug.Log("Actor is inside area");
            _overlapping = ActorOverlapping.INSIDE_AREA;
          }

          if (child_game_object == _actor.gameObject && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is colliding");
            _colliding = ActorColliding.COLLIDING;
          }
        }
    }

    private void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (_based_on_tags) {
          if (child_game_object.tag == _area.tag && other_game_object.tag == _actor.tag) {
            if (Debugging)
              Debug.Log("Actor is outside area");
            _overlapping = ActorOverlapping.OUTSIDE_AREA;
          }

          if (child_game_object.tag == _actor.tag && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is not colliding");
            _colliding = ActorColliding.NOT_COLLIDING;
          }
        } else {
          if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
            if (Debugging)
              Debug.Log("Actor is outside area");
            _overlapping = ActorOverlapping.OUTSIDE_AREA;
          }

          if (child_game_object == _actor.gameObject && other_game_object.tag == "Obstruction") {
            if (Debugging)
              Debug.Log("Actor is not colliding");
            _colliding = ActorColliding.NOT_COLLIDING;
          }
        }
    }

    private void OnCollisionEnterChild(GameObject child_game_object, Collision collision) { }

    private void OnCollisionStayChild(GameObject child_game_object, Collision collision) { }

    private void OnCollisionExitChild(GameObject child_game_object, Collision collision) { }
  }
}
