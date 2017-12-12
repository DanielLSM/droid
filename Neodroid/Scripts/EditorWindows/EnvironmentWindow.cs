﻿using System.Collections;
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
    bool[] _show_environment_properties = new bool[1];
    Dictionary<string,Actor> actors;
    Dictionary<string,Motor> motors;
    Dictionary<string,Observer> observers;
    Dictionary<string,ConfigurableGameObject> _configurables;
    Vector2 _scroll_position;
    Texture _icon;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/world.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Env", _icon, "Window for configuring environment");
      Setup ();
    }

    void Setup () {
      if (_environments != null) {
        _show_environment_properties = new bool[_environments.Length];
      }
    }

    void OnGUI () {
      SerializedObject serialised_object = new SerializedObject (this);
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

        //if (_environments == null || _environments.Length != NeodroidUtilities.FindAllObjectsOfTypeInScene<LearningEnvironment> ().Length)
        _environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<LearningEnvironment> ();
        if (_show_environment_properties.Length != _environments.Length)
          Setup ();

        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Environments");
        if (_show_environment_properties != null) {
          for (int i = 0; i < _show_environment_properties.Length; i++) {
            _show_environment_properties [i] = EditorGUILayout.Foldout (_show_environment_properties [i], _environments [i].GetEnvironmentIdentifier ());
            if (_show_environment_properties [i]) {
              actors = _environments [i].RegisteredActors;
              observers = _environments [i].RegisteredObservers;
              _configurables = _environments [i].RegisteredConfigurables;

              EditorGUILayout.BeginVertical ("Box");
              _environments [i].enabled = EditorGUILayout.BeginToggleGroup (_environments [i].GetEnvironmentIdentifier (), _environments [i].enabled && _environments [i].gameObject.activeSelf);
              EditorGUILayout.ObjectField (_environments [i], typeof(LearningEnvironment), true);

              EditorGUILayout.BeginVertical ("Box");
              GUILayout.Label ("Actors");
              foreach (var actor in actors) {
                if (actor.Value != null) {
                  motors = actor.Value.RegisteredMotors;

                  EditorGUILayout.BeginVertical ("Box");
                  actor.Value.enabled = EditorGUILayout.BeginToggleGroup (actor.Key, actor.Value.enabled && actor.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField (actor.Value, typeof(Actor), true);

                  EditorGUILayout.BeginVertical ("Box");
                  GUILayout.Label ("Motors");
                  foreach (var motor in motors) {
                    if (motor.Value != null) {
                      EditorGUILayout.BeginVertical ("Box");
                      motor.Value.enabled = EditorGUILayout.BeginToggleGroup (motor.Key, motor.Value.enabled && motor.Value.gameObject.activeSelf);
                      EditorGUILayout.ObjectField (motor.Value, typeof(Motor), true);
                      EditorGUILayout.EndToggleGroup ();

                      EditorGUILayout.EndVertical ();
                    }
                  }
                  EditorGUILayout.EndVertical ();

                  EditorGUILayout.EndToggleGroup ();

                  EditorGUILayout.EndVertical ();
                }
              }
              EditorGUILayout.EndVertical ();

              EditorGUILayout.BeginVertical ("Box");
              GUILayout.Label ("Observers");
              foreach (var observer in observers) {
                if (observer.Value != null) {
                  EditorGUILayout.BeginVertical ("Box");
                  observer.Value.enabled = EditorGUILayout.BeginToggleGroup (observer.Key, observer.Value.enabled && observer.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField (observer.Value, typeof(Observer), true);
                  EditorGUILayout.EndToggleGroup ();
                  EditorGUILayout.EndVertical ();
                }
              }
              EditorGUILayout.EndVertical ();

              EditorGUILayout.BeginVertical ("Box");
              GUILayout.Label ("Configurables");
              foreach (var configurable in _configurables) {
                if (configurable.Value != null) {
                  EditorGUILayout.BeginVertical ("Box");
                  configurable.Value.enabled = EditorGUILayout.BeginToggleGroup (configurable.Key, configurable.Value.enabled && configurable.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField (configurable.Value, typeof(ConfigurableGameObject), true);
                  EditorGUILayout.EndToggleGroup ();
                  EditorGUILayout.EndVertical ();
                }
              }
              EditorGUILayout.EndVertical ();


              EditorGUILayout.EndToggleGroup ();
              EditorGUILayout.EndVertical ();
            }
          }
          EditorGUILayout.EndVertical ();

          EditorGUILayout.EndScrollView ();
          serialised_object.ApplyModifiedProperties ();

          if (GUILayout.Button ("Refresh")) {
            var actors = FindObjectsOfType<Actor> ();
            foreach (var obj in actors) {
              obj.Refresh ();
            }
            var configurables = FindObjectsOfType<ConfigurableGameObject> ();
            foreach (var obj in configurables) {
              obj.Refresh ();
            }
            var motors = FindObjectsOfType<Motor> ();
            foreach (var obj in motors) {
              obj.Refresh ();
            }
            var observers = FindObjectsOfType<Observer> ();
            foreach (var obj in observers) {
              obj.Refresh ();
            }
          }

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
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}