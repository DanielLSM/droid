using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public interface Configurable {
    void ApplyConfiguration (Configuration obj);

    string GetConfigurableIdentifier ();
  }
}
