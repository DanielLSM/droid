using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Agents;
using Neodroid.Actors;
using Neodroid.Motors;
using Neodroid.Observers;
using Neodroid.Managers;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class DebugWindow : EditorWindow {

    [MenuItem ("Neodroid/DebugWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(DebugWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    EnvironmentManager _envionment_manager;
    bool _show_envionment_manager_debug = false;

    NeodroidAgent[] _agents;
    bool _show_agents_debug = false;

    Actor[] _actors;
    bool _show_actors_debug = false;

    Motor[] _motors;
    bool _show_motors_debug = false;

    Observer[] _observers;
    bool _show_observers_debug = false;

    Texture _icon;

    void OnEnable () {
      _envionment_manager = FindObjectOfType<EnvironmentManager> ();
      _agents = FindObjectsOfType<NeodroidAgent> ();
      _actors = FindObjectsOfType<Actor> ();
      _motors = FindObjectsOfType<Motor> ();
      _observers = FindObjectsOfType<Observer> ();
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Scripts/Windows/Icons/information.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Debug", _icon, "Window for controlling debug messages");
    }

    void OnGUI () {
      _envionment_manager = FindObjectOfType<EnvironmentManager> ();
      _agents = FindObjectsOfType<NeodroidAgent> ();
      _actors = FindObjectsOfType<Actor> ();
      _motors = FindObjectsOfType<Motor> ();
      _observers = FindObjectsOfType<Observer> ();

      _show_envionment_manager_debug = EditorGUILayout.Toggle ("Debug environment manager", _show_envionment_manager_debug);
      _show_agents_debug = EditorGUILayout.Toggle ("Debug all agents", _show_agents_debug);
      _show_actors_debug = EditorGUILayout.Toggle ("Debug all actors", _show_actors_debug);
      _show_motors_debug = EditorGUILayout.Toggle ("Debug all motors", _show_motors_debug);
      _show_observers_debug = EditorGUILayout.Toggle ("Debug all observers", _show_observers_debug);

      if (GUILayout.Button ("Apply")) {
        _envionment_manager._debug = _show_envionment_manager_debug;
        foreach (var agent in _agents) {
          agent._debug = _show_agents_debug;
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
      }
      
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}