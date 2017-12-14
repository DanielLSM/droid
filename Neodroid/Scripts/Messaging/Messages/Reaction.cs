using System;
using System.Collections.Generic;

namespace Neodroid.Messaging.Messages {

  [Serializable]
  public class Reaction {
    MotorMotion[] _motions;
    Configuration[] _configurations;
    public bool _reset;

    public Reaction (MotorMotion[] motions, Configuration[] configurations, bool reset) {
      _motions = motions;
      _configurations = configurations;
      _reset = reset;
    }

    public MotorMotion[] Motions {
      get { return _motions; }
    }

    public Configuration[] Configurations {
      get { return _configurations; }
    }

    public override string ToString () {
      string motions_str = "";
      foreach (MotorMotion motion in Motions) {
        motions_str += motion.ToString () + "\n";
      }
      string configurations_str = "";
      foreach (Configuration configuration in Configurations) {
        configurations_str += configuration.ToString () + "\n";
      }
      return "<Reaction> " + _reset + ",\n " + motions_str + ",\n " + configurations_str + "</Reaction>";
    }
  }
}
