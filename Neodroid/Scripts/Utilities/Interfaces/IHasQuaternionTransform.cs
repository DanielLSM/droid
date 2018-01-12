using UnityEngine;

namespace Neodroid.Scripts.Utilities.Interfaces {
  public interface IHasQuaternionTransform {
    Vector3 Position { get; }

    Quaternion Rotation { get; }
  }
}
