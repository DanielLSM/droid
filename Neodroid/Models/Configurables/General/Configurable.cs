using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Models.Configurables.General {
  //[ExecuteInEditMode]
  public abstract class Configurable : MonoBehaviour {
    public abstract string ConfigurableIdentifier { get; }
    public abstract void ApplyConfiguration(Configuration obj);
  }
}
