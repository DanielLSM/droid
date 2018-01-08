using Neodroid.Environments;
using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class PoseDeviance : ObjectiveFunction {
    [SerializeField]
    float _peak_reward;

    public override float InternalEvaluate() {
      if (this._playable_area != null
          && !this._playable_area._bounds.Intersects(bounds : this._actor.GetComponent<Collider>().bounds)) {
        if (this.Debugging)
          print(message : "Outside playable area");
        this._environment.Terminate(reason : "Outside playable area");
      }

      var distance = Mathf.Abs(
                               f : Vector3.Distance(
                                                    a : this._goal.transform.position,
                                                    b : this._actor.transform.position));
      var angle = Quaternion.Angle(
                                   a : this._goal.transform.rotation,
                                   b : this._actor.transform.rotation);

      var reward = 0.0f;
      if (!this._sparse) {
        reward += 1 / Mathf.Abs(f : distance + 1);
        reward += 1 / Mathf.Abs(f : angle + 1);
        if (reward <= this._peak_reward)
          reward = 0.0f;
        else
          this._peak_reward = reward;
      }

      if (distance < 0.5) {
        if (this.Debugging)
          print(message : "Within range of goal");
        reward += 10f;
        this._environment.Terminate(reason : "Within range of goal");
      }

      return reward;
    }

    public override void InternalReset() { this._peak_reward = 0.0f; }

    void Start() {
      if (!this._goal) this._goal = FindObjectOfType<Transform>();
      if (!this._actor) this._actor = FindObjectOfType<Actor>();
      if (!this._environment) this._environment = FindObjectOfType<LearningEnvironment>();
      if (this._obstructions.Length <= 0) this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area) this._playable_area = FindObjectOfType<BoundingBox>();
    }

    #region Fields

    [Header(
      header : "Specific",
      order = 102)]
    [SerializeField]
    bool _sparse = true;

    [SerializeField] Transform _goal;

    [SerializeField] Actor _actor;

    [SerializeField] LearningEnvironment _environment;

    [SerializeField] BoundingBox _playable_area;

    [SerializeField] Obstruction[] _obstructions;

    #endregion
  }
}
