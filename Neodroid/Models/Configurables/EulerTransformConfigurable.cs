using System;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class EulerTransformConfigurable : SingleEulerTransformConfigurable,
                                            IHasEulerTransformProperties {
    string _dir_x;
    string _dir_y;
    string _dir_z;

    [SerializeField] Vector3 _direction;

    [Header(
      header : "Observation",
      order = 103)]
    [SerializeField]
    Vector3 _position;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    [SerializeField] Vector3 _rotation;

    string _x;
    string _y;
    string _z;

    public override string ConfigurableIdentifier { get { return this.name + "EulerTransform"; } }

    public Vector3 Position { get { return this._position; } set { this._position = value; } }

    public Vector3 Direction { get { return this._direction; } set { this._direction = value; } }

    public Vector3 Rotation { get { return this._rotation; } set { this._rotation = value; } }

    public override void UpdateObservation() {
      this.Position = this.ParentEnvironment.TransformPosition(position : this.transform.position);
      this.Direction = this.ParentEnvironment.TransformDirection(direction : this.transform.forward);
      this.Rotation = this.ParentEnvironment.TransformDirection(direction : this.transform.up);
    }

    protected override void AddToEnvironment() {
      this._x = this.ConfigurableIdentifier + "X";
      this._y = this.ConfigurableIdentifier + "Y";
      this._z = this.ConfigurableIdentifier + "Z";
      this._dir_x = this.ConfigurableIdentifier + "DirX";
      this._dir_y = this.ConfigurableIdentifier + "DirY";
      this._dir_z = this.ConfigurableIdentifier + "DirZ";
      this._rot_x = this.ConfigurableIdentifier + "RotX";
      this._rot_y = this.ConfigurableIdentifier + "RotY";
      this._rot_z = this.ConfigurableIdentifier + "RotZ";
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
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._dir_x);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._dir_y);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._dir_z);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._rot_x);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._rot_y);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._rot_z);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.ParentEnvironment.TransformPosition(position : this.transform.position);
      var dir = this.ParentEnvironment.TransformDirection(direction : this.transform.forward);
      var rot = this.ParentEnvironment.TransformDirection(direction : this.transform.up);
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
        print(message : "Applying " + v + " To " + this.ConfigurableIdentifier);
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
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(
                  newX : v - dir.x,
                  newY : dir.y,
                  newZ : dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(
                  newX : dir.x,
                  newY : v - dir.y,
                  newZ : dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(
                  newX : dir.x,
                  newY : dir.y,
                  newZ : v - dir.z);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(
                  newX : v - rot.x,
                  newY : rot.y,
                  newZ : rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(
                  newX : rot.x,
                  newY : v - rot.y,
                  newZ : rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(
                  newX : rot.x,
                  newY : rot.y,
                  newZ : v - rot.z);
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
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(
                  newX : v,
                  newY : dir.y,
                  newZ : dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(
                  newX : dir.x,
                  newY : v,
                  newZ : dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(
                  newX : dir.x,
                  newY : dir.y,
                  newZ : v);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(
                  newX : v,
                  newY : rot.y,
                  newZ : rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(
                  newX : rot.x,
                  newY : v,
                  newZ : rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(
                  newX : rot.x,
                  newY : rot.y,
                  newZ : v);
      }

      var inv_pos = this.ParentEnvironment.InverseTransformPosition(position : pos);
      var inv_dir = this.ParentEnvironment.InverseTransformDirection(direction : dir);
      var inv_rot = this.ParentEnvironment.InverseTransformDirection(direction : rot);
      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(
                                                        forward : inv_dir,
                                                        upwards : inv_rot);
    }
  }
}
