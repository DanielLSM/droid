using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Neodroid.Evaluation;
using Neodroid.Utilities;
using Neodroid.Observers;
using Neodroid.Motors;
using Neodroid.Actors;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Configurables;

namespace Neodroid.Environments {
  public class LearningEnvironment : MonoBehaviour, HasRegister<Actor>, HasRegister<Observer>, HasRegister<ConfigurableGameObject> {

    #region PublicMembers

    public CoordinateSystem _coordinate_system = CoordinateSystem.LocalCoordinates;
    public Transform _coordinate_reference_point;

    //infinite
    public ObjectiveFunction _objective_function;
    public SimulationManager _simulation_manager;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Vector3[] _reset_positions;
    Quaternion[] _reset_rotations;
    GameObject[] _child_game_objects;
    Configuration[] _configurations;
    Dictionary<string, Actor> _actors = new Dictionary<string, Actor> ();
    Dictionary<string, Observer> _observers = new Dictionary<string, Observer> ();
    Dictionary<string, ConfigurableGameObject> _configurables = new Dictionary<string, ConfigurableGameObject> ();


    int _current_episode_frame = 0;
    float _lastest_reset_time = 0;
    float energy_spent = 0f;
    bool _interrupted = false;

    #endregion

    #region UnityCallbacks

    void Start () {
      FindMissingMembers ();
      AddToEnvironment ();
      SaveInitialPoses ();
    }

    void SaveInitialPoses () {
      var _ignored_layer = LayerMask.NameToLayer ("IgnoredByNeodroid");
      _child_game_objects = NeodroidUtilities.FindAllGameObjectsExceptLayer (_ignored_layer);
      _reset_positions = new Vector3[_child_game_objects.Length];
      _reset_rotations = new Quaternion[_child_game_objects.Length];
      for (int i = 0; i < _child_game_objects.Length; i++) {
        _reset_positions [i] = _child_game_objects [i].transform.position;
        _reset_rotations [i] = _child_game_objects [i].transform.rotation;
      }
    }

    #endregion

    #region Helpers


    void FindMissingMembers () {
      if (!_simulation_manager) {
        _simulation_manager = FindObjectOfType<SimulationManager> ();
      }
      if (!_objective_function) {
        _objective_function = FindObjectOfType<ObjectiveFunction> ();
      }
    }

    public void UpdateObserversData () {
      foreach (Observer obs in RegisteredObservers.Values) {
        obs.UpdateData ();
      }
    }

    public EnvironmentState GetCurrentState () {
      foreach (Actor a in _actors.Values) {
        foreach (Motor m in a.RegisteredMotors.Values) {
          energy_spent += m.GetEnergySpend ();
        }
      }
      var reward = 0f;
      if (_objective_function != null) {
        reward = _objective_function.Evaluate ();
      }
      EnvironmentDescription description = null;
      if (_interrupted) {
        description = new EnvironmentDescription (
          _simulation_manager._episode_length, 
          _simulation_manager._frame_skips, 
          _actors, 
          _configurables, 
          _objective_function._solved_threshold
        );
      }
      return new EnvironmentState (
        GetEnvironmentIdentifier (),
        energy_spent,
        _observers,
        GetCurrentFrameNumber (),
        reward,
        _interrupted,
        description
      );
    }

    public int GetCurrentFrameNumber () {
      return _current_episode_frame;
    }

    public float GetTimeSinceReset () {
      return Time.time - _lastest_reset_time;//Time.realtimeSinceStartup;
    }

    public void ExecuteReaction (Reaction reaction) {
      _current_episode_frame++;
      if (_simulation_manager._episode_length > 0 && _current_episode_frame > _simulation_manager._episode_length) {
        if (_debug)
          Debug.Log ("Maximum episode length reached, resetting");
        Interrupt ("Maximum episode length reached, resetting");
      } else if (reaction != null && reaction.Reset) {
        Interrupt ("Reaction called reset");
        _configurations = reaction.Configurations;
      } else if (reaction != null && reaction.Motions != null && reaction.Motions.Length > 0)
        foreach (MotorMotion motion in reaction.Motions) {
          if (_debug)
            Debug.Log ("Applying " + motion.ToString () + " To " + name + "'s actors");
          var motion_actor_name = motion.GetActorName ();
          if (RegisteredActors.ContainsKey (motion_actor_name)) {
            RegisteredActors [motion_actor_name].ApplyMotion (motion);
          } else {
            if (_debug)
              Debug.Log ("Could find not actor with the specified name: " + motion_actor_name);
          }
        }
          
      UpdateObserversData ();
    }

    public void Configure () {
      if (_configurations != null) {
        foreach (var configuration in _configurations) {
          if (_configurables.ContainsKey (configuration.ConfigurableName)) {
            _configurables [configuration.ConfigurableName].ApplyConfiguration (configuration);
          } else {
            if (_debug)
              Debug.Log ("Could find not configurable with the specified name: " + configuration.ConfigurableName);
          }
        }
      }
    }

    void AddToEnvironment () {
      _simulation_manager = NeodroidUtilities.MaybeRegisterComponent (_simulation_manager, this);
    }


    #endregion

    #region PublicMethods

    public Dictionary<string, Actor> RegisteredActors {
      get {
        return _actors;
      }
    }

    public Dictionary<string, Observer> RegisteredObservers {
      get {
        return _observers;
      }
    }

    public Dictionary<string, ConfigurableGameObject> RegisteredConfigurables {
      get {
        return _configurables;
      }
    }

    public Vector3 TransformPosition (Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (_coordinate_reference_point) {
          return _coordinate_reference_point.transform.InverseTransformPoint (position);
        } else {
          return position;
        }
      } else if (_coordinate_system == CoordinateSystem.LocalCoordinates) {
        return position - this.transform.position;
      } else {
        return position;
      }
    }

    public Vector3 InverseTransformPosition (Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (_coordinate_reference_point) {
          return _coordinate_reference_point.transform.TransformPoint (position);
        } else {
          return position;
        }
      } else if (_coordinate_system == CoordinateSystem.LocalCoordinates) {
        return position - this.transform.position;
      } else {
        return position;
      }
    }

    public Vector3 TransformDirection (Vector3 direction) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (_coordinate_reference_point) {
          return _coordinate_reference_point.transform.InverseTransformDirection (direction);
        } else {
          return direction;
        }
      } else if (_coordinate_system == CoordinateSystem.LocalCoordinates) {
        return this.transform.InverseTransformDirection (direction);
      } else {
        return direction;
      }
    }

    public Vector3 InverseTransformDirection (Vector3 direction) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (_coordinate_reference_point) {
          return _coordinate_reference_point.transform.TransformDirection (direction);
        } else {
          return direction;
        }
      } else if (_coordinate_system == CoordinateSystem.LocalCoordinates) {
        return this.transform.InverseTransformDirection (direction);
      } else {
        return direction;
      }
    }

    public void Interrupt (string reason) {
      _interrupted = true;
      if (_debug) {
        print (System.String.Format ("Was interrupted, because {0}", reason));
      }
    }

    public void PostUpdate () {
      if (_interrupted) {
        _interrupted = false;
        Reset ();
      }
    }

    void Reset () {
      ResetRegisteredObjects ();
      ResetEnvironment ();
      Configure ();
    }

    public string GetEnvironmentIdentifier () {
      return name;
    }

    void ResetRegisteredObjects () {
      if (_debug)
        Debug.Log ("Resetting registed objects");
      foreach (var actor in _actors.Values) {
        actor.Reset ();
      }
      foreach (var observer in _observers.Values) {
        observer.Reset ();
      }
    }

    void ResetEnvironment () {
      if (_simulation_manager) {
        for (int resets = 0; resets < _simulation_manager._resets; resets++) {
          for (int i = 0; i < _child_game_objects.Length; i++) {
            var rigid_body = _child_game_objects [i].GetComponent<Rigidbody> ();
            if (rigid_body)
              rigid_body.Sleep ();
            _child_game_objects [i].transform.position = _reset_positions [i];
            _child_game_objects [i].transform.rotation = _reset_rotations [i];
            if (rigid_body)
              rigid_body.WakeUp ();

            var animation = _child_game_objects [i].GetComponent<Animation> ();
            if (animation)
              animation.Rewind ();
          }
        }
        _lastest_reset_time = Time.time;
        _current_episode_frame = 0;
        if (_objective_function) {
          _objective_function.Reset ();
        }
      }
    }

    #region Registration

    public void Register (Actor actor) {
      if (!_actors.ContainsKey (actor.GetActorIdentifier ())) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered actor {1}", name, actor.GetActorIdentifier ()));
        _actors.Add (actor.GetActorIdentifier (), actor);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has actor {1} registered", name, actor.GetActorIdentifier ()));
      }
    }

    public void Register (Actor actor, string identifier) {
      if (!_actors.ContainsKey (identifier)) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered actor {1}", name, identifier));
        _actors.Add (identifier, actor);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has actor {1} registered", name, identifier));
      }
    }

    public void Register (Observer observer) {
      if (!_observers.ContainsKey (observer.GetObserverIdentifier ())) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered observer {1}", name, observer.GetObserverIdentifier ()));
        _observers.Add (observer.GetObserverIdentifier (), observer);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has observer {1} registered", name, observer.GetObserverIdentifier ()));
      }
    }

    public void Register (Observer observer, string identifier) {
      if (!_observers.ContainsKey (identifier)) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered observer {1}", name, identifier));
        _observers.Add (identifier, observer);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has observer {1} registered", name, identifier));
      }
    }

    public void Register (ConfigurableGameObject configurable) {
      if (!_configurables.ContainsKey (configurable.GetConfigurableIdentifier ())) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered configurable {1}", name, configurable.GetConfigurableIdentifier ()));
        _configurables.Add (configurable.GetConfigurableIdentifier (), configurable);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has configurable {1} registered", name, configurable.GetConfigurableIdentifier ()));
      }
    }

    public void Register (ConfigurableGameObject configurable, string identifier) {
      if (!_configurables.ContainsKey (identifier)) {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} has registered configurable {1}", name, identifier));
        _configurables.Add (identifier, configurable);
      } else {
        if (_debug)
          Debug.Log (string.Format ("Environment {0} already has configurable {1} registered", name, identifier));
      }
    }

    public void UnRegisterActor (string identifier) {
      if (_actors.ContainsKey (identifier))
      if (_debug)
        Debug.Log (string.Format ("Environment {0} unregistered actor {1}", name, identifier));
      _actors.Remove (identifier);
    }

    public void UnRegisterObserver (string identifier) {
      if (_observers.ContainsKey (identifier))
      if (_debug)
        Debug.Log (string.Format ("Environment {0} unregistered observer {1}", name, identifier));
      _observers.Remove (identifier);
    }

    public void UnRegisterConfigurable (string identifier) {
      if (_configurables.ContainsKey (identifier))
      if (_debug)
        Debug.Log (string.Format ("Environment {0} unregistered configurable {1}", name, identifier));
      _configurables.Remove (identifier);
    }



    #endregion

    #endregion


  }
}
