
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Actors;
using Neodroid.Utilities;
using Neodroid.Environments;
using SceneSpecificAssets.Grasping;
using Neodroid.Utilities.BoundingBoxes;
using Neodroid.Observers;

namespace Neodroid.Evaluation {
  public class PoseDeviance : ObjectiveFunction {

    #region Fields

    [Header ("Specific", order = 102)]
    [SerializeField]
    Transform _goal;
    [SerializeField]
    Actor _actor;
    [SerializeField]
    LearningEnvironment _environment;
    [SerializeField]
    Obstruction[] _obstructions;
    [SerializeField]
    BoundingBox _playable_area;
    [SerializeField]
    float peak_reward = 0.0f;

    #endregion

    public override float InternalEvaluate () {
      if (!_playable_area._bounds.Intersects (_actor.GetComponent<Collider> ().bounds)) {
        if (Debugging)
          print ("Outside playable area");
        _environment.Interrupt ("Outside playable area");
      }

      var reward = 0.0f;
      var distance = Mathf.Abs (Vector3.Distance (_goal.transform.position, _actor.transform.position));
      reward += 1 / Mathf.Abs (distance + 1);
      var angle = Quaternion.Angle (_goal.transform.rotation, _actor.transform.rotation);
      reward += 1 / Mathf.Abs (angle + 1);
      if (reward <= peak_reward) {
        reward = 0.0f;
      } else {
        peak_reward = reward;
      }

      if (distance < 0.5) {
        if (Debugging)
          print ("Within range of goal");
        reward += 100f;
        _environment.Interrupt ("Within range of goal");
      }
      return reward;
    }

    public override void InternalReset () {
      peak_reward = 0.0f;
    }

    private void Awake () {
      if (!_goal) {
        _goal = FindObjectOfType<Transform> ();
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
