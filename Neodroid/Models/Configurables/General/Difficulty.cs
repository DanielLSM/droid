using System;
using Neodroid.Scripts.Messaging.Messages;

namespace Neodroid.Models.Configurables.General {
  public class Difficulty : ConfigurableGameObject {
    public override void ApplyConfiguration(Configuration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}
