using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Managers {
  public class CurriculumManager : NeodroidManager {
    public Curriculum _curriculum;
    public bool _draw_levels;

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
      if (_draw_levels) {
        var i = 0;
        var len = _curriculum._levels.Length;
        foreach (var level in _curriculum._levels)
          if (level.configurable_entries != null && level.configurable_entries.Length > 0) {
            var frac = i++ / (float)len;
            foreach (var entry in level.configurable_entries) {
              var configurable = GameObject.Find(entry.configurable_name);
              if (configurable != null) {
                Gizmos.color = new Color(
                                         frac,
                                         0,
                                         1 - frac,
                                         0.1F);
                Gizmos.DrawSphere(
                                  configurable.transform.position,
                                  entry.max_value);
                Gizmos.color = new Color(
                                         1,
                                         1,
                                         1,
                                         0.4F);
                Gizmos.DrawWireSphere(
                                      configurable.transform.position,
                                      entry.max_value);
                var pos_up = configurable.transform.position;
                pos_up.y += entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_up,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
                var pos_left = configurable.transform.position;
                pos_left.x += entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_left,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
                var pos_forward = configurable.transform.position;
                pos_forward.z += entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_forward,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
                var pos_down = configurable.transform.position;
                pos_down.y -= entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_down,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
                var pos_right = configurable.transform.position;
                pos_right.x -= entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_right,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
                var pos_backward = configurable.transform.position;
                pos_backward.z -= entry.max_value;
                NeodroidUtilities.DrawString(
                                             i.ToString(),
                                             pos_backward,
                                             new Color(
                                                       1,
                                                       1,
                                                       1,
                                                       1));
              }
            }
          }
      }
    }
    #endif
  }
}
