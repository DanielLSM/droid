using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Agents;
using Neodroid.NeodroidEnvironment.Motors;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.NeodroidEnvironment.Actors {
  public class Actor : MonoBehaviour, HasRegister<Motor> {
    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    //public Quaternion _rotation;
    //public Quaternion _direction;

    public Dictionary<string, Motor> _motors;
  
    public NeodroidAgent _agent_game_object;
    public EnvironmentManager _environment_manager;

    public bool _debug = false;

    protected void Start () {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      NeodroidUtilities.MaybeRegisterComponent (_agent_game_object, this);
      Setup ();
      UpdatePosRotDir ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    void UpdatePosRotDir () {
      if (_environment_manager) {
        _position = _environment_manager.TransformPosition (this.transform.position);
        _direction = _environment_manager.TransformDirection (this.transform.forward);
        _rotation = _environment_manager.TransformDirection (this.transform.up);
      } else {
        _position = this.transform.position;
        _direction = this.transform.forward;
        _rotation = this.transform.up;
      }
    }

    private void Update () {
      UpdatePosRotDir ();
    }

    public Dictionary<string, Motor> GetMotors () {
      return _motors;
    }

    public void AddMotor (Motor motor) {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      if (_debug)
        Debug.Log ("Actor " + name + " has motor " + motor);
      _motors.Add (motor._motor_identifier, motor);
    }

    public void Register (Motor obj) {
      AddMotor (obj);
    }

    public virtual void Reset () {
      foreach (var motor in _motors.Values) {
        motor.Reset ();
      }
    }
  }
}
