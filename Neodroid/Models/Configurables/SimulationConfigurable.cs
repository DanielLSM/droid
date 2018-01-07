using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Configurables {
  [RequireComponent(typeof(SimulationManager))]
  public class SimulationConfigurable : ConfigurableGameObject {
    private string _fullscreen;
    private string _height;

    private string _quality_level;
    private string _target_frame_rate;
    private string _time_scale;
    private string _width;

    public override string ConfigurableIdentifier { get { return name + "Simulation"; } }

    protected override void AddToEnvironment() {
      _quality_level = ConfigurableIdentifier + "QualityLevel";
      _target_frame_rate = ConfigurableIdentifier + "TargetFrameRate";
      _time_scale = ConfigurableIdentifier + "TimeScale";
      _width = ConfigurableIdentifier + "Width";
      _height = ConfigurableIdentifier + "Height";
      _fullscreen = ConfigurableIdentifier + "Fullscreen";
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                        ParentEnvironment,
                                                                        (ConfigurableGameObject)this,
                                                                        _quality_level);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                        ParentEnvironment,
                                                                        (ConfigurableGameObject)this,
                                                                        _target_frame_rate);
      ParentEnvironment =
        NeodroidUtilities.MaybeRegisterNamedComponent(
                                                      ParentEnvironment,
                                                      (ConfigurableGameObject)this,
                                                      _width);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                        ParentEnvironment,
                                                                        (ConfigurableGameObject)this,
                                                                        _height);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                        ParentEnvironment,
                                                                        (ConfigurableGameObject)this,
                                                                        _fullscreen);
      ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                        ParentEnvironment,
                                                                        (ConfigurableGameObject)this,
                                                                        _time_scale);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (Debugging)
        print("Applying " + configuration + " To " + ConfigurableIdentifier);

      if (configuration.ConfigurableName == _quality_level)
        QualitySettings.SetQualityLevel(
                                        (int)configuration.ConfigurableValue,
                                        true);
      else if (configuration.ConfigurableName == _target_frame_rate)
        Application.targetFrameRate = (int)configuration.ConfigurableValue;
      else if (configuration.ConfigurableName == _width)
        Screen.SetResolution(
                             (int)configuration.ConfigurableValue,
                             Screen.height,
                             false);
      else if (configuration.ConfigurableName == _height)
        Screen.SetResolution(
                             Screen.width,
                             (int)configuration.ConfigurableValue,
                             false);
      else if (configuration.ConfigurableName == _fullscreen)
        Screen.SetResolution(
                             Screen.width,
                             Screen.height,
                             (int)configuration.ConfigurableValue != 0);
      else if (configuration.ConfigurableName == _time_scale)
        Time.timeScale = configuration.ConfigurableValue;
    }
  }
}
