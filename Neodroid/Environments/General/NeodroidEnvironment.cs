using System;
using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Environments.General {
  public abstract class NeodroidEnvironment : MonoBehaviour {
    public abstract String Identifier { get; }
    public abstract void PostStep();

    public abstract Reaction SampleReaction();
    public abstract EnvironmentState React(Reaction reaction);
  }
}
