
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Actors;
using Neodroid.Utilities;
using Neodroid.Environments;
using SceneSpecificAssets.Grasping;
using Neodroid.Utilities.BoundingBoxes;

namespace Neodroid.Evaluation {
  public class PoseDeviance : ObjectiveFunction {
    public GoalObserver _goal;
    public Actor _actor;
    public LearningEnvironment _environment;

    public Obstruction[] _obstructions;
    public BoundingBox _playable_area;
    public float peak_reward = 0.0f;

    public override float InternalEvaluate () {
      if (!_playable_area._bounds.Intersects (_actor.GetComponent<Collider> ().bounds)) {
        _environment.Interrupt ();
      }

      var reward = 0.0f;  
      reward += 1 / Mathf.Abs (Vector3.Distance (_goal.transform.position, _actor.transform.position) + 1);
      var angle = Quaternion.Angle (_goal.transform.rotation, _actor.transform.rotation);
      reward += 1 / Mathf.Abs (angle + 1);
      if (reward <= peak_reward) {
        reward = 0.0f;
      } else {
        peak_reward = reward;
      }
      return reward;
    }

    public override void InternalReset () {
      peak_reward = 0.0f;
    }

    private void Start () {
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
