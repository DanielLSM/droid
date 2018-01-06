﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Actors;
using Neodroid.Motors;
using Neodroid.Observers;
using Neodroid.Managers;
using Neodroid.Utilities;
using Neodroid.Configurables;
using Neodroid.Messaging.Messages;
using Neodroid.Evaluation;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class SimulationWindow : EditorWindow {

    [MenuItem ("Neodroid/SimulationWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(SimulationWindow));      //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    SimulationManager _simulation_manager;
    LearningEnvironment[] _environments;
    bool[] _show_environment_properties = new bool[1];
    Dictionary<string,Actor> _actors;
    Dictionary<string,Motor> _motors;
    Dictionary<string,Observer> _observers;
    Dictionary<string,ConfigurableGameObject> _configurables;
    Dictionary<string,Resetable> _resetables;
    Vector2 _scroll_position;
    Texture _icon;
    int _preview_image_size = 100;
    Texture _neodroid_icon;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/world.png", typeof(Texture2D));
      _neodroid_icon = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/neodroid_favicon_cut.png", typeof(Texture));
      this.titleContent = new GUIContent ("Neo:Sim", _icon, "Window for configuring simulation");
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
        EditorGUILayout.BeginHorizontal ();

        GUILayout.Label (_neodroid_icon, GUILayout.Width (_preview_image_size), GUILayout.Height (_preview_image_size));

        EditorGUILayout.BeginVertical ();
        _simulation_manager.FrameSkips = EditorGUILayout.IntField ("Frame Skips", _simulation_manager.FrameSkips);
  _simulation_manager.ResetIterations = EditorGUILayout.IntField ("Reset Iterations", _simulation_manager.ResetIterations);
        _simulation_manager.WaitEvery = (WaitOn)EditorGUILayout.EnumPopup ("Wait Every", _simulation_manager.WaitEvery);
        _simulation_manager.TestMotors = EditorGUILayout.Toggle ("Test Motors", _simulation_manager.TestMotors);
        EditorGUILayout.EndVertical ();

        EditorGUILayout.EndHorizontal ();

        _environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<LearningEnvironment> ();
        if (_show_environment_properties.Length != _environments.Length)
          Setup ();

        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);

        EditorGUILayout.BeginVertical ("Box");
        GUILayout.Label ("Environments");
        if (_show_environment_properties != null) {
          for (int i = 0; i < _show_environment_properties.Length; i++) {
            _show_environment_properties [i] = EditorGUILayout.Foldout (_show_environment_properties [i], _environments [i].EnvironmentIdentifier);
            if (_show_environment_properties [i]) {
              _actors = _environments [i].Actors;
              _observers = _environments [i].Observers;
              _configurables = _environments [i].Configurables;
              _resetables = _environments [i].Resetables;

              EditorGUILayout.BeginVertical ("Box");
              _environments [i].enabled = EditorGUILayout.BeginToggleGroup (_environments [i].EnvironmentIdentifier,
                _environments [i].enabled && _environments [i].gameObject.activeSelf);
              EditorGUILayout.ObjectField (_environments [i], typeof(LearningEnvironment), true);
              _environments [i].CoordinateSystem = (CoordinateSystem)EditorGUILayout.EnumPopup ("Coordinate system",
                _environments [i].CoordinateSystem);
              EditorGUI.BeginDisabledGroup (_environments [i].CoordinateSystem != CoordinateSystem.RelativeToReferencePoint);
              _environments [i].CoordinateReferencePoint = (Transform)EditorGUILayout.ObjectField ("Reference point",
                _environments [i].CoordinateReferencePoint, typeof(Transform), true);
              EditorGUI.EndDisabledGroup ();
              _environments [i].ObjectiveFunction = (ObjectiveFunction)EditorGUILayout.ObjectField ("Objective function",
                _environments [i].ObjectiveFunction, typeof(ObjectiveFunction), true);
  _environments [i].EpisodeLength = EditorGUILayout.IntField ("Episode Length", _environments [i].EpisodeLength);

              EditorGUILayout.BeginVertical ("Box");
              GUILayout.Label ("Actors");
              foreach (var actor in _actors) {
                if (actor.Value != null) {
                  _motors = actor.Value.Motors;

                  EditorGUILayout.BeginVertical ("Box");
                  actor.Value.enabled = EditorGUILayout.BeginToggleGroup (actor.Key, actor.Value.enabled && actor.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField (actor.Value, typeof(Actor), true);

                  EditorGUILayout.BeginVertical ("Box");
                  GUILayout.Label ("Motors");
                  foreach (var motor in _motors) {
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
              foreach (var observer in _observers) {
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

              EditorGUILayout.BeginVertical ("Box");
              GUILayout.Label ("Resetables");
              foreach (var resetable in _resetables) {
                if (resetable.Value != null) {
                  EditorGUILayout.BeginVertical ("Box");
                  resetable.Value.enabled = EditorGUILayout.BeginToggleGroup (resetable.Key, resetable.Value.enabled && resetable.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField (resetable.Value, typeof(Resetable), true);
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
            Refresh ();
          }

          EditorGUI.BeginDisabledGroup (!Application.isPlaying);

          if (GUILayout.Button ("Step")) {
            _simulation_manager.ReactInEnvironments (new Reaction (new ReactionParameters (true, true, false, false, false), null, null, null));
          }

          if (GUILayout.Button ("Reset")) {
            _simulation_manager.ReactInEnvironments (new Reaction (new ReactionParameters (true, false, true, false, false), null, null, null));
          }

          EditorGUI.EndDisabledGroup ();
        }
      }
    }

    void Refresh () {
      var actors = FindObjectsOfType<Actor> ();
      foreach (var obj in actors) {
        obj.RefreshAwake ();
      }
      var configurables = FindObjectsOfType<ConfigurableGameObject> ();
      foreach (var obj in configurables) {
        obj.RefreshAwake ();
      }
      var motors = FindObjectsOfType<Motor> ();
      foreach (var obj in motors) {
        obj.RefreshAwake ();
      }
      var observers = FindObjectsOfType<Observer> ();
      foreach (var obj in observers) {
        obj.RefreshAwake ();
      }
      foreach (var obj in actors) {
        obj.RefreshStart ();
      }
      foreach (var obj in configurables) {
        obj.RefreshStart ();
      }
      foreach (var obj in motors) {
        obj.RefreshStart ();
      }
      foreach (var obj in observers) {
        obj.RefreshStart ();
      }
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}