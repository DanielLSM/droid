using System.Collections.Generic;
using FlatBuffers;
using Neodroid.Actors;
using Neodroid.Configurations;
using Neodroid.Messaging;
using Neodroid.Messaging.CustomFBS;
using Neodroid.Messaging.Messages;
using Neodroid.Messaging.Models.Reaction;
using Neodroid.Messaging.Models.State;
using Neodroid.Motors;
using Neodroid.Observers;
using UnityEngine;
using Neodroid.Environments;
using System.Xml;

namespace Neodroid.Messaging {
  public static class FBSUtilities {

    private static Offset<FBSMotor> build_motor (FlatBufferBuilder b, Motor motor, string identifier) {
      StringOffset n = b.CreateString (identifier);
      FBSMotor.StartFBSMotor (b);
      FBSMotor.AddMotorName (b, n);
      FBSMotor.AddValidInput (b, FBSInputRange.CreateFBSInputRange (b, motor._decimal_granularity, motor._max_strength, motor._min_strength));
      FBSMotor.AddEnergyCost (b, motor._energy_cost);
      FBSMotor.AddEnergySpentSinceReset (b, motor.GetEnergySpend ());
      return FBSMotor.EndFBSMotor (b);
    }

    /*private static Offset<FBSPosRotDir> build_posrotdir (FlatBufferBuilder b, Vector3 vec3_pos, Quaternion quat_rot, Quaternion quat_dir) {
          FBSPosRotDir.StartFBSPosRotDir (b);
          FBSPosRotDir.AddPosition (b, FBSVec3.CreateFBSVec3 (b, vec3_pos.x, vec3_pos.y, vec3_pos.z));
          FBSPosRotDir.AddDirection (b, FBSQuat.CreateFBSQuat (b, quat_dir.x, quat_dir.y, quat_dir.z, quat_dir.w));
          FBSPosRotDir.AddRotation (b, FBSQuat.CreateFBSQuat (b, quat_rot.x, quat_rot.y, quat_rot.z, quat_rot.w));
          return FBSPosRotDir.EndFBSPosRotDir (b);
        }*/

    private static Offset<FBSTransform> build_transform (FlatBufferBuilder b, Vector3 vec3_pos, Vector3 vec3_rot, Vector3 vec3_dir) {
      return FBSTransform.CreateFBSTransform (b, vec3_pos.x, vec3_pos.y, vec3_pos.z, vec3_dir.x, vec3_dir.y, vec3_dir.z, vec3_rot.x, vec3_rot.y, vec3_rot.z);
    }

    private static Offset<FBSBody> build_body (FlatBufferBuilder b, Vector3 vec3_vel, Vector3 vec3_ang) {
      return FBSBody.CreateFBSBody (b, vec3_vel.x, vec3_vel.y, vec3_vel.z, vec3_ang.x, vec3_ang.y, vec3_ang.z);
    }

    private static Offset<FBSActor> build_actor (FlatBufferBuilder b, Offset<FBSMotor>[] motors, Actor actor) {
      StringOffset n = b.CreateString (actor.GetActorIdentifier ());
      FBSActor.CreateMotorsVector (b, motors);
      var motor_vector = b.EndVector ();
      FBSActor.StartFBSActor (b);
      FBSActor.AddActorName (b, n);
      FBSActor.AddMotors (b, motor_vector);
      return FBSActor.EndFBSActor (b);
    }

    private static Offset<FBSObserver> build_observer (FlatBufferBuilder b, Observer observer) {
      FBSObserver.CreateDataVector (b, observer.GetData ());
      //CustomFBSImplementation.CreateDataVectorAndAddAllDataAtOnce (b, observer.GetData ());
      var data_vector = b.EndVector ();
      StringOffset n = b.CreateString (observer.GetObserverIdentifier ());
      StringOffset data_type = b.CreateString (observer.GetObserverIdentifier ());
      FBSObserver.StartFBSObserver (b);
      FBSObserver.AddObserverName (b, n);
      FBSObserver.AddData (b, data_vector);
      FBSObserver.AddDataType (b, data_type);
      FBSObserver.AddTransform (b, build_transform (b, observer._position, observer._rotation, observer._direction));
      FBSObserver.AddBody (b, build_body (b, observer._velocity, observer._angular_velocity));
      return FBSObserver.EndFBSObserver (b);
    }

    private static Offset<FBSEnvironmentDescription> build_description (FlatBufferBuilder b, EnvironmentDescription description) {
      FBSEnvironmentDescription.StartFBSEnvironmentDescription (b);
      return FBSEnvironmentDescription.EndFBSEnvironmentDescription (b);
    }

    private static Offset<FBSConfigurable> build_configurable (FlatBufferBuilder b, ConfigurableGameObject configurable) {
      StringOffset n = b.CreateString (configurable.GetConfigurableIdentifier ());
      FBSConfigurable.StartFBSConfigurable (b);
      FBSConfigurable.AddConfigurableName (b, n);
      FBSConfigurable.AddValidInput (b, FBSInputRange.CreateFBSInputRange (b, configurable._decimal_granularity, configurable._max_strength, configurable._min_strength));
      return FBSConfigurable.EndFBSConfigurable (b);
    }

    public static byte[] build_states (EnvironmentState[] states) {
      var b = new FlatBufferBuilder (1);
      foreach (var state in states) {
        var actors = new Offset<FBSActor>[state._actors.Values.Count];
        int j = 0;
        foreach (Actor actor in state._actors.Values) {
          var motors = new Offset<FBSMotor>[actor._motors.Values.Count];
          int i = 0;
          foreach (var motor in actor._motors) {
            motors [i++] = build_motor (b, motor.Value, motor.Key);
          }
          actors [j++] = build_actor (b, motors, actor);
        }

        var observers = new Offset<FBSObserver>[state._observers.Values.Count];
        int k = 0;
        foreach (Observer observer in state._observers.Values) {
          observers [k++] = build_observer (b, observer);
        }

        FBSState.CreateActorsVector (b, actors);
        var actors_vector = b.EndVector ();
        FBSState.CreateObserversVector (b, observers);
        var observers_vector = b.EndVector ();
        var description_offset = build_description (b, state._description);
        

        FBSState.StartFBSState (b);
        FBSState.AddTotalEnergySpent (b, state._total_energy_spent_since_reset);
        FBSState.AddReward (b, state._reward_for_last_step);
        FBSState.AddActors (b, actors_vector);
        FBSState.AddObservers (b, observers_vector);
        if (state._description != null) {
          FBSState.AddEnvironmentDescription (b, description_offset);
        }
        FBSState.AddFrameNumber (b, state._last_steps_frame_number);
        FBSState.AddInterrupted (b, state._interrupted);
        var offset = FBSState.EndFBSState (b);

        FBSState.FinishFBSStateBuffer (b, offset);
      }
      return b.SizedByteArray ();
    }

    public static byte[] build_state (EnvironmentState state) {

      var b = new FlatBufferBuilder (1);

      var actors = new Offset<FBSActor>[state._actors.Values.Count];
      int j = 0;
      foreach (Actor actor in state._actors.Values) {
        var motors = new Offset<FBSMotor>[actor._motors.Values.Count];
        int i = 0;
        foreach (var motor in actor._motors) {
          motors [i++] = build_motor (b, motor.Value, motor.Key);
        }
        actors [j++] = build_actor (b, motors, actor);
      }

      var observers = new Offset<FBSObserver>[state._observers.Values.Count];
      int k = 0;
      foreach (Observer observer in state._observers.Values) {
        observers [k++] = build_observer (b, observer);
      }

      FBSState.CreateActorsVector (b, actors);
      var actors_vector = b.EndVector ();
      FBSState.CreateObserversVector (b, observers);
      var observers_vector = b.EndVector ();
      var description_offset = build_description (b, state._description);


      FBSState.StartFBSState (b);
      FBSState.AddTotalEnergySpent (b, state._total_energy_spent_since_reset);
      FBSState.AddReward (b, state._reward_for_last_step);
      FBSState.AddActors (b, actors_vector);
      FBSState.AddObservers (b, observers_vector);
      if (state._description != null) {
        FBSState.AddEnvironmentDescription (b, description_offset);
      }
      FBSState.AddFrameNumber (b, state._last_steps_frame_number);
      FBSState.AddInterrupted (b, state._interrupted);
      var offset = FBSState.EndFBSState (b);

      FBSState.FinishFBSStateBuffer (b, offset);

      //return b.DataBuffer;
      return b.SizedByteArray ();
    }


    public static Reaction create_reaction (FBSReaction reaction) {
      return new Reaction (create_motions (reaction, reaction.MotionsLength), create_configurations (reaction, reaction.ConfigurationsLength), reaction.Reset);
    }


    public static MotorMotion[] create_motions (FBSReaction reaction, int len) {
      MotorMotion[] m = new MotorMotion[len];
      for (int i = 0; i < len; i++) {
        m [i] = create_motion (reaction.Motions (i));
      }
      return m;
    }

    public static Configuration[] create_configurations (FBSReaction reaction, int len) {
      Configuration[] m = new Configuration[len];
      for (int i = 0; i < len; i++) {
        m [i] = create_configuration (reaction.Configurations (i));
      }
      return m;
    }

    public static Configuration create_configuration (FBSConfiguration? configuration_maybe) {
      FBSConfiguration configuration;
      try {
        configuration = configuration_maybe.Value;
        return new Configuration (configuration.ConfigurableName, configuration.ConfigurableValue);
      } catch {
        return null;
      }
    }

    public static MotorMotion create_motion (FBSMotion? motion_maybe) {
      FBSMotion motion;
      try {
        motion = motion_maybe.Value;
        return new MotorMotion (motion.ActorName, motion.MotorName, motion.Strength);
      } catch {
        return null;
      }
    }
  }
}
