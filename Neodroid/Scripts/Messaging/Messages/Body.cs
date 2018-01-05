
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Body {

    Vector3 _vel;
    Vector3 _ang;

    public Body (Vector3 vel,
                 Vector3 ang) {
      _vel = vel;
      _ang = ang;
    }

    public Vector3 velocity {
      get{ return _vel; }
    }

    public Vector3 angularVelocity {
      get{ return _ang; }
    }
  }
}

