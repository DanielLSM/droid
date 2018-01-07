using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Configurables {
  public class TriTransformConfigurable : SingleEulerTransformConfigurable {
    [Header (
      "Observation",
      order = 103)]
    [SerializeField]
    private Vector3 _position;

    private string _X;
    private string _Y;
    private string _Z;

    public Vector3 Position { get { return _position; } }

    public override string ConfigurableIdentifier { get { return name + "Transform"; } }

    protected override void AddToEnvironment () {
      _X = ConfigurableIdentifier + "X";
      _Y = ConfigurableIdentifier + "Y";
      _Z = ConfigurableIdentifier + "Z";
      ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent (
        ParentEnvironment,
        (ConfigurableGameObject)this,
        _X);
      ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent (
        ParentEnvironment,
        (ConfigurableGameObject)this,
        _Y);
      ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent (
        ParentEnvironment,
        (ConfigurableGameObject)this,
        _Z);
    }

    public override void UpdateObservation () {
      _position = ParentEnvironment.TransformPosition (transform.position);
    }

    public override void ApplyConfiguration (Configuration configuration) {
      var pos = ParentEnvironment.TransformPosition (transform.position);
      var v = configuration.ConfigurableValue;
      if (ValidInput.decimal_granularity >= 0)
        v = (int)System.Math.Round (
          v,
          ValidInput.decimal_granularity);
      if (ValidInput.min_value.CompareTo (ValidInput.max_value) != 0)
      if (v < ValidInput.min_value || v > ValidInput.max_value) {
        print (
          string.Format (
            "Configurable does not accept input{2}, outside allowed range {0} to {1}",
            ValidInput.min_value,
            ValidInput.max_value,
            v));
        return; // Do nothing
      }

      if (Debugging)
        print (
          string.Format (
            "Applying {0} to {1} configurable",
            v,
            configuration.ConfigurableName));
      if (RelativeToExistingValue) {
        if (configuration.ConfigurableName == _X)
          pos.Set (
            v - pos.x,
            pos.y,
            pos.z);
        else if (configuration.ConfigurableName == _Y)
          pos.Set (
            pos.x,
            v - pos.y,
            pos.z);
        else if (configuration.ConfigurableName == _Z)
          pos.Set (
            pos.x,
            pos.y,
            v - pos.z);
      } else {
        if (configuration.ConfigurableName == _X)
          pos.Set (
            v,
            pos.y,
            pos.z);
        else if (configuration.ConfigurableName == _Y)
          pos.Set (
            pos.x,
            v,
            pos.z);
        else if (configuration.ConfigurableName == _Z)
          pos.Set (
            pos.x,
            pos.y,
            v);
      }

      var inv_pos = ParentEnvironment.InverseTransformPosition (pos);
      transform.position = inv_pos;
    }
  }
}
