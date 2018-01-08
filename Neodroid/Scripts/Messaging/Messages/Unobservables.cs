using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Unobservables {
    readonly Body[] _bodies = { };
    readonly Pose[] _poses = { };

    public Unobservables(Rigidbody[] rigidbodies, Transform[] transforms) {
      this._bodies = new Body[rigidbodies.Length];
      for (var i = 0; i < this._bodies.Length; i++)
        this._bodies[i] = new Body(
                                   vel : rigidbodies[i].velocity,
                                   ang : rigidbodies[i].angularVelocity);
      this._poses = new Pose[transforms.Length];
      for (var i = 0; i < this._poses.Length; i++)
        this._poses[i] = new Pose(
                                  position : transforms[i].position,
                                  rotation : transforms[i].rotation);
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
      if (this.Poses != null)
        foreach (var pose in this.Poses)
          poses_str += pose + "\n";
      var bodies_str = "";
      if (this.Bodies != null)
        foreach (var body in this.Bodies)
          bodies_str += body + "\n";
      return string.Format(
                           format : "<Unobservables>\n {0},{1}\n</Unobservables>\n",
                           arg0 : poses_str,
                           arg1 : bodies_str);
    }
  }
}
