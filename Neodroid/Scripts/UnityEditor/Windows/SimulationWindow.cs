using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Evaluation;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Managers;
using Neodroid.Models.Motors.General;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class SimulationWindow : EditorWindow {
    readonly int _preview_image_size = 100;
    Dictionary<string, Actor> _actors;
    Dictionary<string, ConfigurableGameObject> _configurables;
    LearningEnvironment[] _environments;
    Texture _icon;
    Dictionary<string, Motor> _motors;
    Texture _neodroid_icon;
    Dictionary<string, Observer> _observers;
    Dictionary<string, Resetable> _resetables;
    Vector2 _scroll_position;
    bool[] _show_environment_properties = new bool[1];

    SimulationManager _simulation_manager;

    [MenuItem(itemName : "Neodroid/SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(
                t : typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                            assetPath : "Assets/Neodroid/Icons/world.png",
                                                            type : typeof(Texture2D));
      this._neodroid_icon =
        (Texture)AssetDatabase.LoadAssetAtPath(
                                               assetPath : "Assets/Neodroid/Icons/neodroid_favicon_cut.png",
                                               type : typeof(Texture));
      this.titleContent = new GUIContent(
                                         text : "Neo:Sim",
                                         image : this._icon,
                                         tooltip : "Window for configuring simulation");
      this.Setup();
    }

    void Setup() {
      if (this._environments != null) this._show_environment_properties = new bool[this._environments.Length];
    }

    void OnGUI() {
      var serialised_object = new SerializedObject(obj : this);
      this._simulation_manager = FindObjectOfType<SimulationManager>();
      if (this._simulation_manager) {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(
                        this._neodroid_icon,
                        GUILayout.Width(width : this._preview_image_size),
                        GUILayout.Height(height : this._preview_image_size));

        EditorGUILayout.BeginVertical();
        this._simulation_manager.FrameSkips =
          EditorGUILayout.IntField(
                                   label : "Frame Skips",
                                   value : this._simulation_manager.FrameSkips);
        this._simulation_manager.ResetIterations = EditorGUILayout.IntField(
                                                                            label : "Reset Iterations",
                                                                            value : this
                                                                                      ._simulation_manager
                                                                                      .ResetIterations);
        this._simulation_manager.WaitEvery =
          (WaitOn)EditorGUILayout.EnumPopup(
                                            label : "Wait Every",
                                            selected : this._simulation_manager.WaitEvery);
        this._simulation_manager.TestMotors =
          EditorGUILayout.Toggle(
                                 label : "Test Motors",
                                 value : this._simulation_manager.TestMotors);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this._environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<LearningEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) this.Setup();

        this._scroll_position = EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);

        EditorGUILayout.BeginVertical(style : "Box");
        GUILayout.Label(text : "Environments");
        if (this._show_environment_properties != null) {
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            this._show_environment_properties[i] = EditorGUILayout.Foldout(
                                                                           foldout : this
                                                                             ._show_environment_properties[i],
                                                                           content : this
                                                                                     ._environments[i]
                                                                                     .EnvironmentIdentifier);
            if (this._show_environment_properties[i]) {
              this._actors = this._environments[i].Actors;
              this._observers = this._environments[i].Observers;
              this._configurables = this._environments[i].Configurables;
              this._resetables = this._environments[i].Resetables;

              EditorGUILayout.BeginVertical(style : "Box");
              this._environments[i].enabled = EditorGUILayout.BeginToggleGroup(
                                                                               label : this._environments[i]
                                                                                           .EnvironmentIdentifier,
                                                                               toggle : this._environments[i]
                                                                                            .enabled
                                                                                        && this
                                                                                           ._environments[i]
                                                                                           .gameObject
                                                                                           .activeSelf);
              EditorGUILayout.ObjectField(
                                          obj : this._environments[i],
                                          objType : typeof(LearningEnvironment),
                                          allowSceneObjects : true);
              this._environments[i].CoordinateSystem = (CoordinateSystem)EditorGUILayout.EnumPopup(
                                                                                                   label :
                                                                                                   "Coordinate system",
                                                                                                   selected :
                                                                                                   this
                                                                                                     ._environments
                                                                                                       [i]
                                                                                                     .CoordinateSystem);
              EditorGUI.BeginDisabledGroup(
                                           disabled : this._environments[i].CoordinateSystem
                                                      != CoordinateSystem.RelativeToReferencePoint);
              this._environments[i].CoordinateReferencePoint =
                (Transform)EditorGUILayout.ObjectField(
                                                       label : "Reference point",
                                                       obj : this._environments[i].CoordinateReferencePoint,
                                                       objType : typeof(Transform),
                                                       allowSceneObjects : true);
              EditorGUI.EndDisabledGroup();
              this._environments[i].ObjectiveFunction =
                (ObjectiveFunction)EditorGUILayout.ObjectField(
                                                               label : "Objective function",
                                                               obj : this._environments[i].ObjectiveFunction,
                                                               objType : typeof(ObjectiveFunction),
                                                               allowSceneObjects : true);
              this._environments[i].EpisodeLength = EditorGUILayout.IntField(
                                                                             label : "Episode Length",
                                                                             value : this
                                                                                     ._environments[i]
                                                                                     .EpisodeLength);

              EditorGUILayout.BeginVertical(style : "Box");
              GUILayout.Label(text : "Actors");
              foreach (var actor in this._actors)
                if (actor.Value != null) {
                  this._motors = actor.Value.Motors;

                  EditorGUILayout.BeginVertical(style : "Box");
                  actor.Value.enabled = EditorGUILayout.BeginToggleGroup(
                                                                         label : actor.Key,
                                                                         toggle : actor.Value.enabled
                                                                                  && actor.Value.gameObject
                                                                                          .activeSelf);
                  EditorGUILayout.ObjectField(
                                              obj : actor.Value,
                                              objType : typeof(Actor),
                                              allowSceneObjects : true);

                  EditorGUILayout.BeginVertical(style : "Box");
                  GUILayout.Label(text : "Motors");
                  foreach (var motor in this._motors)
                    if (motor.Value != null) {
                      EditorGUILayout.BeginVertical(style : "Box");
                      motor.Value.enabled = EditorGUILayout.BeginToggleGroup(
                                                                             label : motor.Key,
                                                                             toggle : motor.Value.enabled
                                                                                      && motor
                                                                                         .Value.gameObject
                                                                                         .activeSelf);
                      EditorGUILayout.ObjectField(
                                                  obj : motor.Value,
                                                  objType : typeof(Motor),
                                                  allowSceneObjects : true);
                      EditorGUILayout.EndToggleGroup();

                      EditorGUILayout.EndVertical();
                    }

                  EditorGUILayout.EndVertical();

                  EditorGUILayout.EndToggleGroup();

                  EditorGUILayout.EndVertical();
                }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical(style : "Box");
              GUILayout.Label(text : "Observers");
              foreach (var observer in this._observers)
                if (observer.Value != null) {
                  EditorGUILayout.BeginVertical(style : "Box");
                  observer.Value.enabled = EditorGUILayout.BeginToggleGroup(
                                                                            label : observer.Key,
                                                                            toggle : observer.Value.enabled
                                                                                     && observer
                                                                                        .Value.gameObject
                                                                                        .activeSelf);
                  EditorGUILayout.ObjectField(
                                              obj : observer.Value,
                                              objType : typeof(Observer),
                                              allowSceneObjects : true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical(style : "Box");
              GUILayout.Label(text : "Configurables");
              foreach (var configurable in this._configurables)
                if (configurable.Value != null) {
                  EditorGUILayout.BeginVertical(style : "Box");
                  configurable.Value.enabled =
                    EditorGUILayout.BeginToggleGroup(
                                                     label : configurable.Key,
                                                     toggle : configurable.Value.enabled
                                                              && configurable.Value.gameObject.activeSelf);
                  EditorGUILayout.ObjectField(
                                              obj : configurable.Value,
                                              objType : typeof(ConfigurableGameObject),
                                              allowSceneObjects : true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }

              EditorGUILayout.EndVertical();

              EditorGUILayout.BeginVertical(style : "Box");
              GUILayout.Label(text : "Resetables");
              foreach (var resetable in this._resetables)
                if (resetable.Value != null) {
                  EditorGUILayout.BeginVertical(style : "Box");
                  resetable.Value.enabled = EditorGUILayout.BeginToggleGroup(
                                                                             label : resetable.Key,
                                                                             toggle : resetable.Value.enabled
                                                                                      && resetable
                                                                                         .Value.gameObject
                                                                                         .activeSelf);
                  EditorGUILayout.ObjectField(
                                              obj : resetable.Value,
                                              objType : typeof(Resetable),
                                              allowSceneObjects : true);
                  EditorGUILayout.EndToggleGroup();
                  EditorGUILayout.EndVertical();
                }

              EditorGUILayout.EndVertical();

              EditorGUILayout.EndToggleGroup();
              EditorGUILayout.EndVertical();
            }
          }

          EditorGUILayout.EndVertical();

          EditorGUILayout.EndScrollView();
          serialised_object.ApplyModifiedProperties();

          if (GUILayout.Button(text : "Refresh")) this.Refresh();

          EditorGUI.BeginDisabledGroup(disabled : !Application.isPlaying);

          if (GUILayout.Button(text : "Step"))
            this._simulation_manager.ReactInEnvironments(
                                                         reaction : new Reaction(
                                                                                 parameters : new
                                                                                   ReactionParameters(
                                                                                                      terminable
                                                                                                      : true,
                                                                                                      step :
                                                                                                      true,
                                                                                                      reset :
                                                                                                      false,
                                                                                                      configure
                                                                                                      : false,
                                                                                                      describe
                                                                                                      : false),
                                                                                 motions : null,
                                                                                 configurations : null,
                                                                                 unobservables : null));

          if (GUILayout.Button(text : "Reset"))
            this._simulation_manager.ReactInEnvironments(
                                                         reaction : new Reaction(
                                                                                 parameters : new
                                                                                   ReactionParameters(
                                                                                                      terminable
                                                                                                      : true,
                                                                                                      step :
                                                                                                      false,
                                                                                                      reset :
                                                                                                      true,
                                                                                                      configure
                                                                                                      : false,
                                                                                                      describe
                                                                                                      : false),
                                                                                 motions : null,
                                                                                 configurations : null,
                                                                                 unobservables : null));

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
