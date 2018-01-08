using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Observers.NotUsed {
  public class GoalObserver : Observer {
    [SerializeField]
    bool _current_goal;
    [SerializeField]
    bool _draw_names = true;

    [SerializeField]
    int _order_index = 0;

    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    public void SetGoalStatus (bool v) {
      this._current_goal = v;
    }

    public bool DrawNames {
      get {
        return _draw_names;
      }
      set {
        _draw_names = value;
      }
    }

    public bool GetGoalStatus () {
      return this._current_goal;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
    if (this.DrawNames)
        if (this._current_goal)
          NeodroidUtilities.DrawString(
                                       text : this.name,
                                       world_pos : this.transform.position,
                                       color : Color.green);
        else
          NeodroidUtilities.DrawString(
                                       text : this.name,
                                       world_pos : this.transform.position);
    }
    #endif

  }
}
