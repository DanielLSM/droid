using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurables {
  public class EulerTransformConfigurable : SingleEulerTransformConfigurable, HasEulerTransformProperties {
    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Vector3 _rotation;
    [SerializeField]
    Vector3 _direction;

    public Vector3 Position {
      get {
        return _position;
      }
      set {
        _position = value;
      }
    }

    public Vector3 Direction {
      get {
        return _direction;
      }

      set {
        _direction = value;
      }
    }

    public Vector3 Rotation {
      get {
        return _rotation;
      }

      set {
        _rotation = value;
      }
    }


    string _X;
    string _Y;
    string _Z;
    string _DirX;
    string _DirY;
    string _DirZ;
    string _RotX;
    string _RotY;
    string _RotZ;

    public override void UpdateObservation () {
      Position = ParentEnvironment.TransformPosition (this.transform.position);
      Direction = ParentEnvironment.TransformDirection (this.transform.forward);
      Rotation = ParentEnvironment.TransformDirection (this.transform.up);
    }


    protected override void AddToEnvironment () {
      _X = ConfigurableIdentifier + "X";
      _Y = ConfigurableIdentifier + "Y";
      _Z = ConfigurableIdentifier + "Z";
      _DirX = ConfigurableIdentifier + "DirX";
      _DirY = ConfigurableIdentifier + "DirY";
      _DirZ = ConfigurableIdentifier + "DirZ";
      _RotX = ConfigurableIdentifier + "RotX";
      _RotY = ConfigurableIdentifier + "RotY";
      _RotZ = ConfigurableIdentifier + "RotZ";
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _X);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _Y);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _Z);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _DirX);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _DirY);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _DirZ);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _RotX);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _RotY);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _RotZ);
    }

    public override void ApplyConfiguration (Configuration configuration) {
      var pos = ParentEnvironment.TransformPosition (this.transform.position);
      var dir = ParentEnvironment.TransformDirection (this.transform.forward);
      var rot = ParentEnvironment.TransformDirection (this.transform.up);
      var v = configuration.ConfigurableValue;
      if (ValidInput.decimal_granularity >= 0) {
        v = (int)System.Math.Round (v, ValidInput.decimal_granularity);
      }
      if (ValidInput.min_value.CompareTo (ValidInput.max_value) != 0) {
        if (v < ValidInput.min_value || v > ValidInput.max_value) {
          print (System.String.Format ("Configurable does not accept input{2}, outside allowed range {0} to {1}", ValidInput.min_value, ValidInput.max_value, v));
          return; // Do nothing
        }
      }
      if (Debugging)
        print ("Applying " + v.ToString () + " To " + ConfigurableIdentifier);
      if (RelativeToExistingValue) {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v - pos.x, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v - pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v - pos.z);
        } else if (configuration.ConfigurableName == _DirX) {
          dir.Set (v - dir.x, dir.y, dir.z);
        } else if (configuration.ConfigurableName == _DirY) {
          dir.Set (dir.x, v - dir.y, dir.z);
        } else if (configuration.ConfigurableName == _DirZ) {
          dir.Set (dir.x, dir.y, v - dir.z);
        } else if (configuration.ConfigurableName == _RotX) {
          rot.Set (v - rot.x, rot.y, rot.z);
        } else if (configuration.ConfigurableName == _RotY) {
          rot.Set (rot.x, v - rot.y, rot.z);
        } else if (configuration.ConfigurableName == _RotZ) {
          rot.Set (rot.x, rot.y, v - rot.z);
        }
      } else {
        if (configuration.ConfigurableName == _X) {
          pos.Set (v, pos.y, pos.z);
        } else if (configuration.ConfigurableName == _Y) {
          pos.Set (pos.x, v, pos.z);
        } else if (configuration.ConfigurableName == _Z) {
          pos.Set (pos.x, pos.y, v);
        } else if (configuration.ConfigurableName == _DirX) {
          dir.Set (v, dir.y, dir.z);
        } else if (configuration.ConfigurableName == _DirY) {
          dir.Set (dir.x, v, dir.z);
        } else if (configuration.ConfigurableName == _DirZ) {
          dir.Set (dir.x, dir.y, v);
        } else if (configuration.ConfigurableName == _RotX) {
          rot.Set (v, rot.y, rot.z);
        } else if (configuration.ConfigurableName == _RotY) {
          rot.Set (rot.x, v, rot.z);
        } else if (configuration.ConfigurableName == _RotZ) {
          rot.Set (rot.x, rot.y, v);
        }

      }
      var inv_pos = ParentEnvironment.InverseTransformPosition (pos);
      var inv_dir = ParentEnvironment.InverseTransformDirection (dir);
      var inv_rot = ParentEnvironment.InverseTransformDirection (rot);
      transform.position = inv_pos;
      transform.rotation = Quaternion.identity;
      transform.rotation = Quaternion.LookRotation (inv_dir, inv_rot); 
    }

    public override string ConfigurableIdentifier{ get { return name + "Transform"; } }
  }
}