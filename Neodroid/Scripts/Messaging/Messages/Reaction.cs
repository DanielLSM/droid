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
    bool _step = true;
    bool _configure = false;

    public Reaction (MotorMotion[] motions) {
      _motions = motions;
      _reset = false;
      _step = true;
      _configure = false;
    }

    public Reaction (MotorMotion[] motions,
                     Pose[] poses,
                     Body[] bodies,
                     Configuration[] configurations,
                     bool reset,
                     bool step = true,
                     bool configure = false) {
      _motions = motions;
      _configurations = configurations;
      _poses = poses;
      _bodies = bodies;
      _reset = reset; 
      _step = step;
      _configure = configure;
    }

    public Reaction (Configuration[] configurations) {
      _configurations = configurations;
      _reset = true; 
      _step = true;
      _configure = false;
    }


    public Reaction (bool reset, bool step = true, bool configure = false) {
      _reset = reset;
      _step = step;
      _configure = configure;
    }

    public MotorMotion[] Motions {
      get { return _motions; }
    }

    public bool Reset {
      get { return _reset; }
    }

    public bool Step {
      get { return _step; }
    }

    public bool Configure {
      get { return _configure; }
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
      return String.Format (
        "<Reaction>\n " +
        "{0},{1},{2},{3},{4}" +
        "\n</Reaction>", _reset, _step, _configure, motions_str, configurations_str);
    }
  }
}
