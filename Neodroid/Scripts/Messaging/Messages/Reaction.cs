
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Messaging.Messages {

  [System.Serializable]
  public class Reaction {
    MotorMotion[] _motions;
    Configuration[] _configurations;
    Pose[] _poses;
    Body[] _bodies;
    ReactionParameters _parameters = new ReactionParameters ();


    #region Constructors

    public Reaction (ReactionParameters parameters, MotorMotion[] motions, Configuration[] configurations, Pose[] poses, Body[] bodies) {
      _parameters = parameters;
      _motions = motions;
      _configurations = configurations;
      _poses = poses;
      _bodies = bodies;
    }

    #endregion

    #region Getters

    public MotorMotion[] Motions {
      get { return _motions; }
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

    public ReactionParameters Parameters {
      get { return _parameters; }
    }

    #endregion

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
      return System.String.Format (
        "<Reaction>\n " +
        "{0},{1},{2}" +
        "\n</Reaction>", Parameters.ToString (), motions_str, configurations_str);
    }
  }
}
