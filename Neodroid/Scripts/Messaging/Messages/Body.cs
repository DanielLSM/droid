using UnityEngine;

namespace Neodroid.Scripts.Messaging.Messages {
  public class Body {
    public Body(Vector3 vel, Vector3 ang) {
      this.Velocity = vel;
      this.AngularVelocity = ang;
    }

    public Vector3 Velocity { get; private set; }

    public Vector3 AngularVelocity { get; private set; }
  }
}
