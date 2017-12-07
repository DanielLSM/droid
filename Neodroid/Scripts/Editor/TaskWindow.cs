using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Task;
using Neodroid.Managers;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class TaskWindow : EditorWindow {

    [MenuItem ("Neodroid/TaskWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(TaskWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    TaskSequence _task_sequence;
    Texture _icon;
    Vector2 _scroll_position;


    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/script.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Task", _icon, "Window for task descriptions");
    }


    public void OnInspectorUpdate () {
      this.Repaint ();
    }

    void OnEnabled () {
      if (!_task_sequence) {
        _task_sequence = FindObjectOfType<TaskSequence> ();
      }
    }

    void OnGUI () {
      GUILayout.Label ("Tasklist", EditorStyles.boldLabel);
      _task_sequence = FindObjectOfType<TaskSequence> ();
      if (_task_sequence != null) {
        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);
        EditorGUILayout.BeginVertical ("Box");

        var seq = _task_sequence.GetSequence ();
        if (seq != null) {
          foreach (GoalObserver g in seq) {
            if (_task_sequence._current_goal.name == g.name)
              GUILayout.Label (g.name, EditorStyles.whiteLabel);
            else
              GUILayout.Label (g.name);
          }
        }

        EditorGUILayout.EndVertical ();
        EditorGUILayout.EndScrollView ();
      }
    }
      
  }
  #endif
}
