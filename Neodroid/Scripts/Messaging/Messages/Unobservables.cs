using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.Messages {
  public class Unobservables {
    readonly Body[] _bodies = { };
    readonly Pose[] _poses = { };

    public Unobservables(IList<Rigidbody> rigidbodies, IList<Transform> transforms) {
      if (rigidbodies != null) {
        this._bodies = new Body[rigidbodies.Count];
        for (var i = 0; i < this._bodies.Length; i++) {
          if (rigidbodies[i] != null)
            this._bodies[i] = new Body(rigidbodies[i].velocity, rigidbodies[i].angularVelocity);
        }
      }

      if (transforms != null) {
        this._poses = new Pose[transforms.Count];
        for (var i = 0; i < this._poses.Length; i++) {
          if (transforms[i] != null)
            this._poses[i] = new Pose(transforms[i].position, transforms[i].rotation);
        }
      }
    }

    public Unobservables(Body[] bodies, Pose[] poses) {
      this._bodies = bodies;
      this._poses = poses;
    }

    public Unobservables() { }

    public Body[] Bodies { get { return this._bodies; } }

    public Pose[] Poses { get { return this._poses; } }

    public override string ToString() {
      var poses_str = "";
      if (this.Poses != null) {
        foreach (var pose in this.Poses)
          poses_str += pose + "\n";
      }

      var bodies_str = "";
      if (this.Bodies != null) {
        foreach (var body in this.Bodies)
          bodies_str += body + "\n";
      }

      return string.Format("<Unobservables>\n {0},{1}\n</Unobservables>\n", poses_str, bodies_str);
    }
  }
}
