using UnityEngine;

namespace Neodroid.Scripts.Utilities.Interfaces {
  public interface IHasRigidbody {
    Vector3 Velocity { get; }

    Vector3 AngularVelocity { get; }
  }
}
