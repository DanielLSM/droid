﻿using Neodroid.Models.Environments;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class RushTerm : Term {
    [SerializeField] PrototypingEnvironment _env;

    [SerializeField] float _penalty_size = 0.01f;

    void Awake() {
      if (!this._env) this._env = FindObjectOfType<PrototypingEnvironment>();
    }

    public override float Evaluate() {
      if (this._env) return -(1f / this._env.EpisodeLength);

      return -this._penalty_size;
    }
  }
}
