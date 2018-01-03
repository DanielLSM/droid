using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Evaluation;

public class CollidersPunishment : Term {

  public bool _debugging = false;

  public Collider[] _as;
  public Collider _b;

  public override float Evaluate () {
    if (_debugging)
      print ("Inside Evaluate");
    foreach (var _a in _as) {
      if (_a.bounds.Intersects (_b.bounds)) {
        return -1;
      }
    }
    return 0;
  }
}
