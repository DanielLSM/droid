using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Evaluation;

public class CollsionsPunishmentTerm : Term {

  public Collider _a;
  public Collider _b;

  public override float evaluate () {
    if (_a.bounds.Intersects (_b.bounds))
      return -1;
    else
      return 0;
  }
}
