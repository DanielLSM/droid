using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Task;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class TaskWindow : EditorWindow {

    [MenuItem ("Neodroid/TaskWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(TaskWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    NeodroidTask _sequence;
    Texture _icon;
    string _status;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Scripts/Windows/Icons/script.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Task", _icon, "Window for task descriptions");
    }


    public void OnInspectorUpdate () {
      this.Repaint ();
    }

    void OnEnabled () {
      if (!_sequence) {
        _sequence = FindObjectOfType<TaskSequence> ();
      }
    }

    void OnGUI () {
      EditorGUILayout.LabelField ("Status: ", _status);
    }

    void Update () {
      if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
        this.Repaint ();
      } else {
        _status = "Waiting for Editor to Play";
      }
    }
  }
  #endif
}
