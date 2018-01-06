using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Utilities {
  public interface HasRigidbodyProperties {
    Vector3 Velocity {
      get;
    }

    Vector3 AngularVelocity {
      get;
    }
      
  }
}