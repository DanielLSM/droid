using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Unobservables {
    Pose[] _poses = new Pose[]{ };
    Body[] _bodies = new Body[]{ };

    public Body[] Bodies {
      get {
        return _bodies;
      }
    }

    public Pose[] Poses {
      get {
        return _poses;
      }
    }

    public Unobservables (Rigidbody[] rigidbodies, Transform[] transforms) {
      _bodies = new Body[rigidbodies.Length];
      for (var i = 0; i < _bodies.Length; i++) {
        _bodies [i] = new Body (rigidbodies [i].velocity, rigidbodies [i].angularVelocity);
      }
      _poses = new Pose[transforms.Length];
      for (var i = 0; i < _poses.Length; i++) {
        _poses [i] = new Pose (transforms [i].position, transforms [i].rotation);
      }
    }

    public Unobservables (Body[] bodies, Pose[] poses) {
      _bodies = bodies;
      _poses = poses;
    }

    public override string ToString () {
      return "";
    }

  }
}
