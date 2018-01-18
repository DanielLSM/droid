using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class AngularVelocityEvaluation : ObjectiveFunction {
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] bool penalty;

    public override float InternalEvaluate() {
      if (this.penalty) {
        if (this._rigidbody)
          return -this._rigidbody.angularVelocity.magnitude;
      }

      if (this._rigidbody)
        return 1 / (this._rigidbody.angularVelocity.magnitude + 1);

      return 0;
    }

    void Start() {
      if (this._rigidbody == null)
        this._rigidbody = FindObjectOfType<Rigidbody>();
    }
  }
}
