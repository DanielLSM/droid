using System;
using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurables {

  [RequireComponent (typeof(Renderer))]
  public class ColorConfigurable : ConfigurableGameObject {

    Renderer _renderer;

    protected override void Start () {
      _renderer = GetComponent<Renderer> ();
    }

    string _R;
    string _G;
    string _B;
    string _A;

    protected override void AddToEnvironment () {
      _R = ConfigurableIdentifier + "R";
      _G = ConfigurableIdentifier + "G";
      _B = ConfigurableIdentifier + "B";
      _A = ConfigurableIdentifier + "A";
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _R);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _G);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _B);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent (ParentEnvironment, (ConfigurableGameObject)this, _A);
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (Debugging)
        print ("Applying " + configuration.ToString () + " To " + ConfigurableIdentifier);
      foreach (var mat in _renderer.materials) {
        var c = mat.color;

        if (configuration.ConfigurableName == _R) {
          c.r = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == _G) {
          c.g = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == _B) {
          c.b = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == _A) {
          c.a = configuration.ConfigurableValue;
        }

        mat.color = c;
      }
    }

    public override string ConfigurableIdentifier { get { return name + "Color"; } }
  }
}

