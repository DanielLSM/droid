using System.Collections;
using System.Collections.Generic;
using Neodroid.Configurations;
using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Managers {
  public class EnvironmentManager : MonoBehaviour, HasRegister<LearningEnvironment> {

    #region PublicMembers

    public int _episode_length = 1000;
    public int _frame_skips = 0;
    public int _resets = 6;
    public bool _wait_for_reaction_every_frame = false;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Vector3[] _reset_positions;
    Quaternion[] _reset_rotations;
    GameObject[] _game_objects;
    Dictionary<string, LearningEnvironment> _environments = new Dictionary<string, LearningEnvironment> ();
    int _current_episode_frame = 0;
    float _lastest_reset_time = 0;
    Configuration[] _lastest_received_configurations;
    bool _is_environment_updated = false;

    #endregion

    #region UnityCallbacks

    void Start () {
      var _ignored_layer = LayerMask.NameToLayer ("IgnoredByNeodroid");
      _game_objects = NeodroidUtilities.FindAllGameObjectsExceptLayer (_ignored_layer);
      _reset_positions = new Vector3[_game_objects.Length];
      _reset_rotations = new Quaternion[_game_objects.Length];
      for (int i = 0; i < _game_objects.Length; i++) {
        _reset_positions [i] = _game_objects [i].transform.position;
        _reset_rotations [i] = _game_objects [i].transform.rotation;
      }
    }

    void FixedUpdate () {
      if (_wait_for_reaction_every_frame) {
        PauseSimulation ();
      }
    }

    void Update () {
      if (_episode_length > 0 && _current_episode_frame > _episode_length) {
        if (_debug)
          Debug.Log ("Maximum episode length reached, resetting");
        InterruptEnvironment ();
      }
    }

    void LateUpdate () {
      _is_environment_updated = true;
    }

    #endregion

    #region PublicMethods

    public void Step () {
      _current_episode_frame++;
      ResumeSimulation ();
    }

    public bool IsEnvironmentUpdated () {
      return _is_environment_updated;
    }

    public int GetCurrentFrameNumber () {
      return _current_episode_frame;
    }

    public float GetTimeSinceReset () {
      return Time.time - _lastest_reset_time;//Time.realtimeSinceStartup;
    }

    public void InterruptEnvironment () {
      foreach (var environment in _environments.Values) {
        if (_debug)
          Debug.Log ("Interrupting environment");
        environment.Interrupt ();
      }
      ResetEnvironment ();
    }


    public void ResetEnvironment () {
      for (int resets = 0; resets < _resets; resets++) { 
        for (int i = 0; i < _game_objects.Length; i++) {
          var rigid_body = _game_objects [i].GetComponent<Rigidbody> ();
          if (rigid_body)
            rigid_body.Sleep ();
          _game_objects [i].transform.position = _reset_positions [i];
          _game_objects [i].transform.rotation = _reset_rotations [i];
          if (rigid_body)
            rigid_body.WakeUp ();
          
          var animation = _game_objects [i].GetComponent<Animation> ();
          if (animation)
            animation.Rewind ();
        }
      }
      if (_lastest_received_configurations != null) {
        foreach (var environment in _environments) {
          //environment.Configure (_lastest_received_configurations);
        }
      }
      _lastest_reset_time = Time.time;
      _current_episode_frame = 0;
      _is_environment_updated = false;
    }


    public bool IsSimulationPaused () {
      return Time.timeScale == 0;
    }

    void PauseSimulation () {
      Time.timeScale = 0;
    }

    void ResumeSimulation () {
      Time.timeScale = 1;
      _is_environment_updated = false;
    }

    #region Registration

    public void Register (LearningEnvironment environment) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, environment.GetEnvironmentIdentifier ()));
      _environments.Add (environment.GetEnvironmentIdentifier (), environment);
    }

    public void Register (LearningEnvironment environment, string identifier) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, identifier));
      _environments.Add (identifier, environment);
    }

    #endregion

    #endregion

  }
}
