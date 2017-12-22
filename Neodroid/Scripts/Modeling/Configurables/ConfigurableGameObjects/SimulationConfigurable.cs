using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using System;

namespace Neodroid.Configurables {
  [RequireComponent (typeof(SimulationManager))]
  public class SimulationConfigurable : ConfigurableGameObject {

    string _quality_level;
    string _target_frame_rate;
    string _width;
    string _height;
    string _fullscreen;
    string _time_scale;

    protected override void AddToEnvironment () {
      _quality_level = ConfigurableIdentifier + "QualityLevel";
      _target_frame_rate = ConfigurableIdentifier + "TargetFrameRate";
      _time_scale = ConfigurableIdentifier + "TimeScale";
      _width = ConfigurableIdentifier + "Width";
      _height = ConfigurableIdentifier + "Height";
      _fullscreen = ConfigurableIdentifier + "Fullscreen";
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _quality_level);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _target_frame_rate);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _width);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _height);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _fullscreen);
      _environment = NeodroidUtilities.MaybeRegisterNamedComponent (_environment, (ConfigurableGameObject)this, _time_scale);
    }

    public override void ApplyConfiguration (Configuration configuration) {
      if (_debug)
        Debug.Log ("Applying " + configuration.ToString () + " To " + ConfigurableIdentifier);

      if (configuration.ConfigurableName == _quality_level) {
        QualitySettings.SetQualityLevel ((int)(configuration.ConfigurableValue), true);
      } else if (configuration.ConfigurableName == _target_frame_rate) {
        Application.targetFrameRate = (int)(configuration.ConfigurableValue);
      } else if (configuration.ConfigurableName == _width) {
        Screen.SetResolution ((int)(configuration.ConfigurableValue), Screen.height, false);
      } else if (configuration.ConfigurableName == _height) {
        Screen.SetResolution (Screen.width, (int)(configuration.ConfigurableValue), false);
      } else if (configuration.ConfigurableName == _fullscreen) {
        if ((int)(configuration.ConfigurableValue) != 0)
          Screen.SetResolution (Screen.width, Screen.height, true);
        else
          Screen.SetResolution (Screen.width, Screen.height, false);
      } else if (configuration.ConfigurableName == _time_scale) {
        Time.timeScale = configuration.ConfigurableValue;
      }
    }

    public override string ConfigurableIdentifier {
      get {
        return name + "Simulation";
      }
    }
  }
}
