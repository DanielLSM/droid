using System;
using Neodroid.Messaging.Messages;

namespace Neodroid.Models.Configurables.General {
  public class Difficulty : ConfigurableGameObject {
    public override void ApplyConfiguration(Configuration configuration) {
      if (Math.Abs(value : configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(value : configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}
