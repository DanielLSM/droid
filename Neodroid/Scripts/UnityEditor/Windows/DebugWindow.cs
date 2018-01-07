using Assets.Neodroid.Models.Actors;
using Neodroid.Configurables;
using Neodroid.Environments;
using Neodroid.Evaluation;
using Neodroid.Managers;
using Neodroid.Motors;
using Neodroid.Observers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class DebugWindow : EditorWindow {
    private Actor[] _actors;

    private ConfigurableGameObject[] _configurables;

    private LearningEnvironment[] _environments;

    private Texture _icon;

    private Motor[] _motors;

    private ObjectiveFunction[] _objective_functions;

    private Observer[] _observers;
    private bool _show_actors_debug;
    private bool _show_configurables_debug;
    private bool _show_environments_debug;
    private bool _show_motors_debug;
    private bool _show_objective_functions_debug;
    private bool _show_observers_debug;
    private bool _show_simulation_manager_debug;

    private SimulationManager _simulation_manager;

    [MenuItem("Neodroid/DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _simulation_manager = FindObjectOfType<SimulationManager>();
      _environments = FindObjectsOfType<LearningEnvironment>();
      _actors = FindObjectsOfType<Actor>();
      _motors = FindObjectsOfType<Motor>();
      _observers = FindObjectsOfType<Observer>();
      _configurables = FindObjectsOfType<ConfigurableGameObject>();
      _objective_functions = FindObjectsOfType<ObjectiveFunction>();
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/information.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Debug",
                                    _icon,
                                    "Window for controlling debug messages");
    }

    private void OnGUI() {
      _simulation_manager = FindObjectOfType<SimulationManager>();
      _environments = FindObjectsOfType<LearningEnvironment>();
      _actors = FindObjectsOfType<Actor>();
      _motors = FindObjectsOfType<Motor>();
      _observers = FindObjectsOfType<Observer>();
      _configurables = FindObjectsOfType<ConfigurableGameObject>();
      _objective_functions = FindObjectsOfType<ObjectiveFunction>();

      _show_simulation_manager_debug =
        EditorGUILayout.Toggle(
                               "Debug simulation manager",
                               _show_simulation_manager_debug);
      _show_environments_debug =
        EditorGUILayout.Toggle(
                               "Debug all environments",
                               _show_environments_debug);
      _show_actors_debug = EditorGUILayout.Toggle(
                                                  "Debug all actors",
                                                  _show_actors_debug);
      _show_motors_debug = EditorGUILayout.Toggle(
                                                  "Debug all motors",
                                                  _show_motors_debug);
      _show_observers_debug = EditorGUILayout.Toggle(
                                                     "Debug all observers",
                                                     _show_observers_debug);
      _show_configurables_debug =
        EditorGUILayout.Toggle(
                               "Debug all configurables",
                               _show_configurables_debug);
      _show_objective_functions_debug = EditorGUILayout.Toggle(
                                                               "Debug all objective functions",
                                                               _show_objective_functions_debug);

      if (GUILayout.Button("Apply")) {
        if (_simulation_manager != null)
          _simulation_manager.Debugging = _show_simulation_manager_debug;
        foreach (var environment in _environments) environment.Debugging = _show_environments_debug;
        foreach (var actor in _actors) actor.Debugging = _show_actors_debug;
        foreach (var motor in _motors) motor.Debugging = _show_motors_debug;
        foreach (var observer in _observers) observer.Debugging = _show_observers_debug;
        foreach (var configurable in _configurables)
          configurable.Debugging = _show_configurables_debug;
        foreach (var objective_functions in _objective_functions)
          objective_functions.Debugging = _show_objective_functions_debug;
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and 
      // are not saved unless other changes are made from a working inpector window 
      }*/
    }

    public void OnInspectorUpdate() { Repaint(); }
  }

  #endif
}
