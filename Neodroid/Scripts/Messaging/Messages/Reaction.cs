using System;
using System.Collections.Generic;

namespace Neodroid.Messaging.Messages {
  
  [Serializable]
  public class Reaction {
    public MotorMotion[] _motions;
    public EnvironmentConfigurable[] _configuration;
    public bool _reset;

    public Reaction (MotorMotion[] motions, EnvironmentConfigurable[] configuration, bool reset) {
      _motions = motions;
      _configuration = configuration;
      _reset = reset;
    }

    public MotorMotion[] GetMotions () {
      return _motions;
    }

    public EnvironmentConfigurable[] EnvironmentConfiguration {
      get { return _configuration; }
    }

    public override string ToString () {
      string motions_str = "";
      foreach (MotorMotion motion in GetMotions()) {
        motions_str += motion.ToString () + "\n";
      }
      string configuration_str = "";
      foreach (EnvironmentConfigurable configurable in EnvironmentConfiguration) {
        configuration_str += configurable.ToString () + "\n";
      }
      return "<Reaction> " + _reset + ",\n " + motions_str + ",\n " + configuration_str + "</Reaction>";
    }
  }
}
