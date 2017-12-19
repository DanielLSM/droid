using System;
using System.Collections.Generic;
using Neodroid.Configurables;
using Neodroid.Actors;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {

    public Dictionary<string, Actor> _actors;

    public Dictionary<string, ConfigurableGameObject> _configurables;
    public int _max_steps;
    public int _frame_skips;
    public float _solved_threshold;

    public EnvironmentDescription (int max_steps, int frame_skips, Dictionary<string, Actor> actors, Dictionary<string, ConfigurableGameObject> configurables, float solved_threshold) {
      _configurables = configurables;
      _actors = actors;
      _max_steps = max_steps;
      _frame_skips = frame_skips;
      _solved_threshold = solved_threshold;
    }

    public EnvironmentDescription () {
    }
  }
}

