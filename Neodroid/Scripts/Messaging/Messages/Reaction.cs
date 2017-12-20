using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Messaging.Messages {

  [Serializable]
  public class Reaction {
    MotorMotion[] _motions;
    Configuration[] _configurations;
    Pose[] _poses;
    Body[] _bodies;
    bool _reset = false;

    public Reaction (MotorMotion[] motions) {
      _motions = motions;
      _reset = false;
    }

    public Reaction (MotorMotion[] motions,
                     Pose[] poses,
                     Body[] bodies,
                     Configuration[] configurations,
                     bool reset) {
      _motions = motions;
      _configurations = configurations;
      _poses = poses;
      _bodies = bodies;
      _reset = reset; 
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

    public Pose[] Poses {
      get { return _poses; }
    }

    public Body[] Bodies {
      get { return _bodies; }
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
      return "<Reaction>\n" + _reset + ",\n " + motions_str + ",\n " + configurations_str + "\n</Reaction>";
    }
  }
}
