using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Configurations;
using Neodroid.Utilities;
using Neodroid.Agents;

namespace Neodroid.Managers {
  public class EnvironmentManager : MonoBehaviour, HasRegister<NeodroidAgent>, HasRegister<Configurable> {

    #region PublicMembers

    public int _episode_length = 0;
    public int _frames_spent_resetting = 10;
    public bool _wait_for_reaction_every_frame = false;
    public CoordinateSystem _coordinate_system = CoordinateSystem.GlobalCoordinates;
    public Transform _coordinate_reference_point;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Vector3[] _reset_positions;
    Quaternion[] _reset_rotations;
    GameObject[] _game_objects;
    Dictionary<string, Configurable> _configurables = new Dictionary<string, Configurable> ();
    Dictionary<string, NeodroidAgent> _agents = new Dictionary<string, NeodroidAgent> ();
    int _current_episode_frame = 0;
    float _last_reset_time = 0;
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
        PauseEnviroment ();
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

    public void InterruptEnvironment () {
      foreach (var agent in _agents.Values) {
        if (_debug)
          Debug.Log ("Interrupting agent");
        agent.ResetRegisteredObjects ();
        agent.Interrupt ();
      }
      ResetEnvironment ();
      _current_episode_frame = 0;
      _is_environment_updated = false;
    }

    public Vector3 TransformPosition (Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        if (_coordinate_reference_point) {
          return _coordinate_reference_point.transform.InverseTransformPoint (position);
        } else {
          return position;
        }
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
      } else {
        return direction;
      }
    }

    public void Step () {
      _current_episode_frame++;
      ResumeEnvironment ();
    }

    public bool IsEnvironmentUpdated () {
      return _is_environment_updated;
    }

    public int GetCurrentFrameNumber () {
      return _current_episode_frame;
    }

    public float GetTimeSinceReset () {
      return Time.time - _last_reset_time;//Time.realtimeSinceStartup;
    }

    public void ResetEnvironment () {
      for (int resets = 0; resets < _frames_spent_resetting; resets++) { 
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
      _last_reset_time = Time.time;
    }

    public void Configure (float configuration) {
      foreach (var configurable in _configurables.Values) {
        configurable.ApplyConfiguration (configuration);
      }
    }

    public bool IsEnvironmentPaused () {
      return Time.timeScale == 0;
    }

    void PauseEnviroment () {
      Time.timeScale = 0;
    }

    void ResumeEnvironment () {
      Time.timeScale = 1;
      _is_environment_updated = false;
    }

    #region Registration

    public void Register (NeodroidAgent obj) {
      AddAgent (obj);
    }

    public void Register (Configurable obj) {
      AddConfigurable (obj);
    }

    #endregion

    #endregion

    #region Helpers

    void AddAgent (NeodroidAgent agent) {
      if (_debug)
        Debug.Log ("Environment " + name + " has agent " + agent.name);
      _agents.Add (agent.name, agent);
    }

    void AddConfigurable (Configurable configurable) {
      if (_debug)
        Debug.Log ("Environment " + name + " has configurable " + configurable.name);
      _configurables.Add (configurable.name, configurable);
    }

    #endregion
  }
}
