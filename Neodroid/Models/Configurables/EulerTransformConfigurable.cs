using System;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
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

    [Header("Observation", order = 103)]
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
      this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
      this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
      this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
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
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_z);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_z);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      var dir = this.ParentEnvironment.TransformDirection(this.transform.forward);
      var rot = this.ParentEnvironment.TransformDirection(this.transform.up);
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
        if (configuration.ConfigurableName == this._x)
          pos.Set(v - pos.x, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v - pos.y, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v - pos.z);
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(v - dir.x, dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(dir.x, v - dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(dir.x, dir.y, v - dir.z);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(v - rot.x, rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(rot.x, v - rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(rot.x, rot.y, v - rot.z);
      } else {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v);
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(v, dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(dir.x, v, dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(dir.x, dir.y, v);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(v, rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(rot.x, v, rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(rot.x, rot.y, v);
      }

      var inv_pos = this.ParentEnvironment.InverseTransformPosition(pos);
      var inv_dir = this.ParentEnvironment.InverseTransformDirection(dir);
      var inv_rot = this.ParentEnvironment.InverseTransformDirection(rot);
      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }
  }
}
