using Neodroid.Scripts.Utilities.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class TaskWindow : EditorWindow {
    Texture _icon;
    Vector2 _scroll_position;

    TaskSequence _task_sequence;

    [MenuItem("Neodroid/TaskWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(TaskWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/script.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Task", this._icon, "Window for task descriptions");
    }

    public void OnInspectorUpdate() { this.Repaint(); }

    void OnEnabled() {
      if (!this._task_sequence) this._task_sequence = FindObjectOfType<TaskSequence>();
    }

    void OnGUI() {
      GUILayout.Label("Tasklist", EditorStyles.boldLabel);
      this._task_sequence = FindObjectOfType<TaskSequence>();
      if (this._task_sequence != null) {
        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
        EditorGUILayout.BeginVertical("Box");

        var seq = this._task_sequence.GetSequence();
        if (seq != null) {
          foreach (var g in seq) {
            if (g != null) {
              if (this._task_sequence.CurrentGoalCell != null
                  && this._task_sequence.CurrentGoalCell.name == g.name)
                GUILayout.Label(g.name, EditorStyles.whiteLabel);
              else
                GUILayout.Label(g.name);
            }
          }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
      }
    }
  }
  #endif
}
