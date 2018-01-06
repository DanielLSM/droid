
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Messaging.Messages {

  [System.Serializable]
  public class Reaction {
    MotorMotion[] _motions;
    Configuration[] _configurations;
    Unobservables _unobservables = new Unobservables (null, new Pose[] { });
    ReactionParameters _parameters = new ReactionParameters ();


    #region Constructors

    public Reaction (ReactionParameters parameters, MotorMotion[] motions, Configuration[] configurations, Unobservables unobservables) {
      _parameters = parameters;
      _motions = motions;
      _configurations = configurations;
      _unobservables = unobservables;
    }

    public Reaction () {
      _parameters.BeforeObservation = false;
    }

    #endregion

    #region Getters

    public MotorMotion[] Motions {
      get { return _motions; }
    }

    public Configuration[] Configurations {
      get { return _configurations; }
    }

    public ReactionParameters Parameters {
      get { return _parameters; }
    }

    public Unobservables Unobservables {
      get {
        return _unobservables;
      }
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
        "{0},{1},{2},{3}" +
        "\n</Reaction>", Parameters.ToString (), motions_str, configurations_str, Unobservables.ToString ());
    }
  }
}
