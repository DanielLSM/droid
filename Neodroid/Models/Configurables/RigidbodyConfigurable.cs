using Neodroid.Configurables;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Assets.Neodroid.Models.Configurables {
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class RigidbodyConfigurable : ConfigurableGameObject,
                                       HasRigidbodyProperties {
    string _ang_x;
    string _ang_y;
    string _ang_z;

    [SerializeField]
    Vector3 _angular_velocity;

    Rigidbody _rigidbody;

    string _vel_x;
    string _vel_y;
    string _vel_z;

    [Header(
      header : "Observation",
      order = 103)]
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
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._vel_x);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._vel_y);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._vel_z);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._ang_x);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._ang_y);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._ang_z);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var vel = this._rigidbody.velocity;
      var ang = this._rigidbody.velocity;

      var v = configuration.ConfigurableValue;
      if (this.ValidInput.decimal_granularity >= 0)
        v = (int)System.Math.Round(
                            value : v,
                            digits : this.ValidInput.decimal_granularity);
      if (this.ValidInput.min_value.CompareTo(value : this.ValidInput.max_value) != 0)
        if (v < this.ValidInput.min_value || v > this.ValidInput.max_value) {
          print(
                message : string.Format(
                                        format :
                                        "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                                        arg0 : this.ValidInput.min_value,
                                        arg1 : this.ValidInput.max_value,
                                        arg2 : v));
          return; // Do nothing
        }

      if (this.Debugging)
        print(message : "Applying " + v + " To " + this.ConfigurableIdentifier);
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._vel_x)
          vel.Set(
                  newX : v - vel.x,
                  newY : vel.y,
                  newZ : vel.z);
        else if (configuration.ConfigurableName == this._vel_y)
          vel.Set(
                  newX : vel.x,
                  newY : v - vel.y,
                  newZ : vel.z);
        else if (configuration.ConfigurableName == this._vel_z)
          vel.Set(
                  newX : vel.x,
                  newY : vel.y,
                  newZ : v - vel.z);
        else if (configuration.ConfigurableName == this._ang_x)
          ang.Set(
                  newX : v - ang.x,
                  newY : ang.y,
                  newZ : ang.z);
        else if (configuration.ConfigurableName == this._ang_y)
          ang.Set(
                  newX : ang.x,
                  newY : v - ang.y,
                  newZ : ang.z);
        else if (configuration.ConfigurableName == this._ang_z)
          ang.Set(
                  newX : ang.x,
                  newY : ang.y,
                  newZ : v - ang.z);
      } else {
        if (configuration.ConfigurableName == this._vel_x)
          vel.Set(
                  newX : v,
                  newY : vel.y,
                  newZ : vel.z);
        else if (configuration.ConfigurableName == this._vel_y)
          vel.Set(
                  newX : vel.x,
                  newY : v,
                  newZ : vel.z);
        else if (configuration.ConfigurableName == this._vel_z)
          vel.Set(
                  newX : vel.x,
                  newY : vel.y,
                  newZ : v);
        else if (configuration.ConfigurableName == this._ang_x)
          ang.Set(
                  newX : v,
                  newY : ang.y,
                  newZ : ang.z);
        else if (configuration.ConfigurableName == this._ang_y)
          ang.Set(
                  newX : ang.x,
                  newY : v,
                  newZ : ang.z);
        else if (configuration.ConfigurableName == this._ang_z)
          ang.Set(
                  newX : ang.x,
                  newY : ang.y,
                  newZ : v);
      }

      this._rigidbody.velocity = vel;
      this._rigidbody.angularVelocity = ang;
    }
  }
}
