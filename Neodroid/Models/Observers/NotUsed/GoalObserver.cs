using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Observers.NotUsed {
  public class GoalObserver : Observer {
    [SerializeField] bool _current_goal;

    [SerializeField] bool _draw_names = true;

    [SerializeField] int _order_index;

    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    public bool DrawNames { get { return this._draw_names; } set { this._draw_names = value; } }

    public void SetGoalStatus(bool v) { this._current_goal = v; }

    public bool GetGoalStatus() { return this._current_goal; }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.DrawNames) {
        if (this._current_goal)
          NeodroidUtilities.DrawString(this.name, this.transform.position, Color.green);
        else
          NeodroidUtilities.DrawString(this.name, this.transform.position);
      }
    }
    #endif
  }
}
