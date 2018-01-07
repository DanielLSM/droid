using UnityEngine;

namespace Neodroid.Utilities {
  public interface HasRigidbodyProperties {
    Vector3 Velocity { get; }

    Vector3 AngularVelocity { get; }
  }
}
