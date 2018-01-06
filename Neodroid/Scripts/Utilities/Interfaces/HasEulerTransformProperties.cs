using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Utilities {
  public interface HasEulerTransformProperties {
    Vector3 Position {
      get;
    }

    Vector3 Direction {
      get;
    }

    Vector3 Rotation {
      get;
    }
  }

}