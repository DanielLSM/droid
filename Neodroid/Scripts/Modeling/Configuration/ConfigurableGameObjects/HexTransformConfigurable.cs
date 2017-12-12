using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations
{
	public class HexTransformConfigurable : TransformConfigurable
    {
        string _X;
        string _Y;
        string _Z;
        string _RotX;
        string _RotY;
        string _RotZ;

        protected override void AddToEnvironment()
        {
            _X = GetConfigurableIdentifier() + "X";
            _Y = GetConfigurableIdentifier() + "Y";
            _Z = GetConfigurableIdentifier() + "Z";
            _RotX = GetConfigurableIdentifier() + "RotX";
            _RotY = GetConfigurableIdentifier() + "RotY";
            _RotZ = GetConfigurableIdentifier() + "RotZ";
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _X);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _Y);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _Z);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _RotX);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _RotY);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _RotZ);
        }

        public override void ApplyConfiguration(Configuration configuration)
        {
            if (_debug)
                Debug.Log("Applying " + configuration.ToString() + " To " + GetConfigurableIdentifier());
            var pos = _environment.TransformPosition(this.transform.position);
            var dir = _environment.TransformDirection(this.transform.forward);
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
            else if (configuration.ConfigurableName == _RotX)
            {
                dir.Set(configuration.ConfigurableValue, dir.y, dir.z);
            }
            else if (configuration.ConfigurableName == _RotY)
            {
                dir.Set(dir.x, configuration.ConfigurableValue, dir.z);
            }
            else if (configuration.ConfigurableName == _RotZ)
            {
                dir.Set(dir.x, dir.y, configuration.ConfigurableValue);
            }
            var inv_pos = _environment.InverseTransformPosition(pos);
            var inv_dir = _environment.InverseTransformDirection(dir);
            transform.position = inv_pos;
            transform.rotation = Quaternion.identity;
            transform.Rotate(inv_dir);
        }

        public override string GetConfigurableIdentifier()
        {
            return name + "Transform";
        }
    }
}