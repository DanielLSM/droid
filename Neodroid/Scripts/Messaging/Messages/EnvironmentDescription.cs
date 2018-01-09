using System.Collections.Generic;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables.General;
using Neodroid.Scripts.Utilities.Structs;

namespace Neodroid.Scripts.Messaging.Messages {
  public class EnvironmentDescription {
    public EnvironmentDescription(
        int max_steps,
        SimulatorConfiguration simulation_configuration,
        Dictionary<string, Actor> actors,
        Dictionary<string, ConfigurableGameObject> configurables,
        float solved_threshold) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.MaxSteps = max_steps;
      this.FrameSkips = simulation_configuration.FrameSkips;
      this.SolvedThreshold = solved_threshold;
      this.APIVersion = "0.1.2";
    }

    public string APIVersion { get; set; }

    public Dictionary<string, Actor> Actors { get; private set; }

    public Dictionary<string, ConfigurableGameObject> Configurables { get; private set; }

    public int MaxSteps { get; private set; }

    public int FrameSkips { get; private set; }

    public float SolvedThreshold { get; private set; }
  }
}
