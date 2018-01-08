using Neodroid.Evaluation;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class AngularVelocity : ObjectiveFunction {
    [SerializeField] Rigidbody _rigidbody;

    public override float InternalEvaluate() {
      if (this._rigidbody) return 1 / (this._rigidbody.angularVelocity.magnitude + 1);
      return 0;
    }

    void Start() {
      if (this._rigidbody == null) this._rigidbody = FindObjectOfType<Rigidbody>();
    }
  }
}
