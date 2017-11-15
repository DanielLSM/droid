using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Configurations {
  public class Difficulty : Configurable {

    // Example

    public override void Configure (string configuration) { 
      if (configuration == "IncreaseDifficulty") {
        print ("Increased Difficulty");
      } else if (configuration == "DecreaseDifficulty") {
        print ("Decreased Difficulty");
      }
    }

  }
}
