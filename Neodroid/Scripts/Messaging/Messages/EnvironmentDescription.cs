using System.Collections.Generic;
using Assets.Neodroid.Models.Actors;
using Neodroid.Configurables;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {
    public EnvironmentDescription(
      int max_steps,
      int frame_skips,
      Dictionary<string, Actor> actors,
      Dictionary<string, ConfigurableGameObject> configurables,
      float solved_threshold) {
      Configurables = configurables;
      Actors = actors;
      MaxSteps = max_steps;
      FrameSkips = frame_skips;
      SolvedThreshold = solved_threshold;
    }

    public Dictionary<string, Actor> Actors { get; private set; }

    public Dictionary<string, ConfigurableGameObject> Configurables { get; private set; }

    public int MaxSteps { get; private set; }

    public int FrameSkips { get; private set; }

    public float SolvedThreshold { get; private set; }
  }
}
