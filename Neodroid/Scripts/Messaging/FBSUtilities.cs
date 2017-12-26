using System.Collections.Generic;
using FlatBuffers;
using Neodroid.Actors;
using Neodroid.Configurables;
using Neodroid.Motors;
using Neodroid.Observers;
using UnityEngine;
using Neodroid.Environments;
using System.Xml;
using System;
using Neodroid.FBS;
using Neodroid.FBS.Reaction;
using Neodroid.FBS.State;
using Neodroid.Messaging.Messages;

namespace Neodroid.Messaging {
  public static class FBSUtilities {

    #region PublicMethods

    public static byte[] build_states (EnvironmentState[] states) {
      var b = new FlatBufferBuilder (1);
      foreach (var state in states) {

        StringOffset n = b.CreateString (state.EnvironmentName);

        var observers = new Offset<FBSObserver>[state.Observers.Values.Count];
        int k = 0;
        foreach (Observer observer in state.Observers.Values) {
          observers [k++] = build_observer (b, observer);
        }

        var observers_vector = FBSState.CreateObserversVector (b, observers);

        FBSState.StartBodiesVector (b, state.Bodies.Length);
        foreach (Rigidbody rig in state.Bodies) {
          var vel = rig.velocity;
          var ang = rig.angularVelocity;
          FBSBody.CreateFBSBody (b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }
        var bodies_vector = b.EndVector ();

        FBSState.StartPosesVector (b, state.Poses.Length);
        foreach (Transform tra in state.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FBSQuaternionTransform.CreateFBSQuaternionTransform (b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);
        }
        var poses_vector = b.EndVector ();


        Offset<FBSEnvironmentDescription> description_offset = new FlatBuffers.Offset<FBSEnvironmentDescription> ();
        if (state.Description != null) {
          description_offset = build_description (b, state);
        }
        StringOffset d = new StringOffset ();
        if (state.DebugMessage != "") {
          d = b.CreateString (state.DebugMessage);
        }

        FBSState.StartFBSState (b);
        FBSState.AddEnvironmentName (b, n);
        FBSState.AddBodies (b, bodies_vector);
        FBSState.AddPoses (b, poses_vector);
        FBSState.AddTotalEnergySpent (b, state.TotalEnergySpentSinceReset);
        FBSState.AddReward (b, state.Reward);
        FBSState.AddFrameNumber (b, state.FrameNumber);
        FBSState.AddInterrupted (b, state.Interrupted);
        FBSState.AddObservers (b, observers_vector);
        if (state.Description != null) {
          FBSState.AddEnvironmentDescription (b, description_offset);
        }
        if (state.DebugMessage != "") {
          FBSState.AddDebugMessage (b, d);
        }
        var offset = FBSState.EndFBSState (b);

        FBSState.FinishFBSStateBuffer (b, offset);
      }
      return b.SizedByteArray ();
    }

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

    static Offset<FBSMotor> build_motor (FlatBufferBuilder b, Motor motor, string identifier) {
      StringOffset n = b.CreateString (identifier);
      FBSMotor.StartFBSMotor (b);
      FBSMotor.AddMotorName (b, n);
      FBSMotor.AddValidInput (b, FBSRange.CreateFBSRange (b, motor.ValidInput.decimal_granularity, motor.ValidInput.max_value, motor.ValidInput.min_value));
      FBSMotor.AddEnergySpentSinceReset (b, motor.GetEnergySpend ());
      return FBSMotor.EndFBSMotor (b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform_observation (FlatBufferBuilder b, Vector3 pos, Quaternion rot) {
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation (b);
      FBSQuaternionTransformObservation.AddTransform (b, FBSQuaternionTransform.CreateFBSQuaternionTransform (b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation (b);
    }

    static Offset<FBSEulerTransform> build_euler_transform (FlatBufferBuilder b, EulerTransformObserver observer) {
      Vector3 pos = observer._position, rot = observer._rotation, dir = observer._direction;
      FBSEulerTransform.StartFBSEulerTransform (b);
      FBSEulerTransform.AddPosition (b, FBSVector3.CreateFBSVector3 (b, pos.x, pos.y, pos.z));
      FBSEulerTransform.AddRotation (b, FBSVector3.CreateFBSVector3 (b, rot.x, rot.y, rot.z));
      FBSEulerTransform.AddDirection (b, FBSVector3.CreateFBSVector3 (b, dir.x, dir.y, dir.z));
      return FBSEulerTransform.EndFBSEulerTransform (b);
    }


    static Offset<FBSByteArray> build_byte_array (FlatBufferBuilder b, CameraObserver camera) {
      var v_offset = FBSByteArray.CreateByteArrayVector (b, camera.Data);
      FBSByteArray.StartFBSByteArray (b);
      FBSByteArray.AddDataType (b, FBSByteDataType.PNG);
      FBSByteArray.AddByteArray (b, v_offset);
      return FBSByteArray.EndFBSByteArray (b);
    }

    static Offset<FBSBodyObservation> build_body_observation (FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FBSBodyObservation.StartFBSBodyObservation (b);
      FBSBodyObservation.AddBody (b, FBSBody.CreateFBSBody (b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return FBSBodyObservation.EndFBSBodyObservation (b);
    }

    static Offset<FBSActor> build_actor (FlatBufferBuilder b, Offset<FBSMotor>[] motors, Actor actor, string identifier) {
      StringOffset n = b.CreateString (actor.ActorIdentifier);
      var motor_vector = FBSActor.CreateMotorsVector (b, motors);
      FBSActor.StartFBSActor (b);
      FBSActor.AddActorName (b, n);
      FBSActor.AddMotors (b, motor_vector);
      return FBSActor.EndFBSActor (b);
    }

    static Offset<FBSObserver> build_observer (FlatBufferBuilder b, Observer observer) {
      if (observer) {
        StringOffset n = b.CreateString (observer.ObserverIdentifier);
        int observation_offset = 0;
        FBSObserverData observation_type = FBSObserverData.NONE;
        if (observer.GetType () == typeof(EulerTransformObserver)) {
          observation_offset = build_euler_transform (b, (EulerTransformObserver)observer).Value;
          observation_type = FBSObserverData.FBSEulerTransform;
        } else if (observer.GetType () == typeof(CameraObserver)) {
          observation_offset = build_byte_array (b, (CameraObserver)observer).Value;
          observation_type = FBSObserverData.FBSByteArray;
        } /*else if (observer.GetType () == typeof(QuaternionTransformObserver)) {
        observation_offset = build_byte_array (b, (QuaternionTransformObserver)observer).Value;
      observation_type = FBSObserverData.FBSByteArray;
    }*/
        FBSObserver.StartFBSObserver (b);
        FBSObserver.AddObserverName (b, n);
        FBSObserver.AddObservationType (b, observation_type);
        if (observation_offset != 0) {
          FBSObserver.AddObservation (b, observation_offset);
        }
        return FBSObserver.EndFBSObserver (b);
      }
      FBSObserver.StartFBSObserver (b);
      return FBSObserver.EndFBSObserver (b);
    }

    static Offset<FBSEnvironmentDescription> build_description (FlatBufferBuilder b, EnvironmentState state) {
      var actors = new Offset<FBSActor>[state.Description.Actors.Values.Count];
      int j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors = new Offset<FBSMotor>[actor.Value.Motors.Values.Count];
        int i = 0;
        foreach (var motor in actor.Value.Motors) {
          motors [i++] = build_motor (b, motor.Value, motor.Key);
        }
        actors [j++] = build_actor (b, motors, actor.Value, actor.Key);
      }

      var actors_vector = FBSEnvironmentDescription.CreateActorsVector (b, actors);


      var configurables = new Offset<FBSConfigurable>[state.Description.Configurables.Values.Count];
      int k = 0;
      foreach (var configurable in state.Description.Configurables) {
        configurables [k++] = build_configurable (b, configurable.Value, configurable.Key);
      }
      var configurables_vector = FBSEnvironmentDescription.CreateConfigurablesVector (b, configurables);


      FBSEnvironmentDescription.StartFBSEnvironmentDescription (b);
      FBSEnvironmentDescription.AddActors (b, actors_vector);
      FBSEnvironmentDescription.AddConfigurables (b, configurables_vector);
      return FBSEnvironmentDescription.EndFBSEnvironmentDescription (b);
    }

    static Offset<FBSConfigurable> build_configurable (FlatBufferBuilder b, ConfigurableGameObject configurable, string identifier) {
      StringOffset n = b.CreateString (identifier);
      FBSConfigurable.StartFBSConfigurable (b);
      FBSConfigurable.AddConfigurableName (b, n);
      FBSConfigurable.AddValidInput (b, FBSRange.CreateFBSRange (b, configurable.ValidInput.decimal_granularity, configurable.ValidInput.max_value, configurable.ValidInput.min_value));
      return FBSConfigurable.EndFBSConfigurable (b);
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

    #endregion
  }
}
