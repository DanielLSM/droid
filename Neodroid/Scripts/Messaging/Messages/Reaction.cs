using System;
using System.Collections.Generic;

namespace Neodroid.Messaging.Messages {

  [Serializable]
  public class Reaction {
    MotorMotion[] _motions;
    Configuration[] _configurations;
    bool _reset = false;

    public Reaction (MotorMotion[] motions) {
      _motions = motions;
      _reset = false;
    }

    public Reaction (Configuration[] configurations) {
      _configurations = configurations;
      _reset = true;
    }

    public Reaction (bool reset) {
      _reset = reset;
    }

    public MotorMotion[] Motions {
      get { return _motions; }
    }

    public bool Reset {
      get { return _reset; }
    }

    public Configuration[] Configurations {
      get { return _configurations; }
    }

    public override string ToString () {
      string motions_str = "";
      if (Motions != null) {
        foreach (MotorMotion motion in Motions) {
          motions_str += motion.ToString () + "\n";
        }
      }
      string configurations_str = "";
      if (Configurations != null) {
        foreach (Configuration configuration in Configurations) {
          configurations_str += configuration.ToString () + "\n";
        }
      }
      return "<Reaction> " + _reset + ",\n " + motions_str + ",\n " + configurations_str + "</Reaction>";
    }
  }
}
