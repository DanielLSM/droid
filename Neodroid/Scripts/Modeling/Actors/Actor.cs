using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Motors;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using System;

namespace Neodroid.Actors {
  public class Actor : MonoBehaviour, HasRegister<Motor> {
    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    //public Quaternion _rotation;
    //public Quaternion _direction;

    public Dictionary<string, Motor> _motors;
  
    public LearningEnvironment _environment;

    public bool _debug = false;

    void Awake () {
      Setup ();
    }

    void Setup () {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      _environment = NeodroidUtilities.MaybeRegisterComponent (_environment, this);
    }

    #if UNITY_EDITOR
    void OnValidate () { // Only called in the editor
    //Setup ();
    }
    #endif

    protected void Start () {
      UpdatePosRotDir ();
    }


    public void ApplyMotion (MotorMotion motion) {
      if (_debug)
        Debug.Log ("Applying " + motion.ToString () + " To " + name + "'s motors");
      var motion_motor_name = motion.GetMotorName ();
      if (_motors.ContainsKey (motion_motor_name)) {
        _motors [motion_motor_name].ApplyMotion (motion);
      } else {
        if (_debug)
          Debug.Log ("Could find not motor with the specified name: " + motion_motor_name);
      }
        
    }

    void UpdatePosRotDir () {
      if (_environment) {
        _position = _environment.TransformPosition (this.transform.position);
        _direction = _environment.TransformDirection (this.transform.forward);
        _rotation = _environment.TransformDirection (this.transform.up);
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

    public void AddMotor (Motor motor, string identifier) {
      if (_debug)
        Debug.Log ("Actor " + name + " has motor " + identifier);
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      if (!_motors.ContainsKey (identifier)) {
        _motors.Add (identifier, motor);
      } else {
        if (_debug)
          Debug.Log (String.Format ("A motor with the identifier {0} is already registered", identifier));
      }
    }

    public string GetActorIdentifier () {
      return name + "Actor";
    }

    public void Register (Motor motor) {
      AddMotor (motor, motor.GetMotorIdentifier ());
    }

    public void Register (Motor motor, string identifier) {
      AddMotor (motor, identifier);
    }

    public virtual void Reset () {
      if (_motors != null) {
        foreach (var motor in _motors.Values) {
          if (motor != null) {
            motor.Reset ();
          }
        }
      }
    }
  }
}
