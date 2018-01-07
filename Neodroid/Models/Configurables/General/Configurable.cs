using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Configurables {
  //[ExecuteInEditMode]
  public abstract class Configurable : MonoBehaviour {
    public abstract string ConfigurableIdentifier { get; }
    public abstract void ApplyConfiguration(Configuration obj);
  }
}
