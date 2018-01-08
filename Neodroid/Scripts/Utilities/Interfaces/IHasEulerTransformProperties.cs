using UnityEngine;

namespace Neodroid.Scripts.Utilities.Interfaces {
  public interface IHasEulerTransformProperties {
    Vector3 Position { get; }

    Vector3 Direction { get; }

    Vector3 Rotation { get; }
  }
}
