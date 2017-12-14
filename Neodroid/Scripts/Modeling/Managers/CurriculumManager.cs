﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Utilities;

public class CurriculumManager : MonoBehaviour {

  public Curriculum _curriculum;
  public bool _draw_levels = false;

  // Use this for initialization
  void Start () {
		
  }

  void OnDrawGizmosSelected () {
    if (_draw_levels) {
      int i = 0;
      int len = _curriculum._levels.Length;
      foreach (var level in _curriculum._levels) {
        if (level.configurable_entries != null && level.configurable_entries.Length > 0) {
          float frac = i++ / (float)len;
          foreach (var entry in level.configurable_entries) {
            var configurable = GameObject.Find (entry.configurable_name);
            if (configurable != null) {
              
              Gizmos.color = new Color (frac, 0, 1 - frac, 0.1F);
              Gizmos.DrawSphere (configurable.transform.position, entry.max_value);
              Gizmos.color = new Color (1, 1, 1, 0.4F);
              Gizmos.DrawWireSphere (configurable.transform.position, entry.max_value);
              var pos_up = configurable.transform.position;
              pos_up.y += entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_up, new Color (1, 1, 1, 1));
              var pos_left = configurable.transform.position;
              pos_left.x += entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_left, new Color (1, 1, 1, 1));
              var pos_forward = configurable.transform.position;
              pos_forward.z += entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_forward, new Color (1, 1, 1, 1));
              var pos_down = configurable.transform.position;
              pos_down.y -= entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_down, new Color (1, 1, 1, 1));
              var pos_right = configurable.transform.position;
              pos_right.x -= entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_right, new Color (1, 1, 1, 1));
              var pos_backward = configurable.transform.position;
              pos_backward.z -= entry.max_value;
              NeodroidUtilities.DrawString (i.ToString (), pos_backward, new Color (1, 1, 1, 1));
            }
          }
        }
      }
    }
  }


  // Update is called once per frame
  void Update () {
		
  }
}
