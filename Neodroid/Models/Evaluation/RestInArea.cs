using System.Collections;
using Assets.Neodroid.Models.Actors;
using Neodroid.Environments;
using Neodroid.Observers;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using SceneSpecificAssets.Grasping;
using UnityEngine;

namespace Neodroid.Evaluation {
  public class RestInArea : ObjectiveFunction {
    public Actor _actor;
    public Collider _area;
    public LearningEnvironment _environment;
    private bool _is_resting;

    public Obstruction[] _obstructions;

    private ActorOverlapping _overlapping = ActorOverlapping.OUTSIDE_AREA;
    public BoundingBox _playable_area;

    public float _resting_time = 3f;
    private Coroutine wait_for_resting;

    public override float InternalEvaluate() {
      if (_overlapping == ActorOverlapping.INSIDE_AREA && _is_resting && _actor.Alive) {
        _environment.Terminate("Inside goal area");
        return 1f;
      }

      if (_playable_area && _actor)
        if (!_playable_area._bounds.Intersects(_actor.GetComponent<Collider>().bounds))
          _environment.Terminate("Actor is outside playable area");

      return 0f;
    }

    public override void InternalReset() {
      if (wait_for_resting != null)
        StopCoroutine(wait_for_resting);
      _is_resting = false;
    }

    private IEnumerator WaitForResting() {
      yield return new WaitForSeconds(_resting_time);

      _is_resting = true;
    }

    private void Start() {
      if (!_area) _area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!_actor) _actor = FindObjectOfType<Actor>();
      if (!_environment) _environment = FindObjectOfType<LearningEnvironment>();
      if (_obstructions.Length <= 0) _obstructions = FindObjectsOfType<Obstruction>();
      if (!_playable_area) _playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    this,
                                                                    _area.transform,
                                                                    null,
                                                                    OnTriggerEnterChild,
                                                                    null,
                                                                    OnTriggerExitChild,
                                                                    null,
                                                                    OnTriggerStayChild,
                                                                    Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    this,
                                                                    _actor.transform,
                                                                    null,
                                                                    OnTriggerEnterChild,
                                                                    null,
                                                                    OnTriggerExitChild,
                                                                    null,
                                                                    OnTriggerStayChild,
                                                                    Debugging);
    }

    private void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log("Actor is inside area");
          _overlapping = ActorOverlapping.INSIDE_AREA;
          if (wait_for_resting != null)
            StopCoroutine(wait_for_resting);
          wait_for_resting = StartCoroutine(WaitForResting());
        }
    }

    private void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log("Actor is inside area");
          _overlapping = ActorOverlapping.INSIDE_AREA;
        }
    }

    private void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (_actor)
        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log("Actor is outside area");
          _overlapping = ActorOverlapping.OUTSIDE_AREA;
          if (wait_for_resting != null)
            StopCoroutine(wait_for_resting);
        }
    }
  }
}
