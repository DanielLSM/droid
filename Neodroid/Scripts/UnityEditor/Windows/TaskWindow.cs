using Neodroid.Task;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class TaskWindow : EditorWindow {
    private Texture _icon;
    private Vector2 _scroll_position;

    private TaskSequence _task_sequence;

    [MenuItem("Neodroid/TaskWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(TaskWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/script.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Task",
                                    _icon,
                                    "Window for task descriptions");
    }

    public void OnInspectorUpdate() { Repaint(); }

    private void OnEnabled() {
      if (!_task_sequence) _task_sequence = FindObjectOfType<TaskSequence>();
    }

    private void OnGUI() {
      GUILayout.Label(
                      "Tasklist",
                      EditorStyles.boldLabel);
      _task_sequence = FindObjectOfType<TaskSequence>();
      if (_task_sequence != null) {
        _scroll_position = EditorGUILayout.BeginScrollView(_scroll_position);
        EditorGUILayout.BeginVertical("Box");

        var seq = _task_sequence.GetSequence();
        if (seq != null)
          foreach (var g in seq)
            if (g != null)
              if (_task_sequence._current_goal != null && _task_sequence._current_goal.name == g.name)
                GUILayout.Label(
                                g.name,
                                EditorStyles.whiteLabel);
              else
                GUILayout.Label(g.name);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
      }
    }
  }
  #endif
}
