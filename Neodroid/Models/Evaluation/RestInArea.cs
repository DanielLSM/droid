using System.Collections;
using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Models.Environments;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class RestInArea : ObjectiveFunction {
    [SerializeField] Actor _actor;

    [SerializeField] Collider _area;

    [SerializeField] LearningEnvironment _environment;
    [SerializeField] bool _is_resting;

    [SerializeField] Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.OutsideArea;

    [SerializeField] BoundingBox _playable_area;
    [SerializeField] float _resting_time = 3f;
    [SerializeField] Coroutine _wait_for_resting;

    public override float InternalEvaluate() {
      if (this._overlapping == ActorOverlapping.InsideArea && this._is_resting && this._actor.Alive) {
        this._environment.Terminate("Inside goal area");
        return 1f;
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area._bounds.Intersects(this._actor.GetComponent<Collider>().bounds))
          this._environment.Terminate("Actor is outside playable area");
      }

      return 0f;
    }

    public override void InternalReset() {
      if (this._wait_for_resting != null) this.StopCoroutine(this._wait_for_resting);
      this._is_resting = false;
    }

    IEnumerator WaitForResting() {
      yield return new WaitForSeconds(this._resting_time);

      this._is_resting = true;
    }

    void Start() {
      if (!this._area) this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!this._actor) this._actor = FindObjectOfType<Actor>();
      if (!this._environment) this._environment = FindObjectOfType<LearningEnvironment>();
      if (this._obstructions.Length <= 0) this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area) this._playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._area.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._actor.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log("Actor is inside area");
          this._overlapping = ActorOverlapping.InsideArea;
          if (this._wait_for_resting != null) this.StopCoroutine(this._wait_for_resting);
          this._wait_for_resting = this.StartCoroutine(this.WaitForResting());
        }
      }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log("Actor is inside area");
          this._overlapping = ActorOverlapping.InsideArea;
        }
      }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          if (this.Debugging)
            Debug.Log("Actor is outside area");
          this._overlapping = ActorOverlapping.OutsideArea;
          if (this._wait_for_resting != null) this.StopCoroutine(this._wait_for_resting);
        }
      }
    }
  }
}
