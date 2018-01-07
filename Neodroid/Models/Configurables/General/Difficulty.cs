using Neodroid.Configurables;
using Neodroid.Messaging.Messages;

namespace Assets.Neodroid.Models.Configurables.General {
  public class Difficulty : ConfigurableGameObject {

    public override void ApplyConfiguration (Configuration configuration) {
      if (System.Math.Abs (configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (System.Math.Abs (configuration.ConfigurableValue - (-1)) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}
