using Neodroid.Models.Environments;
using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Environments {
  public class RandomisedEnvironment : PrototypingEnvironment {
    readonly Random _random_generator = new Random();

    void RandomiseEnvironment() {
      foreach (var configurable in this._configurables) {
        var valid_range = configurable.Value.ConfigurableValueSpace;
        float value = this._random_generator.Next((int)valid_range.MinValue, (int)valid_range.MaxValue);
        configurable.Value.ApplyConfiguration(new Configuration(configurable.Key, Mathf.Round(value)));
      }
    }

    protected override void InnerPreStart() {
      base.InnerPreStart();
      this.RandomiseEnvironment();
    }

    public override void PostStep() {
      if (this._terminated) {
        this._terminated = false;
        this.Reset();

        this.RandomiseEnvironment();
      }

      if (this._configure) {
        this._configure = false;
        this.Configure();
      }

      this.UpdateConfigurableValues();
      this.UpdateObserversData();
    }
  }
}
