using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurations {
  public class Difficulty : ConfigurableGameObject {

    public override void ApplyConfiguration (Configuration configuration) { 
      if (configuration.ConfigurableValue == 1) {
        print ("Increased Difficulty");
      } else if (configuration.ConfigurableValue == -1) {
        print ("Decreased Difficulty");
      }
    }

  }
}
