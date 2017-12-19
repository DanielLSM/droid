using System.Collections.Generic;
using FlatBuffers;
using Neodroid.Actors;
using Neodroid.Configurables;
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
using System;

namespace Neodroid.Messaging {
  public static class FBSUtilities {

    #region PublicMethods

    public static byte[] build_states (EnvironmentState[] states) {
      var b = new FlatBufferBuilder (1);
      foreach (var state in states) {

        StringOffset n = b.CreateString (state._environment_name);

        var observers = new Offset<FBSObserver>[state._observers.Values.Count];
        int k = 0;
        foreach (Observer observer in state._observers.Values) {
          observers [k++] = build_observer (b, observer);
        }

        FBSState.CreateObserversVector (b, observers);
        var observers_vector = b.EndVector ();

        Offset<FBSEnvironmentDescription> description_offset = new FlatBuffers.Offset<FBSEnvironmentDescription> ();
        if (state._description != null) {
          description_offset = build_description (b, state);
        }
        StringOffset d = new StringOffset ();
        if (state._debug_message != "") {
          d = b.CreateString (state._debug_message);
        }

        FBSState.StartFBSState (b);
        FBSState.AddEnvironmentName (b, n);
        FBSState.AddTotalEnergySpent (b, state._total_energy_spent_since_reset);
        FBSState.AddReward (b, state._reward_for_last_step);
        FBSState.AddFrameNumber (b, state._last_steps_frame_number);
        FBSState.AddInterrupted (b, state._interrupted);
        FBSState.AddObservers (b, observers_vector);
        if (state._description != null) {
          FBSState.AddEnvironmentDescription (b, description_offset);
        }
        if (state._debug_message != "") {
          FBSState.AddDebugMessage (b, d);
        }
        var offset = FBSState.EndFBSState (b);

        FBSState.FinishFBSStateBuffer (b, offset);
      }
      return b.SizedByteArray ();
    }

    public static byte[] build_state (EnvironmentState state) {

      var b = new FlatBufferBuilder (1);

      StringOffset n = b.CreateString (state._environment_name);

      var observers = new Offset<FBSObserver>[state._observers.Values.Count];
      int k = 0;
      foreach (Observer observer in state._observers.Values) {
        observers [k++] = build_observer (b, observer);
      }

      FBSState.CreateObserversVector (b, observers);
      var observers_vector = b.EndVector ();
      var description_offset = build_description (b, state);


      FBSState.StartFBSState (b);
      FBSState.AddEnvironmentName (b, n);
      FBSState.AddDebugMessage (b, n);
      FBSState.AddTotalEnergySpent (b, state._total_energy_spent_since_reset);
      FBSState.AddReward (b, state._reward_for_last_step);
      FBSState.AddFrameNumber (b, state._last_steps_frame_number);
      FBSState.AddInterrupted (b, state._interrupted);
      FBSState.AddObservers (b, observers_vector);
      if (state._description != null) {
        FBSState.AddEnvironmentDescription (b, description_offset);
      }
      var offset = FBSState.EndFBSState (b);

      FBSState.FinishFBSStateBuffer (b, offset);

      //return b.DataBuffer;
      return b.SizedByteArray ();
    }

    public static Reaction create_reaction (FBSReaction reaction) {
      if (reaction.ActionType == FBSAction.FBSMotions) {
        var flat_motions = reaction.Action<FBSMotions> ();
        var motions = create_motions (flat_motions);
        return new Reaction (motions);
      } else if (reaction.ActionType == FBSAction.FBSConfigurations) {
        var flat_configurations = reaction.Action<FBSConfigurations> ();
        var configurations = create_configurations (flat_configurations);
        return new Reaction (configurations);
      }
      return new Reaction (reaction.Reset);
    }

    #endregion

    #region PrivateMethods

    static Offset<FBSMotor> build_motor (FlatBufferBuilder b, Motor motor, string identifier) {
      StringOffset n = b.CreateString (identifier);
      FBSMotor.StartFBSMotor (b);
      FBSMotor.AddMotorName (b, n);
      FBSMotor.AddValidInput (b, FBSRange.CreateFBSRange (b, motor._decimal_granularity, motor._max_strength, motor._min_strength));
      FBSMotor.AddEnergySpentSinceReset (b, motor.GetEnergySpend ());
      return FBSMotor.EndFBSMotor (b);
    }

    static Offset<FBSQuaternionTransform> build_quaternion_transform (FlatBufferBuilder b, Vector3 pos, Quaternion rot) {
      FBSQuaternionTransform.StartFBSQuaternionTransform (b);
      FBSQuaternionTransform.AddPosition (b, FBSVector3.CreateFBSVector3 (b, pos.x, pos.y, pos.z));
      FBSQuaternionTransform.AddRotation (b, FBSQuaternion.CreateFBSQuaternion (b, rot.x, rot.y, rot.z, rot.w));
      return FBSQuaternionTransform.EndFBSQuaternionTransform (b);
    }

    static Offset<FBSEulerTransform> build_euler_transform (FlatBufferBuilder b, Vector3 pos, Vector3 rot, Vector3 dir) {
      FBSEulerTransform.StartFBSEulerTransform (b);
      FBSEulerTransform.AddPosition (b, FBSVector3.CreateFBSVector3 (b, pos.x, pos.y, pos.z));
      FBSEulerTransform.AddRotation (b, FBSVector3.CreateFBSVector3 (b, rot.x, rot.y, rot.z));
      FBSEulerTransform.AddDirection (b, FBSVector3.CreateFBSVector3 (b, dir.x, dir.y, dir.z));
      return FBSEulerTransform.EndFBSEulerTransform (b);
    }

    static Offset<FBSBody> build_body (FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FBSBody.StartFBSBody (b);
      FBSBody.AddVelocity (b, FBSVector3.CreateFBSVector3 (b, vel.x, vel.y, vel.z));
      FBSBody.AddAngularVelocity (b, FBSVector3.CreateFBSVector3 (b, ang.x, ang.y, ang.z));
      return FBSBody.EndFBSBody (b);
    }

    static Offset<FBSActor> build_actor (FlatBufferBuilder b, Offset<FBSMotor>[] motors, Actor actor) {
      StringOffset n = b.CreateString (actor.GetActorIdentifier ());
      FBSActor.CreateMotorsVector (b, motors);
      var motor_vector = b.EndVector ();
      FBSActor.StartFBSActor (b);
      FBSActor.AddActorName (b, n);
      FBSActor.AddMotors (b, motor_vector);
      return FBSActor.EndFBSActor (b);
    }

    static Offset<FBSObserver> build_observer (FlatBufferBuilder b, Observer observer) {
      if (observer.GetType () == typeof(EulerTransformObserver)) {
        var tobs = (EulerTransformObserver)observer;
        var data = build_euler_transform (b, tobs._position, tobs._rotation, tobs._direction);
        StringOffset n = b.CreateString (observer.GetObserverIdentifier ());
        FBSObserver.StartFBSObserver (b);
        FBSObserver.AddObserverName (b, n);
        FBSObserver.AddDataType (b, FBSObserverData.FBSEulerTransform);
        FBSObserver.AddData (b, data.Value);
        return FBSObserver.EndFBSObserver (b);
      } else {
        FBSObserver.StartFBSObserver (b);
        return FBSObserver.EndFBSObserver (b);
      }
    }

    static Offset<FBSEnvironmentDescription> build_description (FlatBufferBuilder b, EnvironmentState state) {
      if (state._description != null) {
        var actors = new Offset<FBSActor>[state._description._actors.Values.Count];
        int j = 0;
        foreach (Actor actor in state._description._actors.Values) {
          var motors = new Offset<FBSMotor>[actor._motors.Values.Count];
          int i = 0;
          foreach (var motor in actor._motors) {
            motors [i++] = build_motor (b, motor.Value, motor.Key);
          }
          actors [j++] = build_actor (b, motors, actor);
        }

        FBSEnvironmentDescription.CreateActorsVector (b, actors);
        var actors_vector = b.EndVector ();

        var configurables = new Offset<FBSConfigurable>[state._description._configurables.Values.Count];
        int k = 0;
        foreach (var configurable in state._description._configurables) {
          configurables [k++] = build_configurable (b, configurable.Value, configurable.Key);
        }

        FBSEnvironmentDescription.CreateConfigurablesVector (b, configurables);
        var configurables_vector = b.EndVector ();


        FBSEnvironmentDescription.StartFBSEnvironmentDescription (b);
        FBSEnvironmentDescription.AddActors (b, actors_vector);
        FBSEnvironmentDescription.AddConfigurables (b, configurables_vector);
        return FBSEnvironmentDescription.EndFBSEnvironmentDescription (b);
      } else {
        FBSEnvironmentDescription.StartFBSEnvironmentDescription (b);
        return FBSEnvironmentDescription.EndFBSEnvironmentDescription (b);
      }
    }

    static Offset<FBSConfigurable> build_configurable (FlatBufferBuilder b, ConfigurableGameObject configurable, string identifier) {
      StringOffset n = b.CreateString (identifier);
      StringOffset o = b.CreateString (configurable._observer_name);
      FBSConfigurable.StartFBSConfigurable (b);
      FBSConfigurable.AddHasObserver (b, configurable._has_observer);
      FBSConfigurable.AddObserverName (b, o);
      FBSConfigurable.AddConfigurableName (b, n);
      FBSConfigurable.AddValidInput (b, FBSRange.CreateFBSRange (b, configurable._decimal_granularity, configurable._max_value, configurable._min_value));
      return FBSConfigurable.EndFBSConfigurable (b);
    }

    static MotorMotion[] create_motions (FBSMotions? motions_maybe) {
      if (motions_maybe.HasValue) {
        var motions = motions_maybe.Value;
        var length = motions.MotionsLength;
        MotorMotion[] m = new MotorMotion[length];
        for (int i = 0; i < length; i++) {
          m [i] = create_motion (motions.Motions (i));
        }
        return m;
      }
      return null;
    }

    static Configuration[] create_configurations (FBSConfigurations? configurations_maybe) {
      if (configurations_maybe.HasValue) {
        var configurations = configurations_maybe.Value;
        var length = configurations.ConfigurationsLength;
        Configuration[] m = new Configuration[length];
        for (int i = 0; i < length; i++) {
          m [i] = create_configuration (configurations.Configurations (i));
        }
        return m;
      }
      return null;
    }

    static Configuration create_configuration (FBSConfiguration? configuration_maybe) {
      if (configuration_maybe.HasValue) {
        FBSConfiguration configuration = configuration_maybe.Value;
        return new Configuration (configuration.ConfigurableName, configuration.ConfigurableValue);
      }
      return null;
    }

    static MotorMotion create_motion (FBSMotion? motion_maybe) {
      if (motion_maybe.HasValue) {
        FBSMotion motion = motion_maybe.Value;
        return new MotorMotion (motion.ActorName, motion.MotorName, motion.Strength);
      }
      return null;
    }

    #endregion
  }
}
