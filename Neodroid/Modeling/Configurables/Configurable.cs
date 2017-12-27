using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Configurables {
  //[ExecuteInEditMode]
  public abstract class Configurable : MonoBehaviour {
    public abstract void ApplyConfiguration (Configuration obj);

    public abstract string ConfigurableIdentifier{ get; }
  }
}
