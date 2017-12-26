using System;
using System.Collections.Generic;
using Neodroid.Configurables;
using Neodroid.Actors;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {

    Dictionary<string, Actor> _actors;

    Dictionary<string, ConfigurableGameObject> _configurables;
    int _max_steps;
    int _frame_skips;
    float _solved_threshold;

    public EnvironmentDescription (int max_steps, int frame_skips, Dictionary<string, Actor> actors, Dictionary<string, ConfigurableGameObject> configurables, float solved_threshold) {
      _configurables = configurables;
      _actors = actors;
      _max_steps = max_steps;
      _frame_skips = frame_skips;
      _solved_threshold = solved_threshold;
    }

    public Dictionary<string, Actor> Actors {
      get {
        return _actors;
      }
    }

    public Dictionary<string, ConfigurableGameObject> Configurables {
      get {
        return _configurables;
      }
    }

    public int MaxSteps {
      get {
        return _max_steps;
      }
    }

    public int FrameSkips {
      get {
        return _frame_skips;
      }
    }

    public float SolvedThreshold {
      get {
        return _solved_threshold;
      }
    }
  }
}

