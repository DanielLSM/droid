﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Agents;
using Neodroid.NeodroidEnvironment.Actors;
using Neodroid.NeodroidEnvironment.Motors;
using Neodroid.NeodroidEnvironment.Observers;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class EnvironmentWindow : EditorWindow {

    [MenuItem ("Neodroid/EnvironmentWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(EnvironmentWindow));      //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

  EnvironmentManager _environment_manager;
  NeodroidAgent[] _agents;
  Vector2 _scroll_position;
  Texture _icon;

    void OnEnable () {
  _icon =  (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Neodroid/Scripts/Windows/Icons/world.png", typeof(Texture2D));
  this.titleContent = new GUIContent("Neo:Env",_icon,"Window for configuring environment");
    }

    void OnGUI () {
  _environment_manager = FindObjectOfType<EnvironmentManager>();
  _environment_manager._frames_spent_resetting = EditorGUILayout.IntField("Frames Spent Resetting", _environment_manager._frames_spent_resetting);
  _agents = FindObjectsOfType<NeodroidAgent> ();

  _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);

  EditorGUILayout.BeginVertical ("Box");
  GUILayout.Label ("Agents");
  foreach(var agent in _agents){
  EditorGUILayout.BeginVertical ("Box");
  agent.enabled = EditorGUILayout.Toggle (agent.name+"Agent",  agent.enabled );
  EditorGUILayout.EndVertical ();
  }
  EditorGUILayout.EndVertical ();

  EditorGUILayout.EndScrollView ();

  if (GUILayout.Button ("Step")) {
  _environment_manager.Step();
  }

      if (GUILayout.Button ("Reset")) {
  _environment_manager.ResetEnvironment();
      }

    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}