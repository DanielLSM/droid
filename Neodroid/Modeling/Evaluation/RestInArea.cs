using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Actors;
using Neodroid.Observers;
using Neodroid.Utilities;
using Neodroid.Environments;
using SceneSpecificAssets.Grasping;
using Neodroid.Utilities.BoundingBoxes;
using System.Threading;

namespace Neodroid.Evaluation {
  public class RestInArea : ObjectiveFunction {
    public Collider _area;
    public Actor _actor;
    public LearningEnvironment _environment;

    public Obstruction[] _obstructions;
    public BoundingBox _playable_area;

    public float _resting_time = 3f;

    ActorOverlapping _overlapping = ActorOverlapping.OUTSIDE_AREA;
    bool _is_resting = false;

    public override float InternalEvaluate () {

      if (_overlapping == ActorOverlapping.INSIDE_AREA && _is_resting && _actor.Alive) {
        _environment.Interrupt ("Inside goal area");
        return 1f;
      }
        
      if (_playable_area && _actor) {
        if (!_playable_area._bounds.Intersects (_actor.GetComponent<Collider> ().bounds)) {
          _environment.Interrupt ("Actor is outside playable area");
        }
      }
        
      return 0f;
    }

    public override void InternalReset () {
      _is_resting = false;
    }

    IEnumerator WaitForResting () {
      yield return new WaitForSeconds (_resting_time);

      _is_resting = true;
    }

    private void Start () {
      if (!_area) {
        _area = FindObjectOfType<Observer> ().gameObject.GetComponent<Collider> ();
      }
      if (!_actor) {
        _actor = FindObjectOfType<Actor> ();
      }
      if (!_environment) {
        _environment = FindObjectOfType<LearningEnvironment> ();
      }
      if (_obstructions.Length <= 0) {
        _obstructions = FindObjectsOfType<Obstruction> ();
      }
      if (!_playable_area) {
        _playable_area = FindObjectOfType<BoundingBox> ();
      }

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
        this,
        _area.transform,
        null,
        OnTriggerEnterChild,
        null,
        OnTriggerExitChild,
        null,
        OnTriggerStayChild,
        Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
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

    void OnTriggerEnterChild (GameObject child_game_object, Collider other_game_object) {
      if (_actor) {
        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log ("Actor is inside area");
          _overlapping = ActorOverlapping.INSIDE_AREA;
          StartCoroutine (WaitForResting ());
        }
      }

    }

    void OnTriggerStayChild (GameObject child_game_object, Collider other_game_object) {
      if (_actor) {

        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log ("Actor is inside area");
          _overlapping = ActorOverlapping.INSIDE_AREA;

        }
      }
    }

    void OnTriggerExitChild (GameObject child_game_object, Collider other_game_object) {
      if (_actor) {

        if (child_game_object == _area.gameObject && other_game_object.gameObject == _actor.gameObject) {
          if (Debugging)
            Debug.Log ("Actor is outside area");
          _overlapping = ActorOverlapping.OUTSIDE_AREA;

          StopCoroutine ("WaitForSeconds");
        }
      }
    }
  }
}
