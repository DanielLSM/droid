using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Agents;
using Neodroid.Actors;
using Neodroid.Motors;
using Neodroid.Observers;
using Neodroid.Managers;
using Neodroid.Utilities;

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
    Actor[] _actors;
    Motor[] _motors;
    Observer[] _observers;
    Vector2 _scroll_position;
    Texture _icon;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Scripts/Windows/Icons/world.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Env", _icon, "Window for configuring environment");
    }

    void OnGUI () {
      _environment_manager = FindObjectOfType<EnvironmentManager> ();
      if (_environment_manager) {
        _environment_manager._episode_length = EditorGUILayout.IntField ("Episode Length", _environment_manager._episode_length);
        _environment_manager._resets = EditorGUILayout.IntField ("Resets when resetting", _environment_manager._resets);
        _environment_manager._wait_for_reaction_every_frame = EditorGUILayout.Toggle ("Wait For Reaction Every Frame", _environment_manager._wait_for_reaction_every_frame);
        _environment_manager._coordinate_system = (CoordinateSystem)EditorGUILayout.EnumPopup ("Coordinate System:", _environment_manager._coordinate_system);

        EditorGUI.BeginDisabledGroup (_environment_manager._coordinate_system != CoordinateSystem.RelativeToReferencePoint);
        _environment_manager._coordinate_reference_point = (Transform)EditorGUILayout.ObjectField ("Coordinate Reference Point:", _environment_manager._coordinate_reference_point, typeof(Transform), true);
        EditorGUI.EndDisabledGroup ();

        _agents = NeodroidUtilities.FindAllObjectsOfTypeInScene<NeodroidAgent> ();
        _actors = NeodroidUtilities.FindAllObjectsOfTypeInScene<Actor> ();
        _motors = NeodroidUtilities.FindAllObjectsOfTypeInScene<Motor> ();
        _observers = NeodroidUtilities.FindAllObjectsOfTypeInScene<Observer> ();


        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Agents");
        foreach (var agent in _agents) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (agent.name);
          agent.enabled = EditorGUILayout.ToggleLeft ("Enabled", agent.enabled && agent.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (agent, typeof(NeodroidAgent), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();


        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Actors");
        foreach (var actor in _actors) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (actor.name);
          actor.enabled = EditorGUILayout.ToggleLeft ("Enabled", actor.enabled && actor.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (actor, typeof(Actor), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();


        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Motors");
        foreach (var motor in _motors) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (motor.GetMotorIdentifier ());
          motor.enabled = EditorGUILayout.ToggleLeft ("Enabled", motor.enabled && motor.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (motor, typeof(Motor), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Observers");
        foreach (var observer in _observers) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (observer.GetObserverIdentifier ());
          observer.enabled = EditorGUILayout.ToggleLeft ("Enabled", observer.enabled && observer.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (observer, typeof(Observer), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();

        EditorGUILayout.EndScrollView ();

        EditorGUI.BeginDisabledGroup (!Application.isPlaying);

        if (GUILayout.Button ("Step")) {
          _environment_manager.Step ();
        }

        if (GUILayout.Button ("Reset")) {
          _environment_manager.ResetEnvironment ();
        }

        EditorGUI.EndDisabledGroup ();
      }
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}