
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [System.Serializable]
  public class Reaction {
    private readonly ReactionParameters _parameters = new ReactionParameters();

    private readonly Unobservables _unobservables = new Unobservables(
                                                                      null,
                                                                      new Pose[] { });

    public override string ToString() {
      var motions_str = "";
      if (Motions != null)
        foreach (var motion in Motions)
          motions_str += motion + "\n";
      var configurations_str = "";
      if (Configurations != null)
        foreach (var configuration in Configurations)
          configurations_str += configuration + "\n";
      return string.Format(
                           "<Reaction>\n " + "{0},{1},{2},{3}" + "\n</Reaction>",
                           Parameters,
                           motions_str,
                           configurations_str,
                           Unobservables);
    }

    #region Constructors

    public Reaction(
      ReactionParameters parameters,
      MotorMotion[] motions,
      Configuration[] configurations,
      Unobservables unobservables) {
      _parameters = parameters;
      Motions = motions;
      Configurations = configurations;
      _unobservables = unobservables;
    }

    public Reaction() { _parameters.IsExternal = false; }

    #endregion

    #region Getters

    public MotorMotion[] Motions { get; private set; }

    public Configuration[] Configurations { get; private set; }

    public ReactionParameters Parameters { get { return _parameters; } }

    public Unobservables Unobservables { get { return _unobservables; } }

    #endregion
  }
}
