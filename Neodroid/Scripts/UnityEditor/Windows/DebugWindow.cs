using Neodroid.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Environments;
using Neodroid.Models.Managers;
using Neodroid.Models.Motors.General;
using Neodroid.Models.Observers.General;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class DebugWindow : EditorWindow {
    Actor[] _actors;

    ConfigurableGameObject[] _configurables;

    LearningEnvironment[] _environments;

    Texture _icon;

    Motor[] _motors;

    ObjectiveFunction[] _objective_functions;

    Observer[] _observers;
    bool _show_actors_debug;
    bool _show_configurables_debug;
    bool _show_environments_debug;
    bool _show_motors_debug;
    bool _show_objective_functions_debug;
    bool _show_observers_debug;
    bool _show_simulation_manager_debug;

    SimulationManager _simulation_manager;

    [MenuItem("Neodroid/DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._simulation_manager = FindObjectOfType<SimulationManager>();
      this._environments = FindObjectsOfType<LearningEnvironment>();
      this._actors = FindObjectsOfType<Actor>();
      this._motors = FindObjectsOfType<Motor>();
      this._observers = FindObjectsOfType<Observer>();
      this._configurables = FindObjectsOfType<ConfigurableGameObject>();
      this._objective_functions = FindObjectsOfType<ObjectiveFunction>();
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/information.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Debug", this._icon, "Window for controlling debug messages");
    }

    void OnGUI() {
      this._simulation_manager = FindObjectOfType<SimulationManager>();
      this._environments = FindObjectsOfType<LearningEnvironment>();
      this._actors = FindObjectsOfType<Actor>();
      this._motors = FindObjectsOfType<Motor>();
      this._observers = FindObjectsOfType<Observer>();
      this._configurables = FindObjectsOfType<ConfigurableGameObject>();
      this._objective_functions = FindObjectsOfType<ObjectiveFunction>();

      this._show_simulation_manager_debug = EditorGUILayout.Toggle(
          "Debug simulation manager",
          this._show_simulation_manager_debug);
      this._show_environments_debug = EditorGUILayout.Toggle(
          "Debug all environments",
          this._show_environments_debug);
      this._show_actors_debug = EditorGUILayout.Toggle("Debug all actors", this._show_actors_debug);
      this._show_motors_debug = EditorGUILayout.Toggle("Debug all motors", this._show_motors_debug);
      this._show_observers_debug = EditorGUILayout.Toggle("Debug all observers", this._show_observers_debug);
      this._show_configurables_debug = EditorGUILayout.Toggle(
          "Debug all configurables",
          this._show_configurables_debug);
      this._show_objective_functions_debug = EditorGUILayout.Toggle(
          "Debug all objective functions",
          this._show_objective_functions_debug);

      if (GUILayout.Button("Apply")) {
        if (this._simulation_manager != null)
          this._simulation_manager.Debugging = this._show_simulation_manager_debug;
        foreach (var environment in this._environments) environment.Debugging = this._show_environments_debug;
        foreach (var actor in this._actors) actor.Debugging = this._show_actors_debug;
        foreach (var motor in this._motors) motor.Debugging = this._show_motors_debug;
        foreach (var observer in this._observers) observer.Debugging = this._show_observers_debug;
        foreach (var configurable in this._configurables)
          configurable.Debugging = this._show_configurables_debug;
        foreach (var objective_functions in this._objective_functions)
          objective_functions.Debugging = this._show_objective_functions_debug;
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }

  #endif
}
