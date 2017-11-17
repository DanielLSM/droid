using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Configurations {
  public class Difficulty : Configurable {

    public override void ApplyConfiguration (float value) { 
      if (value == 1) {
        print ("Increased Difficulty");
      } else if (value == -1) {
        print ("Decreased Difficulty");
      }
    }

  }
}
