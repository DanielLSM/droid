using Neodroid.Evaluation;
using UnityEngine;

public class CollsionsPunishmentTerm : Term {
  public Collider _a;
  public Collider _b;

  public override float Evaluate() {
    if (_a.bounds.Intersects(_b.bounds))
      return -1;
    return 0;
  }
}
