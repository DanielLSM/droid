using System;
using System.Collections.Generic;
using Neodroid.Configurations;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {

    public Dictionary<string, ConfigurableGameObject> _configurables;
    public int _max_steps;
    public int _frame_skips;
    public float _solved_threshold;

    public EnvironmentDescription (int max_steps, int frame_skips, Dictionary<string, ConfigurableGameObject> configurables, float solved_threshold) {
      _configurables = configurables;
      _max_steps = max_steps;
      _frame_skips = frame_skips;
      _solved_threshold = solved_threshold;
    }

    public EnvironmentDescription () {
    }
  }
}

