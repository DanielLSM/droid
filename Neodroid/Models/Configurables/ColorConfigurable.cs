using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  [RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : ConfigurableGameObject {
    string _a;
    string _b;
    string _g;

    string _r;

    Renderer _renderer;

    public override string ConfigurableIdentifier { get { return this.name + "Color"; } }

    protected override void Start() { this._renderer = this.GetComponent<Renderer>(); }

    protected override void AddToEnvironment() {
      this._r = this.ConfigurableIdentifier + "R";
      this._g = this.ConfigurableIdentifier + "G";
      this._b = this.ConfigurableIdentifier + "B";
      this._a = this.ConfigurableIdentifier + "A";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._r);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._g);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._b);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._a);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (this.Debugging)
        print("Applying " + configuration + " To " + this.ConfigurableIdentifier);
      foreach (var mat in this._renderer.materials) {
        var c = mat.color;

        if (configuration.ConfigurableName == this._r)
          c.r = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._g)
          c.g = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._b)
          c.b = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._a)
          c.a = configuration.ConfigurableValue;

        mat.color = c;
      }
    }
  }
}
