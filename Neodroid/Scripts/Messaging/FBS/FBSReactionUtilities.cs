using Neodroid.FBS;
using Neodroid.FBS.Reaction;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.FBS {
  public static class FBSReactionUtilities {
    #region PublicMethods

    public static Reaction create_reaction(FBSReaction? reaction) {
      if (reaction.HasValue) {
        var motions = create_motions(reaction : reaction.Value);
        var configurations = create_configurations(reaction : reaction.Value);
        var unobservables = create_unobservables(reaction : reaction.Value);
        var parameters = create_parameters(reaction : reaction.Value);
        return new Reaction(
                            parameters : parameters,
                            motions : motions,
                            configurations : configurations,
                            unobservables : unobservables);
      }

      return new Reaction(
                          parameters : null,
                          motions : null,
                          configurations : null,
                          unobservables : null);
    }

    #endregion

    #region PrivateMethods

    static Unobservables create_unobservables(FBSReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = create_bodies(unobservables : reaction.Unobservables.Value);

        var poses = create_poses(unobservables : reaction.Unobservables.Value);

        return new Unobservables(
                                 bodies : bodies,
                                 poses : poses);
      }

      return new Unobservables();
    }

    static ReactionParameters create_parameters(FBSReaction reaction) {
      if (reaction.Parameters.HasValue)
        return new ReactionParameters(
                                      terminable : reaction.Parameters.Value.Terminable,
                                      step : reaction.Parameters.Value.Step,
                                      reset : reaction.Parameters.Value.Reset,
                                      configure : reaction.Parameters.Value.Configure,
                                      describe : reaction.Parameters.Value.Describe,
                                      episode_count : reaction.Parameters.Value.EpisodeCount);
      return new ReactionParameters();
    }

    static Configuration[] create_configurations(FBSReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new Configuration[l];
      for (var i = 0; i < l; i++)
        configurations[i] = create_configuration(configuration : reaction.Configurations(j : i));
      return configurations;
    }

    static MotorMotion[] create_motions(FBSReaction reaction) {
      var l = reaction.MotionsLength;
      var motions = new MotorMotion[l];
      for (var i = 0; i < l; i++) motions[i] = create_motion(motion : reaction.Motions(j : i));
      return motions;
    }

    static Configuration create_configuration(FBSConfiguration? configuration) {
      if (configuration.HasValue)
        return new Configuration(
                                 configurable_name : configuration.Value.ConfigurableName,
                                 configurable_value : (float)configuration.Value.ConfigurableValue);
      return null;
    }

    static MotorMotion create_motion(FBSMotion? motion) {
      if (motion.HasValue)
        return new MotorMotion(
                               actor_name : motion.Value.ActorName,
                               motor_name : motion.Value.MotorName,
                               strength : (float)motion.Value.Strength);
      return null;
    }

    static Pose[] create_poses(FBSUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new Pose[l];
      for (var i = 0; i < l; i++) poses[i] = create_pose(trans : unobservables.Poses(j : i));
      return poses;
    }

    static Body[] create_bodies(FBSUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new Body[l];
      for (var i = 0; i < l; i++) bodies[i] = create_body(body : unobservables.Bodies(j : i));
      return bodies;
    }

    static Pose create_pose(FBSQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3(
                                   x : (float)position.X,
                                   y : (float)position.Y,
                                   z : (float)position.Z);
        var quat_rot = new Quaternion(
                                      x : (float)rotation.X,
                                      y : (float)rotation.Y,
                                      z : (float)rotation.Z,
                                      w : (float)rotation.W);
        return new Pose(
                        position : vec3_pos,
                        rotation : quat_rot);
      }

      return new Pose();
    }

    static Body create_body(FBSBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3(
                                   x : (float)vel.X,
                                   y : (float)vel.Y,
                                   z : (float)vel.Z);
        var vec3_ang = new Vector3(
                                   x : (float)ang.X,
                                   y : (float)ang.Y,
                                   z : (float)ang.Z);
        return new Body(
                        vel : vec3_vel,
                        ang : vec3_ang);
      }

      return null;
    }

    #endregion
  }
}
