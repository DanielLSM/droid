using System.Collections.Generic;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {
    public EnvironmentDescription(
      int max_steps,
      int frame_skips,
      Dictionary<string, Actor> actors,
      Dictionary<string, ConfigurableGameObject> configurables,
      float solved_threshold) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.MaxSteps = max_steps;
      this.FrameSkips = frame_skips;
      this.SolvedThreshold = solved_threshold;
    }

    public Dictionary<string, Actor> Actors { get; private set; }

    public Dictionary<string, ConfigurableGameObject> Configurables { get; private set; }

    public int MaxSteps { get; private set; }

    public int FrameSkips { get; private set; }

    public float SolvedThreshold { get; private set; }
  }
}
