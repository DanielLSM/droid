using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Configurations {
  [ExecuteInEditMode]
  public abstract class Configurable : MonoBehaviour {
    public abstract void ApplyConfiguration (Configuration obj);

    public abstract string GetConfigurableIdentifier ();
  }
}
