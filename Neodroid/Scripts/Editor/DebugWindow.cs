using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Environments;
using Neodroid.Actors;
using Neodroid.Motors;
using Neodroid.Observers;
using Neodroid.Managers;
using Neodroid.Configurations;
using Neodroid.Evaluation;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class DebugWindow : EditorWindow {

    [MenuItem ("Neodroid/DebugWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(DebugWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    EnvironmentManager _environment_manager;
  bool _show_environment_manager_debug = false;

    LearningEnvironment[] _environments;
    bool _show_environments_debug = false;

    Actor[] _actors;
    bool _show_actors_debug = false;

    Motor[] _motors;
    bool _show_motors_debug = false;

    Observer[] _observers;
    bool _show_observers_debug = false;

    ConfigurableGameObject[] _configurables;
    bool _show_configurables_debug = false;

    ObjectiveFunction[] _objective_functions;
    bool _show_objective_functions_debug = false;

    Texture _icon;

    void OnEnable () {
      _environment_manager = FindObjectOfType<EnvironmentManager> ();
      _environments = FindObjectsOfType<LearningEnvironment> ();
      _actors = FindObjectsOfType<Actor> ();
      _motors = FindObjectsOfType<Motor> ();
      _observers = FindObjectsOfType<Observer> ();
      _configurables = FindObjectsOfType<ConfigurableGameObject> ();
      _objective_functions = FindObjectsOfType<ObjectiveFunction> ();
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/information.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Debug", _icon, "Window for controlling debug messages");
    }

    void OnGUI () {
      _environment_manager = FindObjectOfType<EnvironmentManager> ();
      _environments = FindObjectsOfType<LearningEnvironment> ();
      _actors = FindObjectsOfType<Actor> ();
      _motors = FindObjectsOfType<Motor> ();
      _observers = FindObjectsOfType<Observer> ();
      _configurables = FindObjectsOfType<ConfigurableGameObject> ();
      _objective_functions = FindObjectsOfType<ObjectiveFunction> ();

      _show_environment_manager_debug = EditorGUILayout.Toggle ("Debug environment manager", _show_environment_manager_debug);
      _show_environments_debug = EditorGUILayout.Toggle ("Debug all agents", _show_environments_debug);
      _show_actors_debug = EditorGUILayout.Toggle ("Debug all actors", _show_actors_debug);
      _show_motors_debug = EditorGUILayout.Toggle ("Debug all motors", _show_motors_debug);
      _show_observers_debug = EditorGUILayout.Toggle ("Debug all observers", _show_observers_debug);
      _show_configurables_debug = EditorGUILayout.Toggle ("Debug all configurables", _show_configurables_debug);
      _show_objective_functions_debug = EditorGUILayout.Toggle ("Debug all objective functions", _show_objective_functions_debug);

      if (GUILayout.Button ("Apply")) {
        _environment_manager._debug = _show_environment_manager_debug;
        foreach (var agent in _environments) {
          agent._debug = _show_environments_debug;
        }
        foreach (var actor in _actors) {
          actor._debug = _show_actors_debug;
        }
        foreach (var motor in _motors) {
          motor._debug = _show_motors_debug;
        }
        foreach (var observer in _observers) {
          observer._debug = _show_observers_debug;
        }
        foreach (var configurable in _configurables) {
          configurable._debug = _show_configurables_debug;
        }
        foreach (var objective_functions in _objective_functions) {
          objective_functions._debug = _show_objective_functions_debug;
        }
      }
      
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}