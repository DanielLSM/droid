using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Motors;
using Neodroid.Utilities;
using UnityEngine;

namespace Assets.Neodroid.Models.Actors {
  [ExecuteInEditMode]
  //[RequireComponent (typeof(Collider))]
  public class Actor : MonoBehaviour,
                       IHasRegister<Motor> {
    private bool _alive = true;

    public bool Alive { get { return _alive; } }

    private void Awake() { Setup(); }

    private void Setup() {
      if (_motors == null)
        _motors = new Dictionary<string, Motor>();
      if (_environment != null) _environment.UnRegisterActor(ActorIdentifier);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
                                                                   ParentEnvironment,
                                                                   this);
    }

    #if UNITY_EDITOR

    void OnValidate() {
      // Only called in the editor
      //Setup ();
    }
    #endif

    public void Kill() { _alive = false; }

    public void ApplyMotion(MotorMotion motion) {
      if (_alive) {
        if (Debugging)
          print("Applying " + motion + " To " + name + "'s motors");
        var motion_motor_name = motion.GetMotorName();
        if (_motors.ContainsKey(motion_motor_name) && _motors[motion_motor_name] != null) {
          _motors[motion_motor_name].ApplyMotion(motion);
        } else {
          if (Debugging)
            print("Could find not motor with the specified name: " + motion_motor_name);
        }
      } else {
        if (Debugging)
          print("Actor is dead, cannot apply motion");
      }
    }

    public void AddMotor(Motor motor, string identifier) {
      if (Debugging)
        print("Actor " + name + " has motor " + identifier);
      if (_motors == null)
        _motors = new Dictionary<string, Motor>();
      if (!_motors.ContainsKey(identifier)) {
        _motors.Add(
                    identifier,
                    motor);
      } else {
        if (Debugging)
          print(
                string.Format(
                              "A motor with the identifier {0} is already registered",
                              identifier));
      }
    }

    public virtual void Reset() {
      if (_motors != null)
        foreach (var motor in _motors.Values)
          if (motor != null)
            motor.Reset();
      _alive = true;
    }

    #region Fields

    [Header(
      "References",
      order = 99)]
    [SerializeField]
    private LearningEnvironment _environment;

    [Header(
      "Development",
      order = 100)]
    [SerializeField]
    private bool _debugging;

    [Header(
      "General",
      order = 101)]
    [SerializeField]
    private Dictionary<string, Motor> _motors;

    #endregion

    #region Getters

    public string ActorIdentifier { get { return name; } }

    public void Register(Motor motor) {
      AddMotor(
               motor,
               motor.GetMotorIdentifier());
    }

    public void Register(Motor motor, string identifier) {
      AddMotor(
               motor,
               identifier);
    }

    public Dictionary<string, Motor> Motors { get { return _motors; } }

    public void RefreshAwake() { Awake(); }

    public void RefreshStart() { }

    public LearningEnvironment ParentEnvironment {
      get { return _environment; }
      set { _environment = value; }
    }

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    #endregion
  }
}
