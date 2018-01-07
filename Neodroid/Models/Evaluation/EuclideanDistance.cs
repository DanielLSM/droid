using Assets.Neodroid.Models.Actors;
using UnityEngine;

namespace Neodroid.Evaluation {
  internal class EuclideanDistance : ObjectiveFunction {
    public Transform g1,
                     g2;

    public override float InternalEvaluate() {
      return Vector3.Distance(
                              g1.position,
                              g2.position);
    }

    private void Start() {
      if (g1 == null) g1 = FindObjectOfType<Actor>().transform;

      if (g2 == null) g2 = transform;
    }
  }
}
