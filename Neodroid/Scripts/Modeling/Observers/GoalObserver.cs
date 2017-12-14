using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Observers;
using Neodroid.Utilities;

#if UNITY_EDITOR
//using UnityEditor;
#endif

public class GoalObserver : Observer {

  public int _order_index = 0;

  public bool _draw_names = true;
  bool _current_goal = false;

  public void SetGoalStatus (bool v) {
    _current_goal = v;
  }

  //#if UNITY_EDITOR
  void OnDrawGizmosSelected () {
    if (_draw_names) {
      if (_current_goal)
        NeodroidUtilities.DrawString (this.name, this.transform.position, Color.green);
      else
        NeodroidUtilities.DrawString (this.name, this.transform.position);
    }
  }
  //#endif
}
