
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.BoundingBoxes;
using System.Collections.Specialized;

namespace Neodroid.Configurables {
  public class EulerTransformConfigurable : ConfigurableGameObject {

    public Axis _axis_of_configuration;
    public bool _use_bounding_box_for_range = false;
    public BoundingBox _bounding_box;

    protected override void Awake () {
      AddToEnvironment ();
      if (_use_bounding_box_for_range) {
        if (_bounding_box != null) {
          var valid_input = new InputRange ();
          valid_input.max_value = System.Math.Min (_bounding_box._bounds.size.x, System.Math.Min (_bounding_box._bounds.size.y, _bounding_box._bounds.size.z));
          valid_input.min_value = -valid_input.max_value;
          ValidInput = valid_input;
        }
      }
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (configuration.ConfigurableValue < ValidInput.min_value || configuration.ConfigurableValue > ValidInput.max_value) {
        print (System.String.Format ("It does not accept input, outside allowed range {0} to {1}", ValidInput.min_value, ValidInput.max_value));
        return; // Do nothing
      }
      if (Debugging)
        print ("Applying " + configuration.ToString () + " To " + ConfigurableIdentifier);
      var pos = ParentEnvironment.TransformPosition (this.transform.position);
      var dir = ParentEnvironment.TransformDirection (this.transform.forward);
      switch (_axis_of_configuration) {
      case Axis.X:
        pos.Set (configuration.ConfigurableValue - pos.x, pos.y, pos.z);
        break;
      case Axis.Y:
        pos.Set (pos.x, configuration.ConfigurableValue - pos.y, pos.z);
        break;
      case Axis.Z:
        pos.Set (pos.x, pos.y, configuration.ConfigurableValue - pos.z);
        break;
      case Axis.RotX:
        dir.Set (configuration.ConfigurableValue - dir.x, dir.y, dir.z);
        break;
      case Axis.RotY:
        dir.Set (dir.x, configuration.ConfigurableValue - dir.y, dir.z);
        break;
      case Axis.RotZ:
        dir.Set (dir.x, dir.y, configuration.ConfigurableValue - dir.z);
        break;
      default:
        break;
      }
      var inv_pos = ParentEnvironment.InverseTransformPosition (pos);
      var inv_dir = ParentEnvironment.InverseTransformDirection (dir);
      transform.position = inv_pos;
      transform.rotation = Quaternion.identity;
      transform.Rotate (inv_dir);
    }

    public override string ConfigurableIdentifier { get { return name + "Transform" + _axis_of_configuration.ToString (); } }
  }
}

