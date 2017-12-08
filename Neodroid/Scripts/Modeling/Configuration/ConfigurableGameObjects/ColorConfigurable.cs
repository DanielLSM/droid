using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations
{

    [RequireComponent(typeof(Renderer))]
    public class ColorConfigurable : ConfigurableGameObject
    {

        Renderer _renderer;

        protected override void Start()
        {
            AddToEnvironment();
            _renderer = GetComponent<Renderer>();
        }

        string _R;
        string _G;
        string _B;
        string _A;

        protected override void AddToEnvironment()
        {
            _R = GetConfigurableIdentifier() + "R";
            _G = GetConfigurableIdentifier() + "G";
            _B = GetConfigurableIdentifier() + "B";
            _A = GetConfigurableIdentifier() + "A";
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _R);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _G);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _B);
            _environment = NeodroidUtilities.MaybeRegisterNamedComponent(_environment, (ConfigurableGameObject)this, _A);
        }

        public override void ApplyConfiguration(Configuration configuration)
        {
            if (_debug)
                Debug.Log("Applying " + configuration.ToString() + " To " + GetConfigurableIdentifier());
            foreach (var mat in _renderer.materials)
            {
                var c = mat.color;

                if (configuration.ConfigurableName == _R)
                {
                    c.r = configuration.ConfigurableValue;
                }
                else if (configuration.ConfigurableName == _G)
                {
                    c.g = configuration.ConfigurableValue;
                }
                else if (configuration.ConfigurableName == _B)
                {
                    c.b = configuration.ConfigurableValue;
                }
                else if (configuration.ConfigurableName == _A)
                {
                    c.a = configuration.ConfigurableValue;
                }

                mat.color = c;
            }
        }

        public override string GetConfigurableIdentifier()
        {
            return name + "Color";
        }
    }
}

