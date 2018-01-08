using Neodroid.Messaging.Messages;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Managers;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  [RequireComponent( typeof(SimulationManager))]
  public class SimulationConfigurable : ConfigurableGameObject {
    string _fullscreen;
    string _height;

    string _quality_level;
    string _target_frame_rate;
    string _time_scale;
    string _width;

    public override string ConfigurableIdentifier { get { return this.name + "Simulation"; } }

    protected override void AddToEnvironment() {
      this._quality_level = this.ConfigurableIdentifier + "QualityLevel";
      this._target_frame_rate = this.ConfigurableIdentifier + "TargetFrameRate";
      this._time_scale = this.ConfigurableIdentifier + "TimeScale";
      this._width = this.ConfigurableIdentifier + "Width";
      this._height = this.ConfigurableIdentifier + "Height";
      this._fullscreen = this.ConfigurableIdentifier + "Fullscreen";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                             r : this.ParentEnvironment,
                                                                             c : (ConfigurableGameObject)this,
                                                                             identifier : this
                                                                               ._quality_level);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                             r : this.ParentEnvironment,
                                                                             c : (ConfigurableGameObject)this,
                                                                             identifier : this
                                                                               ._target_frame_rate);
      this.ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      r : this.ParentEnvironment,
                                                      c : (ConfigurableGameObject)this,
                                                      identifier : this._width);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                             r : this.ParentEnvironment,
                                                                             c : (ConfigurableGameObject)this,
                                                                             identifier : this._height);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                             r : this.ParentEnvironment,
                                                                             c : (ConfigurableGameObject)this,
                                                                             identifier : this._fullscreen);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                             r : this.ParentEnvironment,
                                                                             c : (ConfigurableGameObject)this,
                                                                             identifier : this._time_scale);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (this.Debugging)
        print(message : "Applying " + configuration + " To " + this.ConfigurableIdentifier);

      if (configuration.ConfigurableName == this._quality_level)
        QualitySettings.SetQualityLevel(
                                        index : (int)configuration.ConfigurableValue,
                                        applyExpensiveChanges : true);
      else if (configuration.ConfigurableName == this._target_frame_rate)
        Application.targetFrameRate = (int)configuration.ConfigurableValue;
      else if (configuration.ConfigurableName == this._width)
        Screen.SetResolution(
                             width : (int)configuration.ConfigurableValue,
                             height : Screen.height,
                             fullscreen : false);
      else if (configuration.ConfigurableName == this._height)
        Screen.SetResolution(
                             width : Screen.width,
                             height : (int)configuration.ConfigurableValue,
                             fullscreen : false);
      else if (configuration.ConfigurableName == this._fullscreen)
        Screen.SetResolution(
                             width : Screen.width,
                             height : Screen.height,
                             fullscreen : (int)configuration.ConfigurableValue != 0);
      else if (configuration.ConfigurableName == this._time_scale)
        Time.timeScale = configuration.ConfigurableValue;
    }
  }
}
