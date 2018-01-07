using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Body {
    public Body(
      Vector3 vel,
      Vector3 ang) {
      velocity = vel;
      angularVelocity = ang;
    }

    public Vector3 velocity { get; private set; }

    public Vector3 angularVelocity { get; private set; }
  }
}
