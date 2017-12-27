
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurables {
  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyConfigurable : ConfigurableGameObject {

    public Axis _axis_of_configuration;
    Rigidbody _rigidbody;


    protected override void Start () {
      _rigidbody = GetComponent<Rigidbody> ();
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (Debugging)
        print ("Applying " + configuration.ToString () + " To " + ConfigurableIdentifier);
      var velocity = _rigidbody.velocity;
      var angular_velocity = _rigidbody.velocity;
      switch (_axis_of_configuration) {
      case Axis.X:
				//pos.Set(configuration.ConfigurableValue, pos.y, pos.z);
        break;
      case Axis.Y:
				//pos.Set(pos.x, configuration.ConfigurableValue, pos.z);
        break;
      case Axis.Z:
				//pos.Set(pos.x, pos.y, configuration.ConfigurableValue);
        break;
      case Axis.RotX:
				//dir.Set(configuration.ConfigurableValue, dir.y, dir.z);
        break;
      case Axis.RotY:
				//dir.Set(dir.x, configuration.ConfigurableValue, dir.z);
        break;
      case Axis.RotZ:
				//dir.Set(dir.x, dir.y, configuration.ConfigurableValue);
        break;
      default:
        break;
      }
    }

    public override string ConfigurableIdentifier {
      get {
        {
          return name + "Rigidbody" + _axis_of_configuration.ToString ();
        }
      }
    }
  }
}
