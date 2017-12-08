using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Observers;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalObserver : Observer {

  public int _order_index = 0;

  public bool _draw_names = true;
  bool _current_goal = false;

  public void SetGoalStatus (bool v) {
    _current_goal = v;
  }

  #if UNITY_EDITOR
  void OnDrawGizmos () {
    if (_draw_names) {
      if (_current_goal)
        drawString (this.name, this.transform.position, Color.green);
      else
        drawString (this.name, this.transform.position);
    }
  }

  static public void drawString (string text, Vector3 worldPos, Color? color = null, float oX = 0, float oY = 0) {

    UnityEditor.Handles.BeginGUI ();

    var restoreColor = GUI.color;

    if (color.HasValue)
      GUI.color = color.Value;
    var view = UnityEditor.SceneView.currentDrawingSceneView;
    Vector3 screenPos = view.camera.WorldToScreenPoint (worldPos);

    if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
      GUI.color = restoreColor;
      UnityEditor.Handles.EndGUI ();
      return;
    }

    UnityEditor.Handles.Label (TransformByPixel (worldPos, oX, oY), text);

    GUI.color = restoreColor;
    UnityEditor.Handles.EndGUI ();
  }

  static Vector3 TransformByPixel (Vector3 position, float x, float y) {
    return TransformByPixel (position, new Vector3 (x, y));
  }

  static Vector3 TransformByPixel (Vector3 position, Vector3 translateBy) {
    Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
    if (cam)
      return cam.ScreenToWorldPoint (cam.WorldToScreenPoint (position) + translateBy);
    else
      return position;
  }
  #endif
}
