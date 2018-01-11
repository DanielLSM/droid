using System.Collections;
using System.Collections.Generic;
using Neodroid.Models.Environments;
using Neodroid.Models.Evaluation;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Managers;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using Neodroid.Scripts.Utilities.Enums;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Environments {
  public class LearningEnvironment : MonoBehaviour,
                                     IHasRegister<Actor>,
                                     IHasRegister<Observer>,
                                     IHasRegister<ConfigurableGameObject>,
                                     IHasRegister<Resetable> {
    #region UnityCallbacks

    void Start () {
      this.InnerPreStart();
      if (!this._simulation_manager)
        this._simulation_manager = FindObjectOfType<SimulationManager> ();
      if (!this._objective_function)
        this._objective_function = FindObjectOfType<ObjectiveFunction> ();
      this._simulation_manager = NeodroidUtilities.MaybeRegisterComponent (this._simulation_manager, this);
      this.SaveInitialPoses ();
      this.StartCoroutine (this.SaveInitialBodiesIE ());
    }

    protected virtual void InnerPreStart() {

    }

    #endregion

    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    ObjectiveFunction _objective_function;

    [SerializeField] SimulationManager _simulation_manager;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header ("General", order = 101)]
    [SerializeField]
    Transform _coordinate_reference_point;

    [SerializeField] CoordinateSystem _coordinate_system = CoordinateSystem.LocalCoordinates;

    [SerializeField] int _episode_length = 1000;

    [Header ("(Optional)", order = 102)]
    [SerializeField]
    BoundingBox _playable_area;

    #endregion

    #region PrivateMembers

    Vector3[] _reset_positions;
    Quaternion[] _reset_rotations;
    GameObject[] _child_game_objects;
    Vector3[] _reset_velocities;
    Vector3[] _reset_angulars;
    Rigidbody[] _bodies;
    Transform[] _poses;
    Pose[] _received_poses;
    Body[] _received_bodies;
    Configuration[] _configurations;

    readonly Dictionary<string, Resetable> _resetables = new Dictionary<string, Resetable> ();
    readonly Dictionary<string, Actor> _actors = new Dictionary<string, Actor> ();
    readonly Dictionary<string, Observer> _observers = new Dictionary<string, Observer> ();

    protected readonly Dictionary<string, ConfigurableGameObject> _configurables =
      new Dictionary<string, ConfigurableGameObject> ();

    float _lastest_reset_time;
    float energy_spent;
    protected bool _terminated;
    protected bool _configure;
    bool _describe;
    bool _terminable = true;

    public LearningEnvironment () {
      this.CurrentFrameNumber = 0;
    }

    #endregion

    #region PublicMethods

    #region Getters

    public Dictionary<string, Actor> Actors { get { return this._actors; } }

    public Dictionary<string, Observer> Observers { get { return this._observers; } }

    public int EpisodeLength { get { return this._episode_length; } set { this._episode_length = value; } }

    public Dictionary<string, ConfigurableGameObject> Configurables { get { return this._configurables; } }

    public Dictionary<string, Resetable> Resetables { get { return this._resetables; } }

    public string EnvironmentIdentifier { get { return this.name; } }

    public int CurrentFrameNumber { get; private set; }

    public float GetTimeSinceReset () {
      return Time.time - this._lastest_reset_time; //Time.realtimeSinceStartup;
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public SimulationManager SimulationManager {
      get { return this._simulation_manager; }
      set { this._simulation_manager = value; }
    }

    public ObjectiveFunction ObjectiveFunction {
      get { return this._objective_function; }
      set { this._objective_function = value; }
    }

    public BoundingBox PlayableArea {
      get { return this._playable_area; }
      set { this._playable_area = value; }
    }

    public Transform CoordinateReferencePoint {
      get { return this._coordinate_reference_point; }
      set { this._coordinate_reference_point = value; }
    }

    public CoordinateSystem CoordinateSystem {
      get { return this._coordinate_system; }
      set { this._coordinate_system = value; }
    }

    #endregion

    public void Terminate (string reason) {
      if (this._terminable) {
        if (this.Debugging)
          print (string.Format ("Was interrupted, because {0}", reason));
        this._terminated = true;
      }
    }

    public virtual void PostUpdate () {
      if (this._terminated) {
        this._terminated = false;
        this.Reset ();
      }

      if (this._configure) {
        this._configure = false;
        this.Configure ();
      }

      this.UpdateConfigurableValues ();
      this.UpdateObserversData ();
    }

    public void UpdateObserversData () {
      foreach (var obs in this.Observers.Values) {
        if (obs)
          obs.UpdateObservation ();
      }
    }

    public void UpdateConfigurableValues () {
      foreach (var con in this.Configurables.Values) {
        if (con)
          con.UpdateObservation ();
      }
    }

    public EnvironmentState React (Reaction reaction) {
      if (reaction.Parameters.IsExternal) {
        this._configurations = reaction.Configurations;
        this._configure = reaction.Parameters.Configure;
        this._describe = reaction.Parameters.Describe;
        this._terminable = reaction.Parameters.Terminable;
        if (this._configure && reaction.Unobservables != null) {
          this._received_poses = reaction.Unobservables.Poses;
          this._received_bodies = reaction.Unobservables.Bodies;
        }
      }

      if (reaction.Parameters.Step)
        this.Step (reaction);
      else if (reaction.Parameters.Reset)
        this.Terminate ("Resetting because of reaction");
      return this.GetState ();
    }

    #region Registration

    public void Register (Actor actor) {
      if (!this.Actors.ContainsKey (actor.ActorIdentifier)) {
        if (this.Debugging) {
          Debug.Log (
            string.Format ("Environment {0} has registered actor {1}", this.name, actor.ActorIdentifier));
        }

        this.Actors.Add (actor.ActorIdentifier, actor);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has actor {1} registered",
              this.name,
              actor.ActorIdentifier));
        }
      }
    }

    public void Register (Actor actor, string identifier) {
      if (!this.Actors.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} has registered actor {1}", this.name, identifier));
        this.Actors.Add (identifier, actor);
      } else {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} already has actor {1} registered", this.name, identifier));
      }
    }

    public void Register (Observer observer) {
      if (!this._observers.ContainsKey (observer.ObserverIdentifier)) {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} has registered observer {1}",
              this.name,
              observer.ObserverIdentifier));
        }

        this._observers.Add (observer.ObserverIdentifier, observer);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has observer {1} registered",
              this.name,
              observer.ObserverIdentifier));
        }
      }
    }

    public void Register (Observer observer, string identifier) {
      if (!this._observers.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} has registered observer {1}", this.name, identifier));
        this._observers.Add (identifier, observer);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format ("Environment {0} already has observer {1} registered", this.name, identifier));
        }
      }
    }

    public void Register (ConfigurableGameObject configurable) {
      if (!this._configurables.ContainsKey (configurable.ConfigurableIdentifier)) {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} has registered configurable {1}",
              this.name,
              configurable.ConfigurableIdentifier));
        }

        this._configurables.Add (configurable.ConfigurableIdentifier, configurable);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has configurable {1} registered",
              this.name,
              configurable.ConfigurableIdentifier));
        }
      }
    }

    public void Register (ConfigurableGameObject configurable, string identifier) {
      if (!this._configurables.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} has registered configurable {1}", this.name, identifier));
        this._configurables.Add (identifier, configurable);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has configurable {1} registered",
              this.name,
              identifier));
        }
      }
    }

    public void Register (Resetable resetable, string identifier) {
      if (!this._resetables.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} has registered resetables {1}", this.name, identifier));
        this._resetables.Add (identifier, resetable);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has configurable {1} registered",
              this.name,
              identifier));
        }
      }
    }

    public void Register (Resetable resetable) {
      if (!this._resetables.ContainsKey (resetable.ResetableIdentifier)) {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} has registered resetables {1}",
              this.name,
              resetable.ResetableIdentifier));
        }

        this._resetables.Add (resetable.ResetableIdentifier, resetable);
      } else {
        if (this.Debugging) {
          Debug.Log (
            string.Format (
              "Environment {0} already has configurable {1} registered",
              this.name,
              resetable.ResetableIdentifier));
        }
      }
    }

    public void UnRegisterActor (string identifier) {
      if (this.Actors.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} unregistered actor {1}", this.name, identifier));
      }

      this.Actors.Remove (identifier);
    }

    public void UnRegisterObserver (string identifier) {
      if (this._observers.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} unregistered observer {1}", this.name, identifier));
      }

      this._observers.Remove (identifier);
    }

    public void UnRegisterConfigurable (string identifier) {
      if (this._configurables.ContainsKey (identifier)) {
        if (this.Debugging)
          Debug.Log (string.Format ("Environment {0} unregistered configurable {1}", this.name, identifier));
      }

      this._configurables.Remove (identifier);
    }

    #endregion

    #region Transformations

    public Vector3 TransformPosition (Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.InverseTransformPoint (position);
        return position;
      }

      if (this._coordinate_system == CoordinateSystem.LocalCoordinates)
        return position - this.transform.position;
      return position;
    }

    public Vector3 InverseTransformPosition (Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.TransformPoint (position);
        return position;
      }

      if (this._coordinate_system == CoordinateSystem.LocalCoordinates)
        return position - this.transform.position;
      return position;
    }

    public Vector3 TransformDirection (Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.InverseTransformDirection (direction);
        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.LocalCoordinates)
        return this.transform.InverseTransformDirection (direction);
      return direction;
    }

    public Vector3 InverseTransformDirection (Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.TransformDirection (direction);
        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.LocalCoordinates)
        return this.transform.InverseTransformDirection (direction);
      return direction;
    }

    #endregion

    #endregion

    #region PrivateMethods

    void SaveInitialPoses () {
      var ignored_layer = LayerMask.NameToLayer ("IgnoredByNeodroid");
      this._child_game_objects =
          NeodroidUtilities.RecursiveChildGameObjectsExceptLayer (this.transform, ignored_layer);
      this._reset_positions = new Vector3[this._child_game_objects.Length];
      this._reset_rotations = new Quaternion[this._child_game_objects.Length];
      this._poses = new Transform[this._child_game_objects.Length];
      for (var i = 0; i < this._child_game_objects.Length; i++) {
        this._reset_positions [i] = this._child_game_objects [i].transform.position;
        this._reset_rotations [i] = this._child_game_objects [i].transform.rotation;
        this._poses [i] = this._child_game_objects [i].transform;
        var maybe_joint = this._child_game_objects [i].GetComponent<Joint> ();
        if (maybe_joint != null)
          maybe_joint.gameObject.AddComponent<JointFix> ();
      }
    }

    void SaveInitialBodies () {
      var body_list = new List<Rigidbody> ();
      foreach (var go in this._child_game_objects) {
        if (go != null) {
          var body = go.GetComponent<Rigidbody> ();
          if (body)
            body_list.Add (body);
        }
      }

      //if (body_list.Count > 0) {
      this._bodies = body_list.ToArray ();
      this._reset_velocities = new Vector3[this._bodies.Length];
      this._reset_angulars = new Vector3[this._bodies.Length];
      for (var i = 0; i < this._bodies.Length; i++) {
        this._reset_velocities [i] = this._bodies [i].velocity;
        this._reset_angulars [i] = this._bodies [i].angularVelocity;
      }

      //}
    }

    IEnumerator SaveInitialBodiesIE () {
      yield return new WaitForFixedUpdate ();
      this.SaveInitialBodies ();
    }

    EnvironmentState GetState () {
      foreach (var a in this.Actors.Values) {
        foreach (var m in a.Motors.Values)
          this.energy_spent += m.GetEnergySpend ();
      }

      var reward = 0f;
      if (this._objective_function != null)
        reward = this._objective_function.Evaluate ();

      EnvironmentDescription description = null;
      if (this._describe) {
        var threshold = 0f;
        if (this._objective_function != null)
          threshold = this._objective_function.SolvedThreshold;
        description = new EnvironmentDescription (
          this.EpisodeLength,
          this._simulation_manager.Configuration,
          this.Actors,
          this.Configurables,
          threshold);
        this._describe = false;
      }

      return new EnvironmentState (
        this.EnvironmentIdentifier,
        this.energy_spent,
        this.Observers,
        this.CurrentFrameNumber,
        reward,
        this._terminated,
        this._bodies,
        this._poses,
        description);
    }

    protected void Reset () {
      this.ResetRegisteredObjects ();
      this.SetEnvironmentPoses (this._child_game_objects, this._reset_positions, this._reset_rotations);
      this.SetEnvironmentBodies (this._bodies, this._reset_velocities, this._reset_angulars);
    }

    protected void Configure () {
      if (this._received_poses != null) {
        var positions = new Vector3[this._received_poses.Length];
        var rotations = new Quaternion[this._received_poses.Length];
        for (var i = 0; i < this._received_poses.Length; i++) {
          positions [i] = this._received_poses [i].position;
          rotations [i] = this._received_poses [i].rotation;
        }

        this.SetEnvironmentPoses (this._child_game_objects, positions, rotations);
      }

      if (this._received_bodies != null) {
        var vels = new Vector3[this._received_bodies.Length];
        var angs = new Vector3[this._received_bodies.Length];
        for (var i = 0; i < this._received_bodies.Length; i++) {
          vels [i] = this._received_bodies [i].Velocity;
          angs [i] = this._received_bodies [i].AngularVelocity;
        }

        this.SetEnvironmentBodies (this._bodies, vels, angs);
      }

      if (this._configurations != null) {
        foreach (var configuration in this._configurations) {
          if (this._configurables.ContainsKey (configuration.ConfigurableName))
            this._configurables [configuration.ConfigurableName].ApplyConfiguration (configuration);
          else {
            if (this.Debugging) {
              Debug.Log (
                "Could find not configurable with the specified name: " + configuration.ConfigurableName);
            }
          }
        }
      }
    }

    void Step (Reaction reaction) {
      if (reaction.Parameters.EpisodeCount)
        this.CurrentFrameNumber++;
      if (reaction != null && reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          if (this.Debugging)
            Debug.Log ("Applying " + motion + " To " + this.name + "'s actors");
          var motion_actor_name = motion.GetActorName ();
          if (this.Actors.ContainsKey (motion_actor_name) && this.Actors [motion_actor_name] != null)
            this.Actors [motion_actor_name].ApplyMotion (motion);
          else {
            if (this.Debugging)
              Debug.Log ("Could find not actor with the specified name: " + motion_actor_name);
          }
        }
      }

      if (this.EpisodeLength > 0 && this.CurrentFrameNumber > this.EpisodeLength) {
        if (this.Debugging)
          Debug.Log ("Maximum episode length reached, resetting");
        this.Terminate ("Maximum episode length reached, resetting");
      }

      this.UpdateObserversData ();
    }

    void ResetRegisteredObjects () {
      if (this.Debugging)
        Debug.Log ("Resetting registed objects");
      foreach (var resetable in this.Resetables.Values) {
        if (resetable != null)
          resetable.Reset ();
      }

      foreach (var actor in this.Actors.Values) {
        if (actor)
          actor.Reset ();
      }

      foreach (var observer in this.Observers.Values) {
        if (observer)
          observer.Reset ();
      }
    }

    #region EnvironmentStateSetters

    void SetEnvironmentPoses (GameObject[] child_game_objects, Vector3[] positions, Quaternion[] rotations) {
      if (this._simulation_manager) {
        for (var iterations = 0;
             iterations < this._simulation_manager.Configuration.ResetIterations;
             iterations++) {
          for (var i = 0; i < child_game_objects.Length; i++) {
            if (child_game_objects [i] != null && i < positions.Length && i < rotations.Length) {
              var rigid_body = child_game_objects [i].GetComponent<Rigidbody> ();
              if (rigid_body)
                rigid_body.Sleep ();
              child_game_objects [i].transform.position = positions [i];
              child_game_objects [i].transform.rotation = rotations [i];
              if (rigid_body)
                rigid_body.WakeUp ();

              var joint_fix = child_game_objects [i].GetComponent<JointFix> ();
              if (joint_fix)
                joint_fix.Reset ();
              var anim = child_game_objects [i].GetComponent<Animation> ();
              if (anim)
                anim.Rewind ();
            }
          }
        }

        this._lastest_reset_time = Time.time;
        this.CurrentFrameNumber = 0;
        if (this._objective_function)
          this._objective_function.Reset ();
      }
    }

    void SetEnvironmentBodies (Rigidbody[] bodies, Vector3[] velocities, Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0) {
        for (var i = 0; i < bodies.Length; i++) {
          if (i < bodies.Length && bodies [i] != null && i < velocities.Length && i < angulars.Length) {
            if (this.Debugging) {
              print (
                string.Format (
                  "Setting {0}, velocity to {1} and angular velocity to {2}",
                  bodies [i].name,
                  velocities [i],
                  angulars [i]));
            }

            bodies [i].Sleep ();
            bodies [i].velocity = velocities [i];
            bodies [i].angularVelocity = angulars [i];
            bodies [i].WakeUp ();
          }
        }
      }
    }

    #endregion

    #endregion
  }
}
