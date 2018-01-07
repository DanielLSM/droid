using Assets.Neodroid.Models.Actors;
using Neodroid.Environments;
using Neodroid.Utilities.BoundingBoxes;
using SceneSpecificAssets.Grasping;
using UnityEngine;

namespace Neodroid.Evaluation {
  public class PoseDeviance : ObjectiveFunction {
    private float peak_reward;

    public override float InternalEvaluate() {
      if (_playable_area != null
          && !_playable_area._bounds.Intersects(_actor.GetComponent<Collider>().bounds)) {
        if (Debugging)
          print("Outside playable area");
        _environment.Terminate("Outside playable area");
      }

      var distance = Mathf.Abs(
                               Vector3.Distance(
                                                _goal.transform.position,
                                                _actor.transform.position));
      var angle = Quaternion.Angle(
                                   _goal.transform.rotation,
                                   _actor.transform.rotation);

      var reward = 0.0f;
      if (!_sparse) {
        reward += 1 / Mathf.Abs(distance + 1);
        reward += 1 / Mathf.Abs(angle + 1);
        if (reward <= peak_reward)
          reward = 0.0f;
        else
          peak_reward = reward;
      }

      if (distance < 0.5) {
        if (Debugging)
          print("Within range of goal");
        reward += 10f;
        _environment.Terminate("Within range of goal");
      }

      return reward;
    }

    public override void InternalReset() { peak_reward = 0.0f; }

    private void Start() {
      if (!_goal) _goal = FindObjectOfType<Transform>();
      if (!_actor) _actor = FindObjectOfType<Actor>();
      if (!_environment) _environment = FindObjectOfType<LearningEnvironment>();
      if (_obstructions.Length <= 0) _obstructions = FindObjectsOfType<Obstruction>();
      if (!_playable_area) _playable_area = FindObjectOfType<BoundingBox>();
    }

    #region Fields

    [Header(
      "Specific",
      order = 102)]
    [SerializeField]
    private bool _sparse = true;

    [SerializeField]
    private Transform _goal;

    [SerializeField]
    private Actor _actor;

    [SerializeField]
    private LearningEnvironment _environment;

    [SerializeField]
    private BoundingBox _playable_area;

    [SerializeField]
    private Obstruction[] _obstructions;

    #endregion
  }
}
