
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurables {
  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyConfigurable : ConfigurableGameObject, HasRigidbodyProperties {
    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _velocity;
    [SerializeField]
    Vector3 _angular_velocity;

    Rigidbody _rigidbody;

    public Vector3 Velocity {
      get {
        return _velocity;
      }
      set {
        _velocity = value;
      }
    }

    public Vector3 AngularVelocity {
      get {
        return _angular_velocity;
      }
      set {
        _angular_velocity = value;
      }
    }

    public override void UpdateObservation () {
      Velocity = _rigidbody.velocity;
      AngularVelocity = _rigidbody.angularVelocity;
    }

    protected override void Start () {
      _rigidbody = GetComponent<Rigidbody> ();
      UpdateObservation ();
    }


    string _VelX;
    string _VelY;
    string _VelZ;
    string _AngX;
    string _AngY;
    string _AngZ;

    protected override void AddToEnvironment () {
      _VelX = ConfigurableIdentifier + "VelX";
      _VelY = ConfigurableIdentifier + "VelY";
      _VelZ = ConfigurableIdentifier + "VelZ";
      _AngX = ConfigurableIdentifier + "AngX";
      _AngY = ConfigurableIdentifier + "AngY";
      _AngZ = ConfigurableIdentifier + "AngZ";
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _VelX);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _VelY);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _VelZ);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _AngX);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _AngY);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _AngZ);
    }


    public override void ApplyConfiguration (Configuration configuration) {
      var vel = _rigidbody.velocity;
      var ang = _rigidbody.velocity;

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
        if (configuration.ConfigurableName == _VelX) {
          vel.Set (v - vel.x, vel.y, vel.z);
        } else if (configuration.ConfigurableName == _VelY) {
          vel.Set (vel.x, v - vel.y, vel.z);
        } else if (configuration.ConfigurableName == _VelZ) {
          vel.Set (vel.x, vel.y, v - vel.z);
        } else if (configuration.ConfigurableName == _AngX) {
          ang.Set (v - ang.x, ang.y, ang.z);
        } else if (configuration.ConfigurableName == _AngY) {
          ang.Set (ang.x, v - ang.y, ang.z);
        } else if (configuration.ConfigurableName == _AngZ) {
          ang.Set (ang.x, ang.y, v - ang.z);
        }
      } else {
        if (configuration.ConfigurableName == _VelX) {
          vel.Set (v, vel.y, vel.z);
        } else if (configuration.ConfigurableName == _VelY) {
          vel.Set (vel.x, v, vel.z);
        } else if (configuration.ConfigurableName == _VelZ) {
          vel.Set (vel.x, vel.y, v);
        } else if (configuration.ConfigurableName == _AngX) {
          ang.Set (v, ang.y, ang.z);
        } else if (configuration.ConfigurableName == _AngY) {
          ang.Set (ang.x, v, ang.z);
        } else if (configuration.ConfigurableName == _AngZ) {
          ang.Set (ang.x, ang.y, v);
        }
      }
      _rigidbody.velocity = vel;
      _rigidbody.angularVelocity = ang;
    }



    public override string ConfigurableIdentifier {
      get {
        {
          return name + "Rigidbody";
        }
      
      }
    }
  }
}
