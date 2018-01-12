using Neodroid.FBS;
using Neodroid.FBS.Reaction;
using Neodroid.Scripts.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.FBS {
  public static class FBSReactionUtilities {
    #region PublicMethods

    public static Reaction create_reaction(FReaction? reaction) {
      if (reaction.HasValue) {
        var motions = create_motions(reaction.Value);
        var configurations = create_configurations(reaction.Value);
        var unobservables = create_unobservables(reaction.Value);
        var parameters = create_parameters(reaction.Value);
        return new Reaction(parameters, motions, configurations, unobservables);
      }

      return new Reaction(null, null, null, null);
    }

    #endregion

    #region PrivateMethods

    static Unobservables create_unobservables(FReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = create_bodies(reaction.Unobservables.Value);

        var poses = create_poses(reaction.Unobservables.Value);

        return new Unobservables(bodies, poses);
      }

      return new Unobservables();
    }

    static ReactionParameters create_parameters(FReaction reaction) {
      if (reaction.Parameters.HasValue) {
        return new ReactionParameters(
            reaction.Parameters.Value.Terminable,
            reaction.Parameters.Value.Step,
            reaction.Parameters.Value.Reset,
            reaction.Parameters.Value.Configure,
            reaction.Parameters.Value.Describe,
            reaction.Parameters.Value.EpisodeCount);
      }

      return new ReactionParameters();
    }

    static Configuration[] create_configurations(FReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new Configuration[l];
      for (var i = 0; i < l; i++)
        configurations[i] = create_configuration(reaction.Configurations(i));
      return configurations;
    }

    static MotorMotion[] create_motions(FReaction reaction) {
      var l = reaction.MotionsLength;
      var motions = new MotorMotion[l];
      for (var i = 0; i < l; i++) motions[i] = create_motion(reaction.Motions(i));
      return motions;
    }

    static Configuration create_configuration(FConfiguration? configuration) {
      if (configuration.HasValue) {
        return new Configuration(
            configuration.Value.ConfigurableName,
            (float)configuration.Value.ConfigurableValue);
      }

      return null;
    }

    static MotorMotion create_motion(FMotion? motion) {
      if (motion.HasValue)
        return new MotorMotion(motion.Value.ActorName, motion.Value.MotorName, (float)motion.Value.Strength);
      return null;
    }

    static Pose[] create_poses(FUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new Pose[l];
      for (var i = 0; i < l; i++) poses[i] = create_pose(unobservables.Poses(i));
      return poses;
    }

    static Body[] create_bodies(FUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new Body[l];
      for (var i = 0; i < l; i++) bodies[i] = create_body(unobservables.Bodies(i));
      return bodies;
    }

    static Pose create_pose(FQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3((float)position.X, (float)position.Y, (float)position.Z);
        var quat_rot = new Quaternion(
            (float)rotation.X,
            (float)rotation.Y,
            (float)rotation.Z,
            (float)rotation.W);
        return new Pose(vec3_pos, quat_rot);
      }

      return new Pose();
    }

    static Body create_body(FBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3((float)vel.X, (float)vel.Y, (float)vel.Z);
        var vec3_ang = new Vector3((float)ang.X, (float)ang.Y, (float)ang.Z);
        return new Body(vec3_vel, vec3_ang);
      }

      return null;
    }

    #endregion
  }
}
