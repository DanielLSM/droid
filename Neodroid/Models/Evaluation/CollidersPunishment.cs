using Neodroid.Evaluation;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class CollidersPunishment : Term {
    [SerializeField]
    Collider[] _as;
    [SerializeField]
    Collider _b;
    [SerializeField]
    bool _debugging;

    public override float Evaluate() {
      if (this._debugging)
        print(message : "Inside Evaluate");
      foreach (var a in this._as)
        if (a.bounds.Intersects(bounds : this._b.bounds))
          return -1;
      return 0;
    }
  }
}
