using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Unobservables {
    private readonly Body[] _bodies = { };
    private readonly Pose[] _poses = { };

    public Unobservables(Rigidbody[] rigidbodies, Transform[] transforms) {
      _bodies = new Body[rigidbodies.Length];
      for (var i = 0; i < _bodies.Length; i++)
        _bodies[i] = new Body(
                              rigidbodies[i].velocity,
                              rigidbodies[i].angularVelocity);
      _poses = new Pose[transforms.Length];
      for (var i = 0; i < _poses.Length; i++)
        _poses[i] = new Pose(
                             transforms[i].position,
                             transforms[i].rotation);
    }

    public Unobservables(Body[] bodies, Pose[] poses) {
      _bodies = bodies;
      _poses = poses;
    }

    public Unobservables() { }

    public Body[] Bodies { get { return _bodies; } }

    public Pose[] Poses { get { return _poses; } }

    public override string ToString() {
      var poses_str = "";
      if (Poses != null)
        foreach (var pose in Poses)
          poses_str += pose + "\n";
      var bodies_str = "";
      if (Bodies != null)
        foreach (var body in Bodies)
          bodies_str += body + "\n";
      return string.Format(
                           "<Unobservables>\n {0},{1}\n</Unobservables>\n",
                           poses_str,
                           bodies_str);
    }
  }
}
