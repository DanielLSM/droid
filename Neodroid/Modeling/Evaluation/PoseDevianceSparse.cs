
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Actors;
using Neodroid.Utilities;
using Neodroid.Environments;
using SceneSpecificAssets.Grasping;
using Neodroid.Utilities.BoundingBoxes;

namespace Neodroid.Evaluation {
  public class PoseDevianceSparse : ObjectiveFunction {
    public GoalObserver _goal;
    public Actor _actor;
    public LearningEnvironment _environment;

    public Obstruction[] _obstructions;
    public BoundingBox _playable_area;
    public float peak_reward = 0.0f;

    public override float InternalEvaluate () {
      if (!_playable_area._bounds.Intersects (_actor.GetComponent<Collider> ().bounds)) {
        print ("Outside playable area");
        _environment.Interrupt ("Outside playable area");
      }

      var reward = 0.0f;

      var distance = Mathf.Abs (Vector3.Distance (_goal.transform.position, _actor.transform.position));

      if (distance < 0.5) {
        reward = 1.0f;
        print ("Within range of goal");
        _environment.Interrupt ("Within range of goal");
      }
      return reward;
    }

    public override void InternalReset () {
      peak_reward = 0.0f;
    }

    private void Awake () {
      if (!_goal) {
        _goal = FindObjectOfType<GoalObserver> ();
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

    }
  }
}
