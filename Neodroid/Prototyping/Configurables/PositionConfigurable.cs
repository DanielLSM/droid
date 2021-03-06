﻿using System;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class PositionConfigurable : ConfigurableGameObject,
                                      IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] bool _use_environments_space;

    string _x;
    string _y;
    string _z;

    public override string ConfigurableIdentifier { get { return this.name + "Position"; } }

    public Vector3 ObservationValue { get { return this._position; } }

    protected override void AddToEnvironment() {
      this._x = this.ConfigurableIdentifier + "X";
      this._y = this.ConfigurableIdentifier + "Y";
      this._z = this.ConfigurableIdentifier + "Z";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._z);
    }

    public override void UpdateObservation() {
      if (this._use_environments_space)
        this._position = this.ParentEnvironment.TransformPosition(this.transform.position);
      else
        this._position = this.transform.position;
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space)
        pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      var v = configuration.ConfigurableValue;
      if (this.ConfigurableValueSpace.DecimalGranularity >= 0)
        v = (int)Math.Round(v, this.ConfigurableValueSpace.DecimalGranularity);
      if (this.ConfigurableValueSpace.MinValue.CompareTo(this.ConfigurableValueSpace.MaxValue) != 0) {
        if (v < this.ConfigurableValueSpace.MinValue || v > this.ConfigurableValueSpace.MaxValue) {
          print(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.ConfigurableValueSpace.MinValue,
                  this.ConfigurableValueSpace.MaxValue,
                  v));
          return; // Do nothing
        }
      }

      if (this.Debugging)
        print(string.Format("Applying {0} to {1} configurable", v, configuration.ConfigurableName));
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v - pos.x, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v - pos.y, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v - pos.z);
      } else {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v);
      }

      var inv_pos = pos;
      if (this._use_environments_space)
        inv_pos = this.ParentEnvironment.InverseTransformPosition(inv_pos);
      this.transform.position = inv_pos;
    }
  }
}
