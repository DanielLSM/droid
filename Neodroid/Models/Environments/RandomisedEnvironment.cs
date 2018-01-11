using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Models.Environments {
  public class RandomisedEnvironment : LearningEnvironment {
    readonly System.Random _random_generator = new System.Random ();

    void RandomiseEnvironment () {
      foreach (var configurable in this._configurables) {
        var valid_range = configurable.Value.ValidInput;
        float value = this._random_generator.Next((int)valid_range.MinValue, (int)valid_range.MaxValue);
        configurable.Value.ApplyConfiguration (new Configuration (configurable.Key, Mathf.Round(value)));
      }
    }

    protected override void InnerPreStart() {
      base.InnerPreStart();
      this.RandomiseEnvironment();
    }

    public override void PostUpdate () {
      if (this._terminated) {
        this._terminated = false;
        this.Reset ();

        this.RandomiseEnvironment ();
      }

      if (this._configure) {
        this._configure = false;
        this.Configure ();
      }

      this.UpdateConfigurableValues ();
      this.UpdateObserversData ();
    }
  }
}
