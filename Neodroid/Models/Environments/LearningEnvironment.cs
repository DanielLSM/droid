using System.Collections;
using System.Collections.Generic;
using Assets.Neodroid.Models.Actors;
using Neodroid.Configurables;
using Neodroid.Evaluation;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Observers;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Environments {
  public class LearningEnvironment : MonoBehaviour,
                                     IHasRegister<Actor>,
                                     IHasRegister<Observer>,
                                     IHasRegister<ConfigurableGameObject>,
                                     IHasRegister<Resetable> {
    #region UnityCallbacks

    private void Start() {
      if (!_simulation_manager) _simulation_manager = FindObjectOfType<SimulationManager>();
      if (!_objective_function) _objective_function = FindObjectOfType<ObjectiveFunction>();
      _simulation_manager = NeodroidUtilities.MaybeRegisterComponent(
                                                                     _simulation_manager,
                                                                     this);
      SaveInitialPoses();
      StartCoroutine(SaveInitialBodiesIE());
    }

    #endregion

    #region Fields

    [Header(
      "References",
      order = 99)]
    [SerializeField]
    private ObjectiveFunction _objective_function;

    [SerializeField]
    private SimulationManager _simulation_manager;

    [Header(
      "Development",
      order = 100)]
    [SerializeField]
    private bool _debugging;

    [Header(
      "General",
      order = 101)]
    [SerializeField]
    private Transform _coordinate_reference_point;

    [SerializeField]
    private CoordinateSystem _coordinate_system = CoordinateSystem.LocalCoordinates;

    [SerializeField]
    private int _episode_length = 1000;

    [Header(
      "(Optional)",
      order = 102)]
    [SerializeField]
    private BoundingBox _playable_area;

    #endregion

    #region PrivateMembers

    private Vector3[] _reset_positions;
    private Quaternion[] _reset_rotations;
    private GameObject[] _child_game_objects;
    private Vector3[] _reset_velocities;
    private Vector3[] _reset_angulars;
    private Rigidbody[] _bodies;
    private Transform[] _poses;
    private Pose[] _received_poses;
    private Body[] _received_bodies;
    private Configuration[] _configurations;

    private readonly Dictionary<string, Resetable> _resetables = new Dictionary<string, Resetable>();
    private readonly Dictionary<string, Actor> _actors = new Dictionary<string, Actor>();
    private readonly Dictionary<string, Observer> _observers = new Dictionary<string, Observer>();

    private readonly Dictionary<string, ConfigurableGameObject> _configurables =
      new Dictionary<string, ConfigurableGameObject>();

    private float _lastest_reset_time;
    private float energy_spent;
    private bool _terminated;
    private bool _configure;
    private bool _describe;
    private bool _terminable = true;

    public LearningEnvironment() { CurrentFrameNumber = 0; }

    #endregion

    #region PublicMethods

    #region Getters

    public Dictionary<string, Actor> Actors { get { return _actors; } }

    public Dictionary<string, Observer> Observers { get { return _observers; } }

    public int EpisodeLength { get { return _episode_length; } set { _episode_length = value; } }

    public Dictionary<string, ConfigurableGameObject> Configurables { get { return _configurables; } }

    public Dictionary<string, Resetable> Resetables { get { return _resetables; } }

    public string EnvironmentIdentifier { get { return name; } }

    public int CurrentFrameNumber { get; private set; }

    public float GetTimeSinceReset() {
      return Time.time - _lastest_reset_time; //Time.realtimeSinceStartup;
    }

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    public SimulationManager SimulationManager {
      get { return _simulation_manager; }
      set { _simulation_manager = value; }
    }

    public ObjectiveFunction ObjectiveFunction {
      get { return _objective_function; }
      set { _objective_function = value; }
    }

    public BoundingBox PlayableArea { get { return _playable_area; } set { _playable_area = value; } }

    public Transform CoordinateReferencePoint {
      get { return _coordinate_reference_point; }
      set { _coordinate_reference_point = value; }
    }

    public CoordinateSystem CoordinateSystem {
      get { return _coordinate_system; }
      set { _coordinate_system = value; }
    }

    #endregion

    public void Terminate(string reason) {
      if (_terminable) {
        if (Debugging)
          print(
                string.Format(
                              "Was interrupted, because {0}",
                              reason));
        _terminated = true;
      }
    }

    public void PostUpdate() {
      if (_terminated) {
        _terminated = false;
        Reset();
        UpdateConfigurableValues();
      }

      if (_configure) {
        _configure = false;
        Configure();
        UpdateConfigurableValues();
      }

      UpdateObserversData();
    }

    public void UpdateObserversData() {
      foreach (var obs in Observers.Values)
        if (obs)
          obs.UpdateData();
    }

    public void UpdateConfigurableValues() {
      foreach (var con in Configurables.Values)
        if (con)
          con.UpdateObservation();
    }

    public EnvironmentState React(Reaction reaction) {
      if (reaction.Parameters.IsExternal) {
        _configurations = reaction.Configurations;
        _configure = reaction.Parameters.Configure;
        _describe = reaction.Parameters.Describe;
        _terminable = reaction.Parameters.Terminable;
        if (_configure && reaction.Unobservables != null) {
          _received_poses = reaction.Unobservables.Poses;
          _received_bodies = reaction.Unobservables.Bodies;
        }
      }

      if (reaction.Parameters.Step)
        Step(reaction);
      else if (reaction.Parameters.Reset)
        Terminate("Resetting because of reaction");
      return GetState();
    }

    #region Registration

    public void Register(Actor actor) {
      if (!Actors.ContainsKey(actor.ActorIdentifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered actor {1}",
                                  name,
                                  actor.ActorIdentifier));
        Actors.Add(
                   actor.ActorIdentifier,
                   actor);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has actor {1} registered",
                                  name,
                                  actor.ActorIdentifier));
      }
    }

    public void Register(Actor actor, string identifier) {
      if (!Actors.ContainsKey(identifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered actor {1}",
                                  name,
                                  identifier));
        Actors.Add(
                   identifier,
                   actor);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has actor {1} registered",
                                  name,
                                  identifier));
      }
    }

    public void Register(Observer observer) {
      if (!_observers.ContainsKey(observer.ObserverIdentifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered observer {1}",
                                  name,
                                  observer.ObserverIdentifier));
        _observers.Add(
                       observer.ObserverIdentifier,
                       observer);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has observer {1} registered",
                                  name,
                                  observer.ObserverIdentifier));
      }
    }

    public void Register(Observer observer, string identifier) {
      if (!_observers.ContainsKey(identifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered observer {1}",
                                  name,
                                  identifier));
        _observers.Add(
                       identifier,
                       observer);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has observer {1} registered",
                                  name,
                                  identifier));
      }
    }

    public void Register(ConfigurableGameObject configurable) {
      if (!_configurables.ContainsKey(configurable.ConfigurableIdentifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered configurable {1}",
                                  name,
                                  configurable.ConfigurableIdentifier));
        _configurables.Add(
                           configurable.ConfigurableIdentifier,
                           configurable);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has configurable {1} registered",
                                  name,
                                  configurable.ConfigurableIdentifier));
      }
    }

    public void Register(ConfigurableGameObject configurable, string identifier) {
      if (!_configurables.ContainsKey(identifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered configurable {1}",
                                  name,
                                  identifier));
        _configurables.Add(
                           identifier,
                           configurable);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has configurable {1} registered",
                                  name,
                                  identifier));
      }
    }

    public void Register(Resetable resetable, string identifier) {
      if (!_resetables.ContainsKey(identifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered resetables {1}",
                                  name,
                                  identifier));
        _resetables.Add(
                        identifier,
                        resetable);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has configurable {1} registered",
                                  name,
                                  identifier));
      }
    }

    public void Register(Resetable resetable) {
      if (!_resetables.ContainsKey(resetable.ResetableIdentifier)) {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} has registered resetables {1}",
                                  name,
                                  resetable.ResetableIdentifier));
        _resetables.Add(
                        resetable.ResetableIdentifier,
                        resetable);
      } else {
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} already has configurable {1} registered",
                                  name,
                                  resetable.ResetableIdentifier));
      }
    }

    public void UnRegisterActor(string identifier) {
      if (Actors.ContainsKey(identifier))
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} unregistered actor {1}",
                                  name,
                                  identifier));
      Actors.Remove(identifier);
    }

    public void UnRegisterObserver(string identifier) {
      if (_observers.ContainsKey(identifier))
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} unregistered observer {1}",
                                  name,
                                  identifier));
      _observers.Remove(identifier);
    }

    public void UnRegisterConfigurable(string identifier) {
      if (_configurables.ContainsKey(identifier))
        if (Debugging)
          Debug.Log(
                    string.Format(
                                  "Environment {0} unregistered configurable {1}",
                                  name,
                                  identifier));
      _configurables.Remove(identifier);
    }

    #endregion

    #region Transformations

    public Vector3 TransformPosition(Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
        if (_coordinate_reference_point)
          return _coordinate_reference_point.transform.InverseTransformPoint(position);
        else
          return position;
      if (_coordinate_system == CoordinateSystem.LocalCoordinates)
        return position - transform.position;
      return position;
    }

    public Vector3 InverseTransformPosition(Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
        if (_coordinate_reference_point)
          return _coordinate_reference_point.transform.TransformPoint(position);
        else
          return position;
      if (_coordinate_system == CoordinateSystem.LocalCoordinates)
        return position - transform.position;
      return position;
    }

    public Vector3 TransformDirection(Vector3 direction) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
        if (_coordinate_reference_point)
          return _coordinate_reference_point.transform.InverseTransformDirection(direction);
        else
          return direction;
      if (_coordinate_system == CoordinateSystem.LocalCoordinates)
        return transform.InverseTransformDirection(direction);
      return direction;
    }

    public Vector3 InverseTransformDirection(Vector3 direction) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
        if (_coordinate_reference_point)
          return _coordinate_reference_point.transform.TransformDirection(direction);
        else
          return direction;
      if (_coordinate_system == CoordinateSystem.LocalCoordinates)
        return transform.InverseTransformDirection(direction);
      return direction;
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void SaveInitialPoses() {
      var _ignored_layer = LayerMask.NameToLayer("IgnoredByNeodroid");
      _child_game_objects =
        NeodroidUtilities.RecursiveChildGameObjectsExceptLayer(
                                                               transform,
                                                               _ignored_layer);
      _reset_positions = new Vector3[_child_game_objects.Length];
      _reset_rotations = new Quaternion[_child_game_objects.Length];
      _poses = new Transform[_child_game_objects.Length];
      for (var i = 0; i < _child_game_objects.Length; i++) {
        _reset_positions[i] = _child_game_objects[i].transform.position;
        _reset_rotations[i] = _child_game_objects[i].transform.rotation;
        _poses[i] = _child_game_objects[i].transform;
        var maybe_joint = _child_game_objects[i].GetComponent<Joint>();
        if (maybe_joint != null) maybe_joint.gameObject.AddComponent<JointFix>();
      }
    }

    private void SaveInitialBodies() {
      var body_list = new List<Rigidbody>();
      foreach (var go in _child_game_objects) {
        var body = go.GetComponent<Rigidbody>();
        if (body) body_list.Add(body);
      }

      _bodies = body_list.ToArray();
      _reset_velocities = new Vector3[_bodies.Length];
      _reset_angulars = new Vector3[_bodies.Length];
      for (var i = 0; i < _bodies.Length; i++) {
        _reset_velocities[i] = _bodies[i].velocity;
        _reset_angulars[i] = _bodies[i].angularVelocity;
      }
    }

    private IEnumerator SaveInitialBodiesIE() {
      yield return new WaitForFixedUpdate();
      SaveInitialBodies();
    }

    private EnvironmentState GetState() {
      foreach (var a in Actors.Values)
        foreach (var m in a.Motors.Values)
          energy_spent += m.GetEnergySpend();

      var reward = 0f;
      if (_objective_function != null) reward = _objective_function.Evaluate();

      EnvironmentDescription description = null;
      if (_describe) {
        var threshold = 0f;
        if (_objective_function != null) threshold = _objective_function.SolvedThreshold;
        description = new EnvironmentDescription(
                                                 EpisodeLength,
                                                 _simulation_manager.FrameSkips,
                                                 Actors,
                                                 Configurables,
                                                 threshold
                                                );
        _describe = false;
      }

      return new EnvironmentState(
                                  EnvironmentIdentifier,
                                  energy_spent,
                                  Observers,
                                  CurrentFrameNumber,
                                  reward,
                                  _terminated,
                                  _bodies,
                                  _poses,
                                  description
                                 );
    }

    private void Reset() {
      ResetRegisteredObjects();
      SetEnvironmentPoses(
                          _child_game_objects,
                          _reset_positions,
                          _reset_rotations);
      SetEnvironmentBodies(
                           _bodies,
                           _reset_velocities,
                           _reset_angulars);
    }

    private void Configure() {
      if (_received_poses != null) {
        var positions = new Vector3[_received_poses.Length];
        var rotations = new Quaternion[_received_poses.Length];
        for (var i = 0; i < _received_poses.Length; i++) {
          positions[i] = _received_poses[i].position;
          rotations[i] = _received_poses[i].rotation;
        }

        SetEnvironmentPoses(
                            _child_game_objects,
                            positions,
                            rotations);
      }

      if (_received_bodies != null) {
        var vels = new Vector3[_received_bodies.Length];
        var angs = new Vector3[_received_bodies.Length];
        for (var i = 0; i < _received_bodies.Length; i++) {
          vels[i] = _received_bodies[i].velocity;
          angs[i] = _received_bodies[i].angularVelocity;
        }

        SetEnvironmentBodies(
                             _bodies,
                             vels,
                             angs);
      }

      if (_configurations != null)
        foreach (var configuration in _configurations)
          if (_configurables.ContainsKey(configuration.ConfigurableName)) {
            _configurables[configuration.ConfigurableName].ApplyConfiguration(configuration);
          } else {
            if (Debugging)
              Debug.Log(
                        "Could find not configurable with the specified name: "
                        + configuration.ConfigurableName);
          }
    }

    private void Step(Reaction reaction) {
      if (reaction.Parameters.EpisodeCount) CurrentFrameNumber++;
      if (reaction != null && reaction.Motions != null && reaction.Motions.Length > 0)
        foreach (var motion in reaction.Motions) {
          if (Debugging)
            Debug.Log("Applying " + motion + " To " + name + "'s actors");
          var motion_actor_name = motion.GetActorName();
          if (Actors.ContainsKey(motion_actor_name) && Actors[motion_actor_name] != null) {
            Actors[motion_actor_name].ApplyMotion(motion);
          } else {
            if (Debugging)
              Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
          }
        }

      if (EpisodeLength > 0 && CurrentFrameNumber > EpisodeLength) {
        if (Debugging)
          Debug.Log("Maximum episode length reached, resetting");
        Terminate("Maximum episode length reached, resetting");
      }

      UpdateObserversData();
    }

    private void ResetRegisteredObjects() {
      if (Debugging)
        Debug.Log("Resetting registed objects");
      foreach (var resetable in Resetables.Values)
        if (resetable != null)
          resetable.Reset();
      foreach (var actor in Actors.Values)
        if (actor)
          actor.Reset();
      foreach (var observer in Observers.Values)
        if (observer)
          observer.Reset();
    }

    #region EnvironmentStateSetters

    private void SetEnvironmentPoses(
      GameObject[] child_game_objects,
      Vector3[] positions,
      Quaternion[] rotations) {
      if (_simulation_manager) {
        for (var iterations = 0; iterations < _simulation_manager.ResetIterations; iterations++)
          for (var i = 0; i < child_game_objects.Length; i++)
            if (child_game_objects[i] != null && i < positions.Length && i < rotations.Length) {
              var rigid_body = child_game_objects[i].GetComponent<Rigidbody>();
              if (rigid_body)
                rigid_body.Sleep();
              child_game_objects[i].transform.position = positions[i];
              child_game_objects[i].transform.rotation = rotations[i];
              if (rigid_body)
                rigid_body.WakeUp();

              var joint_fix = child_game_objects[i].GetComponent<JointFix>();
              if (joint_fix)
                joint_fix.Reset();
              var anim = child_game_objects[i].GetComponent<Animation>();
              if (anim)
                anim.Rewind();
            }

        _lastest_reset_time = Time.time;
        CurrentFrameNumber = 0;
        if (_objective_function) _objective_function.Reset();
      }
    }

    private void SetEnvironmentBodies(Rigidbody[] bodies, Vector3[] velocities, Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0)
        for (var i = 0; i < bodies.Length; i++)
          if (i < bodies.Length && bodies[i] != null && i < velocities.Length && i < angulars.Length) {
            if (Debugging)
              print(
                    string.Format(
                                  "Setting {0}, velocity to {1} and angular velocity to {2}",
                                  bodies[i].name,
                                  velocities[i],
                                  angulars[i]));
            bodies[i].Sleep();
            bodies[i].velocity = velocities[i];
            bodies[i].angularVelocity = angulars[i];
            bodies[i].WakeUp();
          }
    }

    #endregion

    #endregion
  }
}
