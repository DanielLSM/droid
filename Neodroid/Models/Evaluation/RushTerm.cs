using Neodroid.Models.Environments;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class RushTerm : Term {

    [SerializeField] LearningEnvironment _env;

    [SerializeField] float _penalty_size = 0.01f;

    void Awake () {
      if (!this._env) {
        this._env = FindObjectOfType<LearningEnvironment> ();
      }
    }

    public override float Evaluate () {
      if (this._env) {
        return -(1f / _env.EpisodeLength);
      }
      return -_penalty_size;
    }
  }
}