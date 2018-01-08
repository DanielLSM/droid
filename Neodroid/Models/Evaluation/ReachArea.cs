using Neodroid.Environments;
using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  enum ActorOverlapping {
    InsideArea,
    OutsideArea
  }

  enum ActorColliding {
    NotColliding,
    Colliding
  }

  //[RequireComponent (typeof(BoundingBox))]
  //[RequireComponent (typeof(BoxCollider))]
  public class ReachArea : ObjectiveFunction {
    [SerializeField]
     GameObject _actor;
    [SerializeField]
     Collider _area;
    [SerializeField] bool _based_on_tags;
    [SerializeField]ActorColliding _colliding = ActorColliding.NotColliding;
    [SerializeField] LearningEnvironment _environment;

    [SerializeField] Obstruction[] _obstructions;
    //Used for.. if outside playable area then reset
    [SerializeField]
    ActorOverlapping _overlapping = ActorOverlapping.OutsideArea;
    [SerializeField] BoundingBox _playable_area;

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

      if (this._overlapping == ActorOverlapping.InsideArea) {
        this._environment.Terminate(reason : "Inside goal area");
        return 1f;
      }

      if (this._colliding == ActorColliding.Colliding)
        this._environment.Terminate(reason : "Actor colliding with obstruction");
      if (this._playable_area && this._actor)
        if (!this._playable_area._bounds.Intersects(bounds : this._actor.GetComponent<Collider>().bounds))
          this._environment.Terminate(reason : "Actor is outside playable area");

      return 0f;
    }

    void Start() {
      if (!this._area) this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!this._actor) this._actor = FindObjectOfType<Actor>().gameObject;
      if (!this._environment) this._environment = FindObjectOfType<LearningEnvironment>();
      if (this._obstructions.Length <= 0) this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area) this._playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this._area.transform,
                                                                    on_collision_enter_child : this
                                                                      .OnCollisionEnterChild,
                                                                    on_trigger_enter_child : this
                                                                      .OnTriggerEnterChild,
                                                                    on_collision_exit_child : this
                                                                      .OnCollisionExitChild,
                                                                    on_trigger_exit_child : this
                                                                      .OnTriggerExitChild,
                                                                    on_collision_stay_child : this
                                                                      .OnCollisionStayChild,
                                                                    on_trigger_stay_child : this
                                                                      .OnTriggerStayChild,
                                                                    debug : this.Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this._actor.transform,
                                                                    on_collision_enter_child : this
                                                                      .OnCollisionEnterChild,
                                                                    on_trigger_enter_child : this
                                                                      .OnTriggerEnterChild,
                                                                    on_collision_exit_child : this
                                                                      .OnCollisionExitChild,
                                                                    on_trigger_exit_child : this
                                                                      .OnTriggerExitChild,
                                                                    on_collision_stay_child : this
                                                                      .OnCollisionStayChild,
                                                                    on_trigger_stay_child : this
                                                                      .OnTriggerStayChild,
                                                                    debug : this.Debugging);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log(message : "Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is colliding");
            this._colliding = ActorColliding.Colliding;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log(message : "Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is colliding");
            this._colliding = ActorColliding.Colliding;
          }
        }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log(message : "Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is colliding");
            this._colliding = ActorColliding.Colliding;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log(message : "Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is colliding");
            this._colliding = ActorColliding.Colliding;
          }
        }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log(message : "Actor is outside area");
            this._overlapping = ActorOverlapping.OutsideArea;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is not colliding");
            this._colliding = ActorColliding.NotColliding;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log(message : "Actor is outside area");
            this._overlapping = ActorOverlapping.OutsideArea;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log(message : "Actor is not colliding");
            this._colliding = ActorColliding.NotColliding;
          }
        }
    }

    void OnCollisionEnterChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionStayChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionExitChild(GameObject child_game_object, Collision collision) { }
  }
}
