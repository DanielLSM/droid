using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Configurations {
  public abstract class Configurable : MonoBehaviour {
    public abstract void ApplyConfiguration (Configuration obj);

    public abstract string GetConfigurableIdentifier ();
  }
}
