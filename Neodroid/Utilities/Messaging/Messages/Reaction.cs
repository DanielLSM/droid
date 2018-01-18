using System;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.Messages {
  [Serializable]
  public class Reaction {
    readonly ReactionParameters _parameters = new ReactionParameters();

    readonly Unobservables _unobservables = new Unobservables(null, new Pose[] { });

    public override string ToString() {
      var motions_str = "";
      if (this.Motions != null) {
        foreach (var motion in this.Motions)
          motions_str += motion + "\n";
      }

      var configurations_str = "";
      if (this.Configurations != null) {
        foreach (var configuration in this.Configurations)
          configurations_str += configuration + "\n";
      }

      return string.Format(
          "<Reaction>\n " + "{0},{1},{2},{3}" + "\n</Reaction>",
          this.Parameters,
          motions_str,
          configurations_str,
          this.Unobservables);
    }

    #region Constructors

    public Reaction(
        ReactionParameters parameters,
        MotorMotion[] motions,
        Configuration[] configurations,
        Unobservables unobservables) {
      this._parameters = parameters;
      this.Motions = motions;
      this.Configurations = configurations;
      this._unobservables = unobservables;
    }

    public Reaction() { this._parameters.IsExternal = false; }

    #endregion

    #region Getters

    public MotorMotion[] Motions { get; private set; }

    public Configuration[] Configurations { get; private set; }

    public ReactionParameters Parameters { get { return this._parameters; } }

    public Unobservables Unobservables { get { return this._unobservables; } }

    #endregion
  }
}
