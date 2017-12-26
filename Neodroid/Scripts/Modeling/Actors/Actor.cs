using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Motors;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;

namespace Neodroid.Actors {
  [ExecuteInEditMode]
  //[RequireComponent (typeof(Collider))]
  public class Actor : MonoBehaviour, HasRegister<Motor> {

    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging = false;

    [Header ("General", order = 101)]
    [SerializeField]
    Dictionary<string, Motor> _motors;


    #endregion

    void Awake () {
      Setup ();
    }

    void Setup () {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      if (_environment != null) {
        _environment.UnRegisterActor (ActorIdentifier);
      }
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent (ParentEnvironment, this);
    }

    #if UNITY_EDITOR
    void OnValidate () { // Only called in the editor
      //Setup ();
    }
    #endif


    public void ApplyMotion (MotorMotion motion) {
      if (Debugging)
        print ("Applying " + motion.ToString () + " To " + name + "'s motors");
      var motion_motor_name = motion.GetMotorName ();
      if (_motors.ContainsKey (motion_motor_name) && _motors [motion_motor_name] != null) {
        _motors [motion_motor_name].ApplyMotion (motion);
      } else {
        if (Debugging)
          print ("Could find not motor with the specified name: " + motion_motor_name);
      }

    }

    public void AddMotor (Motor motor, string identifier) {
      if (Debugging)
        print ("Actor " + name + " has motor " + identifier);
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      if (!_motors.ContainsKey (identifier)) {
        _motors.Add (identifier, motor);
      } else {
        if (Debugging)
          print (System.String.Format ("A motor with the identifier {0} is already registered", identifier));
      }
    }

    #region Getters

    public string ActorIdentifier { get { return name; } }

    public void Register (Motor motor) {
      AddMotor (motor, motor.GetMotorIdentifier ());
    }

    public void Register (Motor motor, string identifier) {
      AddMotor (motor, identifier);
    }

    public Dictionary<string, Motor> Motors {
      get {
        return _motors;
      }
    }

    public void RefreshAwake () {
      Awake ();
    }

    public void RefreshStart () {
    }

    public LearningEnvironment ParentEnvironment {
      get {
        return _environment;
      }
      set {
        _environment = value;
      }
    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set {
        _debugging = value;
      }
    }

    #endregion

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
