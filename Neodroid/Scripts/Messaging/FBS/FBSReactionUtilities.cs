using System.Collections.Generic;
using FlatBuffers;
using Neodroid.Actors;
using Neodroid.Configurables;
using Neodroid.Motors;
using Neodroid.Observers;
using UnityEngine;
using Neodroid.Environments;
using System.Xml;

using Neodroid.FBS;
using Neodroid.FBS.Reaction;
using Neodroid.FBS.State;
using Neodroid.Messaging.Messages;

namespace Neodroid.Messaging {
  public static class FBSReactionUtilities {
    #region PublicMethods

    public static Reaction create_reaction (FBSReaction? reaction) {
      if (reaction.HasValue) {
        var motions = create_motions (reaction.Value);
        var configurations = create_configurations (reaction.Value);
        var bodies = create_bodies (reaction.Value);
        var poses = create_poses (reaction.Value);
        return new Reaction (motions, poses, bodies, configurations, reaction.Value.Reset, reaction.Value.Step, reaction.Value.Configure);
      }
      return new Reaction (false);
    }

    #endregion

    #region PrivateMethods


    static Configuration[] create_configurations (FBSReaction reaction) {
      var l = reaction.ConfigurationsLength;
      Configuration[] configurations = new Configuration[l];
      for (var i = 0; i < l; i++) {
        configurations [i] = create_configuration (reaction.Configurations (i));
      }
      return configurations;
    }

    static MotorMotion[] create_motions (FBSReaction reaction) {
      var l = reaction.MotionsLength;
      MotorMotion[] motions = new MotorMotion[l ];
      for (var i = 0; i < l; i++) {
        motions [i] = create_motion (reaction.Motions (i));
      }
      return motions;
    }

    static Pose create_pose (FBSQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3 ((float)position.X, (float)position.Y, (float)position.Z);
        var quat_rot = new Quaternion ((float)rotation.X, (float)rotation.Y, (float)rotation.Z, (float)rotation.W);
        return new Pose (vec3_pos, quat_rot);
      }
      return new Pose ();
    }

    static Body create_body (FBSBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3 ((float)vel.X, (float)vel.Y, (float)vel.Z);
        var vec3_ang = new Vector3 ((float)ang.X, (float)ang.Y, (float)ang.Z);
        return new Body (vec3_vel, vec3_ang);
      }
      return null;
    }

    static Configuration create_configuration (FBSConfiguration? configuration) {
      if (configuration.HasValue) {
        return new Configuration (configuration.Value.ConfigurableName, (float)configuration.Value.ConfigurableValue);
      }
      return null;
    }

    static MotorMotion create_motion (FBSMotion? motion) {
      if (motion.HasValue) {
        return new MotorMotion (motion.Value.ActorName, motion.Value.MotorName, (float)motion.Value.Strength);
      }
      return null;
    }


    static Pose[] create_poses (FBSReaction reaction) {
      var l = reaction.PosesLength;
      Pose[] poses = new Pose[l ];
      for (var i = 0; i < l; i++) {
        poses [i] = create_pose (reaction.Poses (i));
      }
      return poses;
    }

    static Body[] create_bodies (FBSReaction reaction) {
      var l = reaction.BodiesLength;
      Body[] bodies = new Body[l];
      for (var i = 0; i < l; i++) {
        bodies [i] = create_body (reaction.Bodies (i));
      }
      return bodies;
    }

    #endregion
  }
}

