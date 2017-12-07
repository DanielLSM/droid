using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Actors;
using Neodroid.Motors;
using Neodroid.Observers;
using Neodroid.Managers;
using Neodroid.Utilities;
using Neodroid.Configurations;

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

  SimulationManager _simulation_manager;
    LearningEnvironment[] _environments;
    Actor[] _actors;
    Motor[] _motors;
    Observer[] _observers;
    ConfigurableGameObject[] _configurables;
    Vector2 _scroll_position;
    Texture _icon;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/world.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Env", _icon, "Window for configuring environment");
    }

    void OnGUI () {
  _simulation_manager = FindObjectOfType<SimulationManager> ();
  if (_simulation_manager) {
  _simulation_manager._episode_length = EditorGUILayout.IntField ("Episode Length", _simulation_manager._episode_length);
  _simulation_manager._frame_skips = EditorGUILayout.IntField ("Frame skips", _simulation_manager._frame_skips);
  _simulation_manager._resets = EditorGUILayout.IntField ("Resets when resetting", _simulation_manager._resets);
  _simulation_manager._wait_for_reaction_every_frame = EditorGUILayout.Toggle ("Wait For Reaction Every Frame", _simulation_manager._wait_for_reaction_every_frame);


  //_environment._coordinate_system = (CoordinateSystem)EditorGUILayout.EnumPopup ("Coordinate System:", _environment._coordinate_system);

  //EditorGUI.BeginDisabledGroup (_environment._coordinate_system != CoordinateSystem.RelativeToReferencePoint);
  //_environment._coordinate_reference_point = (Transform)EditorGUILayout.ObjectField ("Coordinate Reference Point:", _environment._coordinate_reference_point, typeof(Transform), true);
  //EditorGUI.EndDisabledGroup ();

        _environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<LearningEnvironment> ();
        _actors = NeodroidUtilities.FindAllObjectsOfTypeInScene<Actor> ();
        _motors = NeodroidUtilities.FindAllObjectsOfTypeInScene<Motor> ();
        _observers = NeodroidUtilities.FindAllObjectsOfTypeInScene<Observer> ();
        _configurables = NeodroidUtilities.FindAllObjectsOfTypeInScene<ConfigurableGameObject> ();


        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Environments");
        foreach (var environment in _environments) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (environment.GetEnvironmentIdentifier ());
          environment.enabled = EditorGUILayout.ToggleLeft ("Enabled", environment.enabled && environment.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (environment, typeof(LearningEnvironment), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();


        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Actors");
        foreach (var actor in _actors) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (actor.GetActorIdentifier ());
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

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Configurables");
        foreach (var configurable in _configurables) {
          EditorGUILayout.BeginHorizontal ("Box");
          GUILayout.Label (configurable.GetConfigurableIdentifier ());
          configurable.enabled = EditorGUILayout.ToggleLeft ("Enabled", configurable.enabled && configurable.gameObject.activeSelf, GUILayout.Width (60));
          EditorGUILayout.ObjectField (configurable, typeof(ConfigurableGameObject), true, GUILayout.Width (60));
          EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();

        EditorGUILayout.EndScrollView ();

        EditorGUI.BeginDisabledGroup (!Application.isPlaying);

        if (GUILayout.Button ("Step")) {
        //_simulation_manager.Step ();
        }

        if (GUILayout.Button ("Reset")) {
  //_simulation_manager.ResetEnvironment ();
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