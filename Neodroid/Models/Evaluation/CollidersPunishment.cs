using Neodroid.Evaluation;
using UnityEngine;

public class CollidersPunishment : Term {
  public Collider[] _as;
  public Collider _b;

  public bool _debugging;

  public override float Evaluate() {
    if (_debugging)
      print("Inside Evaluate");
    foreach (var _a in _as)
      if (_a.bounds.Intersects(_b.bounds))
        return -1;
    return 0;
  }
}
