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

    [MenuItem(itemName : "Neodroid/TaskWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(TaskWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                            assetPath : "Assets/Neodroid/Icons/script.png",
                                                            type : typeof(Texture2D));
      this.titleContent = new GUIContent(
                                         text : "Neo:Task",
                                         image : this._icon,
                                         tooltip : "Window for task descriptions");
    }

    public void OnInspectorUpdate() { this.Repaint(); }

    void OnEnabled() {
      if (!this._task_sequence) this._task_sequence = FindObjectOfType<TaskSequence>();
    }

    void OnGUI() {
      GUILayout.Label(
                      text : "Tasklist",
                      style : EditorStyles.boldLabel);
      this._task_sequence = FindObjectOfType<TaskSequence>();
      if (this._task_sequence != null) {
        this._scroll_position = EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
        EditorGUILayout.BeginVertical(style : "Box");

        var seq = this._task_sequence.GetSequence();
        if (seq != null)
          foreach (var g in seq)
            if (g != null)
              if (this._task_sequence.CurrentGoal != null
                  && this._task_sequence.CurrentGoal.name == g.name)
                GUILayout.Label(
                                text : g.name,
                                style : EditorStyles.whiteLabel);
              else
                GUILayout.Label(text : g.name);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
      }
    }
  }
  #endif
}
