using Neodroid.Models.Managers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Models.Managers.NotUsed {
  public class CurriculumManager : NeodroidManager {
    [SerializeField]
    Curriculum _curriculum;
    [SerializeField]
    bool _draw_levels;

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this._draw_levels) {
        var i = 0;
        var len = this._curriculum.Levels.Length;
        foreach (var level in this._curriculum.Levels)
          if (level.configurable_entries != null && level.configurable_entries.Length > 0) {
            var frac = i++ / (float)len;
            foreach (var entry in level.configurable_entries) {
              var configurable = GameObject.Find(name : entry.configurable_name);
              if (configurable != null) {
                Gizmos.color = new Color(
                                         r : frac,
                                         g : 0,
                                         b : 1 - frac,
                                         a : 0.1F);
                Gizmos.DrawSphere(
                                  center : configurable.transform.position,
                                  radius : entry.MaxValue);
                Gizmos.color = new Color(
                                         r : 1,
                                         g : 1,
                                         b : 1,
                                         a : 0.4F);
                Gizmos.DrawWireSphere(
                                      center : configurable.transform.position,
                                      radius : entry.MaxValue);
                var pos_up = configurable.transform.position;
                pos_up.y += entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_up,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
                var pos_left = configurable.transform.position;
                pos_left.x += entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_left,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
                var pos_forward = configurable.transform.position;
                pos_forward.z += entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_forward,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
                var pos_down = configurable.transform.position;
                pos_down.y -= entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_down,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
                var pos_right = configurable.transform.position;
                pos_right.x -= entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_right,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
                var pos_backward = configurable.transform.position;
                pos_backward.z -= entry.MaxValue;
                NeodroidUtilities.DrawString(
                                             text : i.ToString(),
                                             world_pos : pos_backward,
                                             color : new Color(
                                                               r : 1,
                                                               g : 1,
                                                               b : 1,
                                                               a : 1));
              }
            }
          }
      }
    }
    #endif
  }
}
