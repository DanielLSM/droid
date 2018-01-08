using System;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class PositionConfigurable : ConfigurableGameObject,
                                      IHasEulerPosition {
    [Header(
      header : "Observation",
      order = 103)]
    [SerializeField]
    Vector3 _position;

    string _x;
    string _y;
    string _z;

    public override string ConfigurableIdentifier { get { return this.name + "Position"; } }

    public Vector3 Position { get { return this._position; } }

    protected override void AddToEnvironment() {
      this._x = this.ConfigurableIdentifier + "X";
      this._y = this.ConfigurableIdentifier + "Y";
      this._z = this.ConfigurableIdentifier + "Z";
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterComponent(
                                                 r : this.ParentEnvironment,
                                                 c : (ConfigurableGameObject)this);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._x);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._y);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._z);
    }

    public override void UpdateObservation() {
      this._position = this.ParentEnvironment.TransformPosition(position : this.transform.position);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.ParentEnvironment.TransformPosition(position : this.transform.position);
      var v = configuration.ConfigurableValue;
      if (this.ValidInput.DecimalGranularity >= 0)
        v = (int)Math.Round(
                            value : v,
                            digits : this.ValidInput.DecimalGranularity);
      if (this.ValidInput.MinValue.CompareTo(value : this.ValidInput.MaxValue) != 0)
        if (v < this.ValidInput.MinValue || v > this.ValidInput.MaxValue) {
          print(
                message : string.Format(
                                        format :
                                        "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                                        arg0 : this.ValidInput.MinValue,
                                        arg1 : this.ValidInput.MaxValue,
                                        arg2 : v));
          return; // Do nothing
        }

      if (this.Debugging)
        print(
              message : string.Format(
                                      format : "Applying {0} to {1} configurable",
                                      arg0 : v,
                                      arg1 : configuration.ConfigurableName));
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x)
          pos.Set(
                  newX : v - pos.x,
                  newY : pos.y,
                  newZ : pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(
                  newX : pos.x,
                  newY : v - pos.y,
                  newZ : pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(
                  newX : pos.x,
                  newY : pos.y,
                  newZ : v - pos.z);
      } else {
        if (configuration.ConfigurableName == this._x)
          pos.Set(
                  newX : v,
                  newY : pos.y,
                  newZ : pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(
                  newX : pos.x,
                  newY : v,
                  newZ : pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(
                  newX : pos.x,
                  newY : pos.y,
                  newZ : v);
      }

      var inv_pos = this.ParentEnvironment.InverseTransformPosition(position : pos);
      this.transform.position = inv_pos;
    }
  }
}
