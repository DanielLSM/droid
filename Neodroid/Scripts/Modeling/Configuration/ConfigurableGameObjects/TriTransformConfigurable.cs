using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations
{

	public class TriTransformConfigurable : TransformConfigurable
    {

        string _X;
        string _Y;
        string _Z;

        protected override void AddToEnvironment()
        {
            _X = GetConfigurableIdentifier() + "X";
            _Y = GetConfigurableIdentifier() + "Y";
            _Z = GetConfigurableIdentifier() + "Z";
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _X);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _Y);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _Z);
        }

        public override void ApplyConfiguration(Configuration configuration)
        {
            if (_debug) 
                Debug.Log("Applying " + configuration.ToString() + " To " + GetConfigurableIdentifier());
            var pos = _environment.TransformPosition(this.transform.position);
            if (configuration.ConfigurableName == _X)
            {
                pos.Set(configuration.ConfigurableValue, pos.y, pos.z);
            }
            else if (configuration.ConfigurableName == _Y)
            {
                pos.Set(pos.x, configuration.ConfigurableValue, pos.z);
            }
            else if (configuration.ConfigurableName == _Z)
            {
                pos.Set(pos.x, pos.y, configuration.ConfigurableValue);
            }
            var inv_pos = _environment.InverseTransformPosition(pos);
            transform.position = inv_pos;
        }

        public override string GetConfigurableIdentifier()
        {
            return name + "Transform";
        }
    }
}