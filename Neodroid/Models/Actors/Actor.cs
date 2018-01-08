using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Actors {
  [ExecuteInEditMode]
  //[RequireComponent (typeof(Collider))]
  public class Actor : MonoBehaviour,
                       IHasRegister<Motor> {
    bool _alive = true;

    public bool Alive { get { return this._alive; } }

    void Awake() { this.Setup(); }

    void Setup() {
      if (this._motors == null) this._motors = new Dictionary<string, Motor>();
      if (this._environment != null) this._environment.UnRegisterActor(identifier : this.ActorIdentifier);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
                                                                        r : this.ParentEnvironment,
                                                                        c : this);
    }

    #if UNITY_EDITOR

    void OnValidate() {
      // Only called in the editor
      //Setup ();
    }
    #endif

    public void Kill() { this._alive = false; }

    public void ApplyMotion(MotorMotion motion) {
      if (this._alive) {
        if (this.Debugging)
          print(message : "Applying " + motion + " To " + this.name + "'s motors");
        var motion_motor_name = motion.GetMotorName();
        if (this._motors.ContainsKey(key : motion_motor_name)
            && this._motors[key : motion_motor_name] != null) {
          this._motors[key : motion_motor_name].ApplyMotion(motion : motion);
        } else {
          if (this.Debugging)
            print(message : "Could find not motor with the specified name: " + motion_motor_name);
        }
      } else {
        if (this.Debugging)
          print(message : "Actor is dead, cannot apply motion");
      }
    }

    public void AddMotor(Motor motor, string identifier) {
      if (this.Debugging)
        print(message : "Actor " + this.name + " has motor " + identifier);
      if (this._motors == null) this._motors = new Dictionary<string, Motor>();
      if (!this._motors.ContainsKey(key : identifier)) {
        this._motors.Add(
                         key : identifier,
                         value : motor);
      } else {
        if (this.Debugging)
          print(
                message : string.Format(
                                        format : "A motor with the identifier {0} is already registered",
                                        arg0 : identifier));
      }
    }

    public virtual void Reset() {
      if (this._motors != null)
        foreach (var motor in this._motors.Values)
          if (motor != null)
            motor.Reset();
      this._alive = true;
    }

    #region Fields

    [Header(
      header : "References",
      order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header(
      header : "Development",
      order = 100)]
    [SerializeField]
    bool _debugging;

    [Header(
      header : "General",
      order = 101)]
    [SerializeField]
    Dictionary<string, Motor> _motors;

    #endregion

    #region Getters

    public string ActorIdentifier { get { return this.name; } }

    public void Register(Motor motor) {
      this.AddMotor(
                    motor : motor,
                    identifier : motor.GetMotorIdentifier());
    }

    public void Register(Motor motor, string identifier) {
      this.AddMotor(
                    motor : motor,
                    identifier : identifier);
    }

    public Dictionary<string, Motor> Motors { get { return this._motors; } }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { }

    public LearningEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion
  }
}
