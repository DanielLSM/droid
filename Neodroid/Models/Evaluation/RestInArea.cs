using System.Collections;
using Neodroid.Environments;
using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class RestInArea : ObjectiveFunction {
    [SerializeField]
    Actor _actor;
    [SerializeField]
    Collider _area;
    [SerializeField] LearningEnvironment _environment;

    [SerializeField] Obstruction[] _obstructions;
    //Used for.. if outside playable area then reset
    [SerializeField]
    ActorOverlapping _overlapping = ActorOverlapping.OutsideArea;
    [SerializeField] BoundingBox _playable_area;
    [SerializeField] bool _is_resting;
    [SerializeField]  float _resting_time = 3f;
    [SerializeField] Coroutine _wait_for_resting;

    public override float InternalEvaluate() {
      if (this._overlapping == ActorOverlapping.InsideArea && this._is_resting && this._actor.Alive) {
        this._environment.Terminate(reason : "Inside goal area");
        return 1f;
      }

      if (this._playable_area && this._actor)
        if (!this._playable_area._bounds.Intersects(bounds : this._actor.GetComponent<Collider>().bounds))
          this._environment.Terminate(reason : "Actor is outside playable area");

      return 0f;
    }

    public override void InternalReset() {
      if (this._wait_for_resting != null) this.StopCoroutine(routine : this._wait_for_resting);
      this._is_resting = false;
    }

    IEnumerator WaitForResting() {
      yield return new WaitForSeconds(seconds : this._resting_time);

      this._is_resting = true;
    }

    void Start() {
      if (!this._area) this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!this._actor) this._actor = FindObjectOfType<Actor>();
      if (!this._environment) this._environment = FindObjectOfType<LearningEnvironment>();
      if (this._obstructions.Length <= 0) this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area) this._playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this._area.transform,
                                                                    on_collision_enter_child : null,
                                                                    on_trigger_enter_child : this
                                                                      .OnTriggerEnterChild,
                                                                    on_collision_exit_child : null,
                                                                    on_trigger_exit_child : this
                                                                      .OnTriggerExitChild,
                                                                    on_collision_stay_child : null,
                                                                    on_trigger_stay_child : this
                                                                      .OnTriggerStayChild,
                                                                    debug : this.Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this._actor.transform,
                                                                    on_collision_enter_child : null,
                                                                    on_trigger_enter_child : this
                                                                      .OnTriggerEnterChild,
                                                                    on_collision_exit_child : null,
                                                                    on_trigger_exit_child : this
                                                                      .OnTriggerExitChild,
                                                                    on_collision_stay_child : null,
                                                                    on_trigger_stay_child : this
                                                                      .OnTriggerStayChild,
                                                                    debug : this.Debugging);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log(message : "Actor is inside area");
          this._overlapping = ActorOverlapping.InsideArea;
          if (this._wait_for_resting != null) this.StopCoroutine(routine : this._wait_for_resting);
          this._wait_for_resting = this.StartCoroutine(routine : this.WaitForResting());
        }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log(message : "Actor is inside area");
          this._overlapping = ActorOverlapping.InsideArea;
        }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor)
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log(message : "Actor is outside area");
          this._overlapping = ActorOverlapping.OutsideArea;
          if (this._wait_for_resting != null) this.StopCoroutine(routine : this._wait_for_resting);
        }
    }
  }
}
