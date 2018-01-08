using Neodroid.Models.Managers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Models.Managers.NotUsed {
  public class CurriculumManager : NeodroidManager {
    [SerializeField] Curriculum _curriculum;

    [SerializeField] bool _draw_levels;

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this._draw_levels) {
        var i = 0;
        var len = this._curriculum.Levels.Length;
        foreach (var level in this._curriculum.Levels) {
          if (level.configurable_entries != null && level.configurable_entries.Length > 0) {
            var frac = i++ / (float)len;
            foreach (var entry in level.configurable_entries) {
              var configurable = GameObject.Find(entry.configurable_name);
              if (configurable != null) {
                Gizmos.color = new Color(frac, 0, 1 - frac, 0.1F);
                Gizmos.DrawSphere(configurable.transform.position, entry.MaxValue);
                Gizmos.color = new Color(1, 1, 1, 0.4F);
                Gizmos.DrawWireSphere(configurable.transform.position, entry.MaxValue);
                var pos_up = configurable.transform.position;
                pos_up.y += entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_up, new Color(1, 1, 1, 1));
                var pos_left = configurable.transform.position;
                pos_left.x += entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_left, new Color(1, 1, 1, 1));
                var pos_forward = configurable.transform.position;
                pos_forward.z += entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_forward, new Color(1, 1, 1, 1));
                var pos_down = configurable.transform.position;
                pos_down.y -= entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_down, new Color(1, 1, 1, 1));
                var pos_right = configurable.transform.position;
                pos_right.x -= entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_right, new Color(1, 1, 1, 1));
                var pos_backward = configurable.transform.position;
                pos_backward.z -= entry.MaxValue;
                NeodroidUtilities.DrawString(i.ToString(), pos_backward, new Color(1, 1, 1, 1));
              }
            }
          }
        }
      }
    }
    #endif
  }
}
