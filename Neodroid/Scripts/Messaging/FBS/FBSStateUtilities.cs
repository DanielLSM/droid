using System.Collections.Generic;
using FlatBuffers;
using Neodroid.FBS;
using Neodroid.FBS.State;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Motors.General;
using Neodroid.Models.Observers;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.FBS {
  public static class FBSStateUtilities {
    #region PublicMethods

    public static byte[] build_states (IEnumerable<EnvironmentState> states) {
      var b = new FlatBufferBuilder (1);
      foreach (var state in states) {
        var n = b.CreateString (state.EnvironmentName);

        var observers = new Offset<FOBS>[state.Observations.Values.Count];
        var k = 0;
        foreach (var observer in state.Observations.Values)
          observers [k++] = build_observer (b, observer);

        var observers_vector = FState.CreateObservationsVector (b, observers);

        FUnobservables.StartBodiesVector (b, state.Unobservables.Bodies.Length);
        foreach (var rig in state.Unobservables.Bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          FBody.CreateFBody (b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }

        var bodies_vector = b.EndVector ();

        FUnobservables.StartPosesVector (b, state.Unobservables.Poses.Length);
        foreach (var tra in state.Unobservables.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FQuaternionTransform.CreateFQuaternionTransform (
            b,
            pos.x,
            pos.y,
            pos.z,
            rot.x,
            rot.y,
            rot.z,
            rot.w);
        }

        var poses_vector = b.EndVector ();

        FUnobservables.StartFUnobservables (b);
        FUnobservables.AddPoses (b, poses_vector);
        FUnobservables.AddBodies (b, bodies_vector);
        var unobservables = FUnobservables.EndFUnobservables (b);

        var description_offset = new Offset<FEnvironmentDescription> ();
        if (state.Description != null)
          description_offset = build_description (b, state);
        var d = new StringOffset ();
        if (state.DebugMessage != "")
          d = b.CreateString (state.DebugMessage);

        FState.StartFState (b);
        FState.AddEnvironmentName (b, n);
        FState.AddUnobservables (b, unobservables);
        FState.AddTotalEnergySpent (b, state.TotalEnergySpentSinceReset);
        FState.AddReward (b, state.Reward);
        FState.AddFrameNumber (b, state.FrameNumber);
        FState.AddTerminated (b, state.Terminated);
        FState.AddObservations (b, observers_vector);
        if (state.Description != null)
          FState.AddEnvironmentDescription (b, description_offset);
        if (state.DebugMessage != "")
          FState.AddDebugMessage (b, d);
        var offset = FState.EndFState (b);

        FState.FinishFStateBuffer (b, offset);
      }

      return b.SizedByteArray ();
    }

    #endregion

    #region PrivateMethods

    static Offset<FMotor> build_motor (FlatBufferBuilder b, Motor motor, string identifier) {
      var n = b.CreateString (identifier);
      FMotor.StartFMotor (b);
      FMotor.AddMotorName (b, n);
      FMotor.AddValidInput (
        b,
        FRange.CreateFRange (
          b,
          motor.MotionSpace.DecimalGranularity,
          motor.MotionSpace.MaxValue,
          motor.MotionSpace.MinValue));
      FMotor.AddEnergySpentSinceReset (b, motor.GetEnergySpend ());
      return FMotor.EndFMotor (b);
    }

    /*static Offset<FQT> build_quaternion_transform_observation(
        FlatBufferBuilder b,
        Vector3 pos,
        Quaternion rot) {
      FQT.StartFQT(b);
      FQT.AddTransform(
          b,
          FQuaternionTransform.CreateFQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return FQT.EndFQT(b);
    }*/

    static Offset<FEulerTransform> build_euler_transform (
      FlatBufferBuilder b,
      IHasEulerTransform observer) {
      Vector3 pos = observer.Position, rot = observer.Rotation, dir = observer.Direction;
      return FEulerTransform.CreateFEulerTransform (b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, dir.x, dir.y, dir.z);
    }

    static Offset<FQT> build_quaternion_transform (
      FlatBufferBuilder b,
      IHasQuaternionTransform observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FQT.StartFQT (b);
      FQT.AddTransform (
        b,
        FQuaternionTransform.CreateFQuaternionTransform (
          b,
          pos.x,
          pos.y,
          pos.z,
          rot.x,
          rot.y,
          rot.z,
          rot.w));
      return FQT.EndFQT (b);
    }

    static Offset<FByteArray> build_byte_array (FlatBufferBuilder b, IHasByteArray camera) {
      var v_offset = FByteArray.CreateBytesVector (b, camera.Bytes);
      FByteArray.StartFByteArray (b);
      FByteArray.AddType (b, FByteDataType.PNG);
      FByteArray.AddBytes (b, v_offset);
      return FByteArray.EndFByteArray (b);
    }

    static Offset<FRB> build_body_observation (FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FRB.StartFRB (b);
      FRB.AddBody (b, FBody.CreateFBody (b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return FRB.EndFRB (b);
    }

    static Offset<FSingle> build_single (FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle (b);
      FSingle.AddValue (b, numeral.ObservationValue);
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FSingle.EndFSingle (b);
    }

    static Offset<FActor> build_actor (
      FlatBufferBuilder b,
      Offset<FMotor>[] motors,
      Actor actor,
      string identifier) {
      var n = b.CreateString (actor.ActorIdentifier);
      var motor_vector = FActor.CreateMotorsVector (b, motors);
      FActor.StartFActor (b);
      FActor.AddAlive (b, actor.Alive);
      FActor.AddActorName (b, n);
      FActor.AddMotors (b, motor_vector);
      return FActor.EndFActor (b);
    }

    static Offset<FOBS> build_observer (FlatBufferBuilder b, Observer observer) {
      var n = b.CreateString (observer.ObserverIdentifier);


      var observation_offset = 0;
      var observation_type = FObservation.NONE;

      if (observer is CameraObserver) {
        observation_offset = build_byte_array (b, (CameraObserver)observer).Value;
        observation_type = FObservation.FByteArray;
      } else if (observer is IHasSingle) {
        observation_offset = build_single (b, (IHasSingle)observer).Value;
        observation_type = FObservation.FSingle;
      } else if (observer is IHasEulerTransform) {
        observation_offset = build_euler_transform (b, (IHasEulerTransform)observer).Value;
        observation_type = FObservation.FET;
      } else if (observer is IHasQuaternionTransform) {
        observation_offset = build_quaternion_transform (b, (IHasQuaternionTransform)observer).Value;
        observation_type = FObservation.FQT;
      } else if (observer is IHasRigidbody) {
        observation_offset = build_body_observation (b, ((IHasRigidbody)observer).Velocity, ((IHasRigidbody)observer).AngularVelocity).Value;
        observation_type = FObservation.FRB;
      } else if (observer is IHasByteArray) {
        observation_offset = build_byte_array (b, (IHasByteArray)observer).Value;
        observation_type = FObservation.FByteArray;
      } else
        return FOBS.CreateFOBS (b, n);




      FOBS.StartFOBS (b);
      FOBS.AddObservationName (b, n);
      FOBS.AddObservationType (b, observation_type);
      FOBS.AddObservation (b, observation_offset);
      return FOBS.EndFOBS (b);
    }

    static Offset<FEnvironmentDescription> build_description (FlatBufferBuilder b, EnvironmentState state) {
      var actors = new Offset<FActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors = new Offset<FMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors)
          motors [i++] = build_motor (b, motor.Value, motor.Key);
        actors [j++] = build_actor (b, motors, actor.Value, actor.Key);
      }

      var actors_vector = FEnvironmentDescription.CreateActorsVector (b, actors);

      var configurables = new Offset<FConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables)
        configurables [k++] = build_configurable (b, configurable.Value, configurable.Key);
      var configurables_vector = FEnvironmentDescription.CreateConfigurablesVector (b, configurables);

      var api_version_offset = b.CreateString (state.Description.APIVersion);

      FEnvironmentDescription.StartFEnvironmentDescription (b);
      FEnvironmentDescription.AddMaxEpisodeLength (b, state.Description.MaxSteps);
      FEnvironmentDescription.AddSolvedThreshold (b, state.Description.SolvedThreshold);
      FEnvironmentDescription.AddActors (b, actors_vector);
      FEnvironmentDescription.AddConfigurables (b, configurables_vector);
      FEnvironmentDescription.AddApiVersion (b, api_version_offset);
      return FEnvironmentDescription.EndFEnvironmentDescription (b);
    }

    static Offset<FTriple> build_position (FlatBufferBuilder b, PositionConfigurable observer) {
      var pos = observer.Position;
      FTriple.StartFTriple (b);
      FTriple.AddVec3 (b, FVector3.CreateFVector3 (b, pos.x, pos.y, pos.z));
      return FTriple.EndFTriple (b);
    }

    static Offset<FConfigurable> build_configurable (
      FlatBufferBuilder b,
      ConfigurableGameObject configurable,
      string identifier) {
      var n = b.CreateString (identifier);

      var observation_offset = 0;
      var observation_type = FObservation.NONE;
      if (configurable is IHasQuaternionTransform) {
        observation_offset = build_quaternion_transform (b, (IHasQuaternionTransform)configurable).Value;
        observation_type = FObservation.FQT;
      } else if (configurable is PositionConfigurable) {
        observation_offset = build_position (b, (PositionConfigurable)configurable).Value;
        observation_type = FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = build_single (b, (IHasSingle)configurable).Value;
        observation_type = FObservation.FSingle;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = build_euler_transform (b, (IHasEulerTransform)configurable).Value;
        observation_type = FObservation.FET;
      } else {
        FConfigurable.StartFConfigurable (b);
        FConfigurable.AddConfigurableName (b, n);
        return FConfigurable.EndFConfigurable (b);
      }

      FConfigurable.StartFConfigurable (b);
      FConfigurable.AddConfigurableName (b, n);
      FConfigurable.AddObservation (b, observation_offset);
      FConfigurable.AddObservationType (b, observation_type);
      return FConfigurable.EndFConfigurable (b);
    }

    #endregion
  }
}
