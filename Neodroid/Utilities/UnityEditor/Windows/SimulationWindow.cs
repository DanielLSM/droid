using System.Collections.Generic;
using Neodroid.Environments.General;
using Neodroid.Managers;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Environments;
using Neodroid.Models.Motors.General;
using Neodroid.Prototyping.Evaluation.General;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Enums;
using Neodroid.Utilities.ScriptableObjects;

using UnityEngine;

#if UNITY_EDITOR
  using UnityEditor;
#endif

namespace Neodroid.Utilities.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class SimulationWindow : EditorWindow {
    readonly int _preview_image_size = 100;
    Dictionary<string, Actor> _actors;
    Dictionary<string, ConfigurableGameObject> _configurables;
    PrototypingEnvironment[] _environments;
    Texture _icon;
    Dictionary<string, Motor> _motors;
    Texture _neodroid_icon;
    Dictionary<string, Observer> _observers;
    Dictionary<string, Resetable> _resetables;
    Vector2 _scroll_position;
    bool[] _show_environment_properties = new bool[1];

    PausableManager _time_simulation_manager;

    [MenuItem("Neodroid/SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/world.png",
          typeof(Texture2D));
      this._neodroid_icon = (Texture)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/neodroid_favicon_cut.png",
          typeof(Texture));
      this.titleContent = new GUIContent("Neo:Sim", this._icon, "Window for configuring simulation");
      this.Setup();
    }

    void Setup() {
      if (this._environments != null) this._show_environment_properties = new bool[this._environments.Length];
    }

    void OnGUI() {
      var serialised_object = new SerializedObject(this);
      this._time_simulation_manager = FindObjectOfType<PausableManager>();
      if (this._time_simulation_manager) {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(
            this._neodroid_icon,
            GUILayout.Width(this._preview_image_size),
            GUILayout.Height(this._preview_image_size));

        EditorGUILayout.BeginVertical();
        this._time_simulation_manager.Configuration.FrameSkips = EditorGUILayout.IntField(
            "Frame Skips",
            this._time_simulation_manager.Configuration.FrameSkips);
        this._time_simulation_manager.Configuration.ResetIterations = EditorGUILayout.IntField(
            "Reset Iterations",
            this._time_simulation_manager.Configuration.ResetIterations);
        this._time_simulation_manager.Configuration.SimulationType =
            (SimulationType)EditorGUILayout.EnumPopup(
                "Simulation Type",
                this._time_simulation_manager.Configuration.SimulationType);
        this._time_simulation_manager.TestMotors = EditorGUILayout.Toggle(
            "Test Motors",
            this._time_simulation_manager.TestMotors);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this._environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<PrototypingEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) this.Setup();

        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Environments");
        if (this._show_environment_properties != null) {
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            this._show_environment_properties[i] = EditorGUILayout.Foldout(
                this._show_environment_properties[i],
                this._environments[i].Identifier);
            if (this._show_environment_properties[i]) {
              this._actors = this._environments[i].Actors;
              this._observers = this._environments[i].Observers;
              this._configurables = this._environments[i].Configurables;
              this._resetables = this._environments[i].Resetables;

              EditorGUILayout.BeginVertical("Box");
              this._environments[i].enabled = EditorGUILayout.BeginToggleGroup(
                  this._environments[i].Identifier,
                  this._environments[i].enabled && this._environments[i].gameObject.activeSelf);
              EditorGUILayout.ObjectField(this._environments[i], typeof(PrototypingEnvironment), true);
              this._environments[i].CoordinateSystem = (CoordinateSystem)EditorGUILayout.EnumPopup(
                  "Coordinate system",
                  this._environments[i].CoordinateSystem);
              EditorGUI.BeginDisabledGroup(
                  this._environments[i].CoordinateSystem != CoordinateSystem.RelativeToReferencePoint);
              this._environments[i].CoordinateReferencePoint = (Transform)EditorGUILayout.ObjectField(
                  "Reference point",
                  this._environments[i].CoordinateReferencePoint,
                  typeof(Transform),
                  true);
              EditorGUI.EndDisabledGroup();
              this._environments[i].ObjectiveFunction = (ObjectiveFunction)EditorGUILayout.ObjectField(
                  "Objective function",
                  this._environments[i].ObjectiveFunction,
                  typeof(ObjectiveFunction),
                  true);
              this._environments[i].EpisodeLength = EditorGUILayout.IntField(
                  "Episode Length",
                  this._environments[i].EpisodeLength);

              EditorGUILayout.BeginVertical("Box");
              GUILayout.Label("Actors");
              foreach (var actor in this._actors) {
                if (actor.Value != null) {
                  this._motors = actor.Value.Motors;

                  EditorGUILayout.BeginVertical("Box");
                  actor.Value.enabled = EditorGUILayout.BeginToggleGroup(
                      actor.Key,
                      actor.Value.enabled && actor.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField(actor.Value, typeof(Actor), true);

                  EditorGUILayout.BeginVertical("Box");
                  GUILayout.Label("Motors");
                  foreach (var motor in this._motors) {
                    if (motor.Value != null) {
                      EditorGUILayout.BeginVertical("Box");
                      motor.Value.enabled = EditorGUILayout.BeginToggleGroup(
                          motor.Key,
                          motor.Value.enabled && motor.Value.gameObject.activeSelf);
                      EditorGUILayout.ObjectField(motor.Value, typeof(Motor), true);
                      EditorGUILayout.EndToggleGroup();

                      EditorGUILayout.EndVertical();
                    }
                  }

                  EditorGUILayout.EndVertical();

                  EditorGUILayout.EndToggleGroup();

                  EditorGUILayout.EndVertical();
                }
              }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical("Box");
              GUILayout.Label("Observers");
              foreach (var observer in this._observers) {
                if (observer.Value != null) {
                  EditorGUILayout.BeginVertical("Box");
                  observer.Value.enabled = EditorGUILayout.BeginToggleGroup(
                      observer.Key,
                      observer.Value.enabled && observer.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField(observer.Value, typeof(Observer), true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }
              }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical("Box");
              GUILayout.Label("Configurables");
              foreach (var configurable in this._configurables) {
                if (configurable.Value != null) {
                  EditorGUILayout.BeginVertical("Box");
                  configurable.Value.enabled = EditorGUILayout.BeginToggleGroup(
                      configurable.Key,
                      configurable.Value.enabled && configurable.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField(configurable.Value, typeof(ConfigurableGameObject), true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }
              }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical("Box");
              GUILayout.Label("Resetables");
              foreach (var resetable in this._resetables) {
                if (resetable.Value != null) {
                  EditorGUILayout.BeginVertical("Box");
                  resetable.Value.enabled = EditorGUILayout.BeginToggleGroup(
                      resetable.Key,
                      resetable.Value.enabled && resetable.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField(resetable.Value, typeof(Resetable), true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }
              }

              EditorGUILayout.EndVertical();

              EditorGUILayout.EndToggleGroup();
              EditorGUILayout.EndVertical();
            }
          }

          EditorGUILayout.EndVertical();

          EditorGUILayout.EndScrollView();
          serialised_object.ApplyModifiedProperties();

          if (GUILayout.Button("Refresh")) this.Refresh();

          EditorGUI.BeginDisabledGroup(!Application.isPlaying);

          if (GUILayout.Button("Step")) {
            this._time_simulation_manager.React(
                new Reaction(new ReactionParameters(true, true, false, false, false), null, null, null));
          }

          if (GUILayout.Button("Reset")) {
            this._time_simulation_manager.React(
                new Reaction(new ReactionParameters(true, false, true, false, false), null, null, null));
          }

          EditorGUI.EndDisabledGroup();
        }
      }
    }

    void Refresh() {
      var actors = FindObjectsOfType<Actor>();
      foreach (var obj in actors) obj.RefreshAwake();
      var configurables = FindObjectsOfType<ConfigurableGameObject>();
      foreach (var obj in configurables) obj.RefreshAwake();
      var motors = FindObjectsOfType<Motor>();
      foreach (var obj in motors) obj.RefreshAwake();
      var observers = FindObjectsOfType<Observer>();
      foreach (var obj in observers) obj.RefreshAwake();
      foreach (var obj in actors) obj.RefreshStart();
      foreach (var obj in configurables) obj.RefreshStart();
      foreach (var obj in motors) obj.RefreshStart();
      foreach (var obj in observers) obj.RefreshStart();
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }

  #endif
}
