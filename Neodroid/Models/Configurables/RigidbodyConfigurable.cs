using System;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyConfigurable : ConfigurableGameObject,
                                       IHasRigidbody {
    string _ang_x;
    string _ang_y;
    string _ang_z;

    [SerializeField] Vector3 _angular_velocity;

    Rigidbody _rigidbody;

    string _vel_x;
    string _vel_y;
    string _vel_z;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _velocity;

    public override string ConfigurableIdentifier {
      get {
        {
          return this.name + "Rigidbody";
        }
      }
    }

    public Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    public override void UpdateObservation() {
      this.Velocity = this._rigidbody.velocity;
      this.AngularVelocity = this._rigidbody.angularVelocity;
    }

    protected override void Start() {
      this._rigidbody = this.GetComponent<Rigidbody>();
      this.UpdateObservation();
    }

    protected override void AddToEnvironment() {
      this._vel_x = this.ConfigurableIdentifier + "VelX";
      this._vel_y = this.ConfigurableIdentifier + "VelY";
      this._vel_z = this.ConfigurableIdentifier + "VelZ";
      this._ang_x = this.ConfigurableIdentifier + "AngX";
      this._ang_y = this.ConfigurableIdentifier + "AngY";
      this._ang_z = this.ConfigurableIdentifier + "AngZ";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_z);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_z);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var vel = this._rigidbody.velocity;
      var ang = this._rigidbody.velocity;

      var v = configuration.ConfigurableValue;
      if (this.ValidInput.DecimalGranularity >= 0)
        v = (int)Math.Round(v, this.ValidInput.DecimalGranularity);
      if (this.ValidInput.MinValue.CompareTo(this.ValidInput.MaxValue) != 0) {
        if (v < this.ValidInput.MinValue || v > this.ValidInput.MaxValue) {
          print(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.ValidInput.MinValue,
                  this.ValidInput.MaxValue,
                  v));
          return; // Do nothing
        }
      }

      if (this.Debugging)
        print("Applying " + v + " To " + this.ConfigurableIdentifier);
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._vel_x)
          vel.Set(v - vel.x, vel.y, vel.z);
        else if (configuration.ConfigurableName == this._vel_y)
          vel.Set(vel.x, v - vel.y, vel.z);
        else if (configuration.ConfigurableName == this._vel_z)
          vel.Set(vel.x, vel.y, v - vel.z);
        else if (configuration.ConfigurableName == this._ang_x)
          ang.Set(v - ang.x, ang.y, ang.z);
        else if (configuration.ConfigurableName == this._ang_y)
          ang.Set(ang.x, v - ang.y, ang.z);
        else if (configuration.ConfigurableName == this._ang_z)
          ang.Set(ang.x, ang.y, v - ang.z);
      } else {
        if (configuration.ConfigurableName == this._vel_x)
          vel.Set(v, vel.y, vel.z);
        else if (configuration.ConfigurableName == this._vel_y)
          vel.Set(vel.x, v, vel.z);
        else if (configuration.ConfigurableName == this._vel_z)
          vel.Set(vel.x, vel.y, v);
        else if (configuration.ConfigurableName == this._ang_x)
          ang.Set(v, ang.y, ang.z);
        else if (configuration.ConfigurableName == this._ang_y)
          ang.Set(ang.x, v, ang.z);
        else if (configuration.ConfigurableName == this._ang_z)
          ang.Set(ang.x, ang.y, v);
      }

      this._rigidbody.velocity = vel;
      this._rigidbody.angularVelocity = ang;
    }
  }
}
