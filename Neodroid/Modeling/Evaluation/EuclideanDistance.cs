using Neodroid.Actors;
using UnityEngine;

namespace Neodroid.Evaluation {
  class EuclideanDistance : ObjectiveFunction {

    public Transform g1, g2;

    public override float InternalEvaluate () {
      return Vector3.Distance (g1.position, g2.position);
    }

    private void Start () {
      if (g1 == null) {
        g1 = FindObjectOfType<Actor> ().transform;
      }

      if (g2 == null) {
        g2 = this.transform;
      }
    }
  }
}
