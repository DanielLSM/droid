using System.Collections.Generic;
using Neodroid.Models.Environments;
using Neodroid.Models.Motors.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Actors {
  [ExecuteInEditMode]
  //[RequireComponent (typeof(Collider))]
  public class Actor : MonoBehaviour,
                       IHasRegister<Motor> {
    [SerializeField] bool _alive = true;

    [SerializeField] Bounds _bounds;
    [SerializeField] bool _draw_bounds;

    public bool Alive { get { return this._alive; } }

    public Bounds ActorBounds {
      get {
        var col = this.GetComponent<BoxCollider>();
        this._bounds = new Bounds(this.transform.position, Vector3.zero); // position and size

        if (col) this._bounds.Encapsulate(col.bounds);

        foreach (var child_col in this.GetComponentsInChildren<Collider>()) {
          if (child_col != col)
            this._bounds.Encapsulate(child_col.bounds);
        }

        return this._bounds;
      }
    }

    void Awake() { this.Setup(); }

    void Setup() {
      if (this._motors == null)
        this._motors = new Dictionary<string, Motor>();
      if (this._environment != null)
        this._environment.UnRegisterActor(this.ActorIdentifier);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    void Update() {
      if (this._draw_bounds) {
        var corners = NeodroidUtilities.ExtractCorners(
            this.ActorBounds.center,
            this.ActorBounds.extents,
            this.transform);

        NeodroidUtilities.DrawBox(
            corners[0],
            corners[1],
            corners[2],
            corners[3],
            corners[4],
            corners[5],
            corners[6],
            corners[7],
            Color.gray);
      }
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
          print("Applying " + motion + " To " + this.name + "'s motors");
        var motion_motor_name = motion.GetMotorName();
        if (this._motors.ContainsKey(motion_motor_name) && this._motors[motion_motor_name] != null)
          this._motors[motion_motor_name].ApplyMotion(motion);
        else {
          if (this.Debugging)
            print("Could find not motor with the specified name: " + motion_motor_name);
        }
      } else {
        if (this.Debugging)
          print("Actor is dead, cannot apply motion");
      }
    }

    public void AddMotor(Motor motor, string identifier) {
      if (this.Debugging)
        print("Actor " + this.name + " has motor " + identifier);
      if (this._motors == null)
        this._motors = new Dictionary<string, Motor>();
      if (!this._motors.ContainsKey(identifier))
        this._motors.Add(identifier, motor);
      else {
        if (this.Debugging)
          print(string.Format("A motor with the identifier {0} is already registered", identifier));
      }
    }

    public virtual void Reset() {
      if (this._motors != null) {
        foreach (var motor in this._motors.Values) {
          if (motor != null)
            motor.Reset();
        }
      }

      this._alive = true;
    }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    Dictionary<string, Motor> _motors;

    #endregion

    #region Getters

    public string ActorIdentifier { get { return this.name; } }

    public void Register(Motor motor) { this.AddMotor(motor, motor.GetMotorIdentifier()); }

    public void Register(Motor motor, string identifier) { this.AddMotor(motor, identifier); }

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
