using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  class EuclideanDistance : ObjectiveFunction {
    [SerializeField] Transform _g1;
    [SerializeField] Transform _g2;

    public override float InternalEvaluate() {
      return Vector3.Distance(
                              a : this._g1.position,
                              b : this._g2.position);
    }

    void Start() {
      if (this._g1 == null) this._g1 = FindObjectOfType<Actor>().transform;

      if (this._g2 == null) this._g2 = this.transform;
    }
  }
}
