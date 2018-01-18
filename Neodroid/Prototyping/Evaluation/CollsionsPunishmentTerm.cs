using UnityEngine;

namespace Neodroid.Models.Evaluation {
  public class CollsionsPunishmentTerm : Term {
    [SerializeField] Collider _a;

    [SerializeField] Collider _b;

    public override float Evaluate() {
      if (this._a.bounds.Intersects(this._b.bounds))
        return -1;
      return 0;
    }
  }
}
