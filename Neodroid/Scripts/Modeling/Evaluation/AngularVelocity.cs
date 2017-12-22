using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Evaluation;

public class AngularVelocity : ObjectiveFunction {

  public Rigidbody _rigidbody;

  public override float InternalEvaluate () {
    if (_rigidbody) {
      return 1 / (_rigidbody.angularVelocity.magnitude + 1);
    }
    return 0;
  }

  private void Start () {
    if (_rigidbody == null) {
      _rigidbody = FindObjectOfType<Rigidbody> ();
    }
  }
}
