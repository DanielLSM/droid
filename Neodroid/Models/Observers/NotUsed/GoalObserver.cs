using Neodroid.Observers;
using Neodroid.Utilities;
using UnityEngine;

public class GoalObserver : Observer {
  private bool _current_goal;

  public bool _draw_names = true;

  public int _order_index;

  public void SetGoalStatus(bool v) { _current_goal = v; }

  public bool GetGoalStatus() { return _current_goal; }

  #if UNITY_EDITOR
  private void OnDrawGizmosSelected() {
    if (_draw_names)
      if (_current_goal)
        NeodroidUtilities.DrawString(
                                     name,
                                     transform.position,
                                     Color.green);
      else
        NeodroidUtilities.DrawString(
                                     name,
                                     transform.position);
  }
  #endif
}
