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

    public static byte[] build_states(IEnumerable<EnvironmentState> states) {
      var b = new FlatBufferBuilder(1);
      foreach (var state in states) {
        var n = b.CreateString(state.EnvironmentName);

        var observers = new Offset<FBSObserver>[state.Observers.Values.Count];
        var k = 0;
        foreach (var observer in state.Observers.Values)
          observers[k++] = build_observer(b, observer);

        var observers_vector = FBSState.CreateObserversVector(b, observers);

        FBSUnobservables.StartBodiesVector(b, state.Unobservables.Bodies.Length);
        foreach (var rig in state.Unobservables.Bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          FBSBody.CreateFBSBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }

        var bodies_vector = b.EndVector();

        FBSUnobservables.StartPosesVector(b, state.Unobservables.Poses.Length);
        foreach (var tra in state.Unobservables.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FBSQuaternionTransform.CreateFBSQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w);
        }

        var poses_vector = b.EndVector();

        FBSUnobservables.StartFBSUnobservables(b);
        FBSUnobservables.AddPoses(b, poses_vector);
        FBSUnobservables.AddBodies(b, bodies_vector);
        var unobservables = FBSUnobservables.EndFBSUnobservables(b);

        var description_offset = new Offset<FBSEnvironmentDescription>();
        if (state.Description != null)
          description_offset = build_description(b, state);
        var d = new StringOffset();
        if (state.DebugMessage != "")
          d = b.CreateString(state.DebugMessage);

        FBSState.StartFBSState(b);
        FBSState.AddEnvironmentName(b, n);
        FBSState.AddUnobservables(b, unobservables);
        FBSState.AddTotalEnergySpent(b, state.TotalEnergySpentSinceReset);
        FBSState.AddReward(b, state.Reward);
        FBSState.AddFrameNumber(b, state.FrameNumber);
        FBSState.AddTerminated(b, state.Terminated);
        FBSState.AddObservers(b, observers_vector);
        if (state.Description != null)
          FBSState.AddEnvironmentDescription(b, description_offset);
        if (state.DebugMessage != "")
          FBSState.AddDebugMessage(b, d);
        var offset = FBSState.EndFBSState(b);

        FBSState.FinishFBSStateBuffer(b, offset);
      }

      return b.SizedByteArray();
    }

    #endregion

    #region PrivateMethods

    static Offset<FBSMotor> build_motor(FlatBufferBuilder b, Motor motor, string identifier) {
      var n = b.CreateString(identifier);
      FBSMotor.StartFBSMotor(b);
      FBSMotor.AddMotorName(b, n);
      FBSMotor.AddValidInput(
          b,
          FBSRange.CreateFBSRange(
              b,
              motor.ValidInput.DecimalGranularity,
              motor.ValidInput.MaxValue,
              motor.ValidInput.MinValue));
      FBSMotor.AddEnergySpentSinceReset(b, motor.GetEnergySpend());
      return FBSMotor.EndFBSMotor(b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform_observation(
        FlatBufferBuilder b,
        Vector3 pos,
        Quaternion rot) {
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation(b);
      FBSQuaternionTransformObservation.AddTransform(
          b,
          FBSQuaternionTransform.CreateFBSQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation(b);
    }

    static Offset<FBSEulerTransform> build_euler_transform(
        FlatBufferBuilder b,
        IHasEulerTransformProperties observer) {
      Vector3 pos = observer.Position, rot = observer.Rotation, dir = observer.Direction;
      FBSEulerTransform.StartFBSEulerTransform(b);
      FBSEulerTransform.AddPosition(b, FBSVector3.CreateFBSVector3(b, pos.x, pos.y, pos.z));
      FBSEulerTransform.AddRotation(b, FBSVector3.CreateFBSVector3(b, rot.x, rot.y, rot.z));
      FBSEulerTransform.AddDirection(b, FBSVector3.CreateFBSVector3(b, dir.x, dir.y, dir.z));
      return FBSEulerTransform.EndFBSEulerTransform(b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform(
        FlatBufferBuilder b,
        QuaternionTransformObserver observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation(b);
      FBSQuaternionTransformObservation.AddTransform(
          b,
          FBSQuaternionTransform.CreateFBSQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation(b);
    }

    static Offset<FBSByteArray> build_byte_array(FlatBufferBuilder b, CameraObserver camera) {
      var v_offset = FBSByteArray.CreateByteArrayVector(b, camera.Data);
      FBSByteArray.StartFBSByteArray(b);
      FBSByteArray.AddDataType(b, FBSByteDataType.PNG);
      FBSByteArray.AddByteArray(b, v_offset);
      return FBSByteArray.EndFBSByteArray(b);
    }

    static Offset<FBSBodyObservation> build_body_observation(FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FBSBodyObservation.StartFBSBodyObservation(b);
      FBSBodyObservation.AddBody(b, FBSBody.CreateFBSBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return FBSBodyObservation.EndFBSBodyObservation(b);
    }

    static Offset<FBSActor> build_actor(
        FlatBufferBuilder b,
        Offset<FBSMotor>[] motors,
        Actor actor,
        string identifier) {
      var n = b.CreateString(actor.ActorIdentifier);
      var motor_vector = FBSActor.CreateMotorsVector(b, motors);
      FBSActor.StartFBSActor(b);
      FBSActor.AddAlive(b, actor.Alive);
      FBSActor.AddActorName(b, n);
      FBSActor.AddMotors(b, motor_vector);
      return FBSActor.EndFBSActor(b);
    }

    static Offset<FBSObserver> build_observer(FlatBufferBuilder b, Observer observer) {
      var n = b.CreateString(observer.ObserverIdentifier);
      var observation_offset = 0;
      var observation_type = FBSObserverData.NONE;
      if (observer is IHasEulerTransformProperties) {
        observation_offset = build_euler_transform(b, (IHasEulerTransformProperties)observer).Value;
        observation_type = FBSObserverData.FBSEulerTransform;
      } else if (observer is CameraObserver) {
        observation_offset = build_byte_array(b, (CameraObserver)observer).Value;
        observation_type = FBSObserverData.FBSByteArray;
      } else if (observer is IHasRigidbodyProperties) {
        observation_offset = build_body_observation(
            b,
            ((IHasRigidbodyProperties)observer).Velocity,
            ((IHasRigidbodyProperties)observer).AngularVelocity).Value;
        observation_type = FBSObserverData.FBSBodyObservation;
      } else if (observer is QuaternionTransformObserver) {
        observation_offset = build_quaternion_transform(b, (QuaternionTransformObserver)observer).Value;
        observation_type = FBSObserverData.FBSByteArray;
      } else
        return FBSObserver.CreateFBSObserver(b, n);

      FBSObserver.StartFBSObserver(b);
      FBSObserver.AddObserverName(b, n);
      FBSObserver.AddObservationType(b, observation_type);
      FBSObserver.AddObservation(b, observation_offset);
      return FBSObserver.EndFBSObserver(b);
    }

    static Offset<FBSEnvironmentDescription> build_description(FlatBufferBuilder b, EnvironmentState state) {
      var actors = new Offset<FBSActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors = new Offset<FBSMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors)
          motors[i++] = build_motor(b, motor.Value, motor.Key);
        actors[j++] = build_actor(b, motors, actor.Value, actor.Key);
      }

      var actors_vector = FBSEnvironmentDescription.CreateActorsVector(b, actors);

      var configurables = new Offset<FBSConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables)
        configurables[k++] = build_configurable(b, configurable.Value, configurable.Key);
      var configurables_vector = FBSEnvironmentDescription.CreateConfigurablesVector(b, configurables);

      var api_version_offset = b.CreateString(state.Description.APIVersion);

      FBSEnvironmentDescription.StartFBSEnvironmentDescription(b);
      FBSEnvironmentDescription.AddMaxEpisodeLength(b, state.Description.MaxSteps);
      FBSEnvironmentDescription.AddSolvedThreshold(b, state.Description.SolvedThreshold);
      FBSEnvironmentDescription.AddActors(b, actors_vector);
      FBSEnvironmentDescription.AddConfigurables(b, configurables_vector);
      FBSEnvironmentDescription.AddApiVersion(b,api_version_offset);
      return FBSEnvironmentDescription.EndFBSEnvironmentDescription(b);
    }

    static Offset<FBSPosition> build_position(FlatBufferBuilder b, PositionConfigurable observer) {
      var pos = observer.Position;
      FBSPosition.StartFBSPosition(b);
      FBSPosition.AddPosition(b, FBSVector3.CreateFBSVector3(b, pos.x, pos.y, pos.z));
      return FBSPosition.EndFBSPosition(b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform(
        FlatBufferBuilder b,
        QuaternionTransformConfigurable observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation(b);
      FBSQuaternionTransformObservation.AddTransform(
          b,
          FBSQuaternionTransform.CreateFBSQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation(b);
    }

    static Offset<FBSConfigurable> build_configurable(
        FlatBufferBuilder b,
        ConfigurableGameObject configurable,
        string identifier) {
      var n = b.CreateString(identifier);

      var observation_offset = 0;
      var observation_type = FBSObserverData.NONE;
      if (configurable is QuaternionTransformConfigurable) {
        observation_offset =
            build_quaternion_transform(b, (QuaternionTransformConfigurable)configurable).Value;
        observation_type = FBSObserverData.FBSQuaternionTransformObservation;
      } else if (configurable is PositionConfigurable) {
        observation_offset = build_position(b, (PositionConfigurable)configurable).Value;
        observation_type = FBSObserverData.FBSPosition;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = build_euler_transform(b, (IHasEulerTransformProperties)configurable).Value;
        observation_type = FBSObserverData.FBSEulerTransform;
      } else {
        FBSConfigurable.StartFBSConfigurable(b);
        FBSConfigurable.AddConfigurableName(b, n);
        FBSConfigurable.AddValidInput(
            b,
            FBSRange.CreateFBSRange(
                b,
                configurable.ValidInput.DecimalGranularity,
                configurable.ValidInput.MaxValue,
                configurable.ValidInput.MinValue));
        return FBSConfigurable.EndFBSConfigurable(b);
      }

      FBSConfigurable.StartFBSConfigurable(b);
      FBSConfigurable.AddConfigurableName(b, n);
      FBSConfigurable.AddValidInput(
          b,
          FBSRange.CreateFBSRange(
              b,
              configurable.ValidInput.DecimalGranularity,
              configurable.ValidInput.MaxValue,
              configurable.ValidInput.MinValue));
      FBSConfigurable.AddObservation(b, observation_offset);
      FBSConfigurable.AddObservationType(b, observation_type);
      return FBSConfigurable.EndFBSConfigurable(b);
    }

    #endregion
  }
}
