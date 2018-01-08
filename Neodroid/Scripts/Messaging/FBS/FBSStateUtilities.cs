using System.Collections.Generic;
using FlatBuffers;
using Neodroid.FBS;
using Neodroid.FBS.State;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables;
using Neodroid.Models.Configurables.General;
using Neodroid.Models.Motors.General;
using Neodroid.Models.Observers;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Scripts.Messaging.FBS {
  public static class FBSStateUtilities {
    #region PublicMethods

    public static byte[] build_states (IEnumerable<EnvironmentState> states) {
      var b = new FlatBufferBuilder (initialSize : 1);
      foreach (var state in states) {
        var n = b.CreateString (s : state.EnvironmentName);

        var observers = new Offset<FBSObserver>[state.Observers.Values.Count];
        var k = 0;
        foreach (var observer in state.Observers.Values)
          observers [k++] = build_observer (
            b : b,
            observer : observer);

        var observers_vector = FBSState.CreateObserversVector (
                                 builder : b,
                                 data : observers);

        FBSUnobservables.StartBodiesVector (
          builder : b,
          numElems : state.Unobservables.Bodies.Length);
        foreach (var rig in state.Unobservables.Bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          FBSBody.CreateFBSBody (
            builder : b,
            velocity_X : vel.x,
            velocity_Y : vel.y,
            velocity_Z : vel.z,
            angular_velocity_X : ang.x,
            angular_velocity_Y : ang.y,
            angular_velocity_Z : ang.z);
        }

        var bodies_vector = b.EndVector ();

        FBSUnobservables.StartPosesVector (
          builder : b,
          numElems : state.Unobservables.Poses.Length);
        foreach (var tra in state.Unobservables.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FBSQuaternionTransform.CreateFBSQuaternionTransform (
            builder : b,
            position_X : pos.x,
            position_Y : pos.y,
            position_Z : pos.z,
            rotation_X : rot.x,
            rotation_Y : rot.y,
            rotation_Z : rot.z,
            rotation_W : rot.w);
        }

        var poses_vector = b.EndVector ();

        FBSUnobservables.StartFBSUnobservables (builder : b);
        FBSUnobservables.AddPoses (
          builder : b,
          posesOffset : poses_vector);
        FBSUnobservables.AddBodies (
          builder : b,
          bodiesOffset : bodies_vector);
        var unobservables = FBSUnobservables.EndFBSUnobservables (builder : b);

        var description_offset = new Offset<FBSEnvironmentDescription> ();
        if (state.Description != null)
          description_offset = build_description (
            b : b,
            state : state);
        var d = new StringOffset ();
        if (state.DebugMessage != "")
          d = b.CreateString (s : state.DebugMessage);

        FBSState.StartFBSState (builder : b);
        FBSState.AddEnvironmentName (
          builder : b,
          environmentNameOffset : n);
        FBSState.AddUnobservables (
          builder : b,
          unobservablesOffset : unobservables);
        FBSState.AddTotalEnergySpent (
          builder : b,
          totalEnergySpent : state.TotalEnergySpentSinceReset);
        FBSState.AddReward (
          builder : b,
          reward : state.Reward);
        FBSState.AddFrameNumber (
          builder : b,
          frameNumber : state.FrameNumber);
        FBSState.AddTerminated (
          builder : b,
          terminated : state.Terminated);
        FBSState.AddObservers (
          builder : b,
          observersOffset : observers_vector);
        if (state.Description != null)
          FBSState.AddEnvironmentDescription (
            builder : b,
            environmentDescriptionOffset : description_offset);
        if (state.DebugMessage != "")
          FBSState.AddDebugMessage (
            builder : b,
            debugMessageOffset : d);
        var offset = FBSState.EndFBSState (builder : b);

        FBSState.FinishFBSStateBuffer (
          builder : b,
          offset : offset);
      }

      return b.SizedByteArray ();
    }

    #endregion

    #region PrivateMethods

    static Offset<FBSMotor> build_motor (FlatBufferBuilder b, Motor motor, string identifier) {
      var n = b.CreateString (s : identifier);
      FBSMotor.StartFBSMotor (builder : b);
      FBSMotor.AddMotorName (
        builder : b,
        motorNameOffset : n);
      FBSMotor.AddValidInput (
        builder : b,
        validInputOffset : FBSRange.CreateFBSRange (
          builder : b,
          DecimalGranularity : motor
                                                                                               .ValidInput
                                                                                               .DecimalGranularity,
          MaxValue : motor.ValidInput.MaxValue,
          MinValue : motor
                                                                                     .ValidInput.MinValue));
      FBSMotor.AddEnergySpentSinceReset (
        builder : b,
        energySpentSinceReset : motor.GetEnergySpend ());
      return FBSMotor.EndFBSMotor (builder : b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform_observation (
      FlatBufferBuilder b,
      Vector3 pos,
      Quaternion rot) {
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation (builder : b);
      FBSQuaternionTransformObservation.AddTransform (
        builder : b,
        transformOffset : FBSQuaternionTransform
                                                       .CreateFBSQuaternionTransform (
          builder : b,
          position_X : pos
                                                                                       .x,
          position_Y : pos
                                                                                       .y,
          position_Z : pos
                                                                                       .z,
          rotation_X : rot
                                                                                       .x,
          rotation_Y : rot
                                                                                       .y,
          rotation_Z : rot
                                                                                       .z,
          rotation_W : rot
                                                                                       .w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation (builder : b);
    }

    static Offset<FBSEulerTransform> build_euler_transform (
      FlatBufferBuilder b,
      IHasEulerTransformProperties observer) {
      Vector3 pos = observer.Position,
      rot = observer.Rotation,
      dir = observer.Direction;
      FBSEulerTransform.StartFBSEulerTransform (builder : b);
      FBSEulerTransform.AddPosition (
        builder : b,
        positionOffset : FBSVector3.CreateFBSVector3 (
          builder : b,
          X : pos.x,
          Y : pos.y,
          Z : pos.z));
      FBSEulerTransform.AddRotation (
        builder : b,
        rotationOffset : FBSVector3.CreateFBSVector3 (
          builder : b,
          X : rot.x,
          Y : rot.y,
          Z : rot.z));
      FBSEulerTransform.AddDirection (
        builder : b,
        directionOffset : FBSVector3.CreateFBSVector3 (
          builder : b,
          X : dir.x,
          Y : dir.y,
          Z : dir.z));
      return FBSEulerTransform.EndFBSEulerTransform (builder : b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform (
      FlatBufferBuilder b,
      QuaternionTransformObserver observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation (builder : b);
      FBSQuaternionTransformObservation.AddTransform (
        builder : b,
        transformOffset : FBSQuaternionTransform
                                                       .CreateFBSQuaternionTransform (
          builder : b,
          position_X : pos
                                                                                       .x,
          position_Y : pos
                                                                                       .y,
          position_Z : pos
                                                                                       .z,
          rotation_X : rot
                                                                                       .x,
          rotation_Y : rot
                                                                                       .y,
          rotation_Z : rot
                                                                                       .z,
          rotation_W : rot
                                                                                       .w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation (builder : b);
    }

    static Offset<FBSByteArray> build_byte_array (FlatBufferBuilder b, CameraObserver camera) {
      var v_offset = FBSByteArray.CreateByteArrayVector (
                       builder : b,
                       data : camera.Data);
      FBSByteArray.StartFBSByteArray (builder : b);
      FBSByteArray.AddDataType (
        builder : b,
        dataType : FBSByteDataType.PNG);
      FBSByteArray.AddByteArray (
        builder : b,
        byteArrayOffset : v_offset);
      return FBSByteArray.EndFBSByteArray (builder : b);
    }

    static Offset<FBSBodyObservation> build_body_observation (
      FlatBufferBuilder b,
      Vector3 vel,
      Vector3 ang) {
      FBSBodyObservation.StartFBSBodyObservation (builder : b);
      FBSBodyObservation.AddBody (
        builder : b,
        bodyOffset : FBSBody.CreateFBSBody (
          builder : b,
          velocity_X : vel.x,
          velocity_Y : vel.y,
          velocity_Z : vel.z,
          angular_velocity_X : ang.x,
          angular_velocity_Y : ang.y,
          angular_velocity_Z : ang.z));
      return FBSBodyObservation.EndFBSBodyObservation (builder : b);
    }

    static Offset<FBSActor> build_actor (
      FlatBufferBuilder b,
      Offset<FBSMotor>[] motors,
      Actor actor,
      string identifier) {
      var n = b.CreateString (s : actor.ActorIdentifier);
      var motor_vector = FBSActor.CreateMotorsVector (
                           builder : b,
                           data : motors);
      FBSActor.StartFBSActor (builder : b);
      FBSActor.AddAlive (
        builder : b,
        alive : actor.Alive);
      FBSActor.AddActorName (
        builder : b,
        actorNameOffset : n);
      FBSActor.AddMotors (
        builder : b,
        motorsOffset : motor_vector);
      return FBSActor.EndFBSActor (builder : b);
    }


    static Offset<FBSObserver> build_observer (FlatBufferBuilder b, Observer observer) {
      var n = b.CreateString (s : observer.ObserverIdentifier);
      var observation_offset = 0;
      var observation_type = FBSObserverData.NONE;
      if (observer is IHasEulerTransformProperties) {
        observation_offset = build_euler_transform (
          b : b,
          observer : (IHasEulerTransformProperties)observer).Value;
        observation_type = FBSObserverData.FBSEulerTransform;
      } else if (observer is CameraObserver) {
        observation_offset = build_byte_array (
          b : b,
          camera : (CameraObserver)observer).Value;
        observation_type = FBSObserverData.FBSByteArray;
      } else if (observer is IHasRigidbodyProperties) {
        observation_offset = build_body_observation (
          b,
          vel : ((IHasRigidbodyProperties)observer).Velocity, ang : ((IHasRigidbodyProperties)observer).AngularVelocity).Value;
        observation_type = FBSObserverData.FBSBodyObservation;
      } else if (observer is QuaternionTransformObserver) {
        observation_offset =
          build_quaternion_transform (
          b : b,
          observer : (QuaternionTransformObserver)observer).Value;
        observation_type = FBSObserverData.FBSByteArray;
      } else {
        return FBSObserver.CreateFBSObserver (
          builder : b,
          observer_nameOffset : n);
      }

      FBSObserver.StartFBSObserver (builder : b);
      FBSObserver.AddObserverName (
        builder : b,
        observerNameOffset : n);
      FBSObserver.AddObservationType (
        builder : b,
        observationType : observation_type);
      FBSObserver.AddObservation (
        builder : b,
        observationOffset : observation_offset);
      return FBSObserver.EndFBSObserver (builder : b);
    }

    static Offset<FBSEnvironmentDescription> build_description (
      FlatBufferBuilder b,
      EnvironmentState state) {
      var actors = new Offset<FBSActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors = new Offset<FBSMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors)
          motors [i++] = build_motor (
            b : b,
            motor : motor.Value,
            identifier : motor.Key);
        actors [j++] = build_actor (
          b : b,
          motors : motors,
          actor : actor.Value,
          identifier : actor.Key);
      }

      var actors_vector = FBSEnvironmentDescription.CreateActorsVector (
                            builder : b,
                            data : actors);

      var configurables = new Offset<FBSConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables)
        configurables [k++] = build_configurable (
          b : b,
          configurable : configurable.Value,
          identifier : configurable.Key);
      var configurables_vector = FBSEnvironmentDescription.CreateConfigurablesVector (
                                   builder : b,
                                   data : configurables);

      FBSEnvironmentDescription.StartFBSEnvironmentDescription (builder : b);
      FBSEnvironmentDescription.AddMaxEpisodeLength (
        builder : b,
        maxEpisodeLength : state.Description.MaxSteps);
      FBSEnvironmentDescription.AddSolvedThreshold (
        builder : b,
        solvedThreshold : state.Description.SolvedThreshold);
      FBSEnvironmentDescription.AddActors (
        builder : b,
        actorsOffset : actors_vector);
      FBSEnvironmentDescription.AddConfigurables (
        builder : b,
        configurablesOffset : configurables_vector);
      return FBSEnvironmentDescription.EndFBSEnvironmentDescription (builder : b);
    }

    static Offset<FBSPosition> build_position (
      FlatBufferBuilder b,
      PositionConfigurable observer) {
      var pos = observer.Position;
      FBSPosition.StartFBSPosition (builder : b);
      FBSPosition.AddPosition (
        builder : b,
        positionOffset : FBSVector3.CreateFBSVector3 (
          builder : b,
          X : pos.x,
          Y : pos.y,
          Z : pos.z));
      return FBSPosition.EndFBSPosition (builder : b);
    }

    static Offset<FBSQuaternionTransformObservation> build_quaternion_transform (
      FlatBufferBuilder b,
      QuaternionTransformConfigurable observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FBSQuaternionTransformObservation.StartFBSQuaternionTransformObservation (builder : b);
      FBSQuaternionTransformObservation.AddTransform (
        builder : b,
        transformOffset : FBSQuaternionTransform
                                                       .CreateFBSQuaternionTransform (
          builder : b,
          position_X : pos
                                                                                       .x,
          position_Y : pos
                                                                                       .y,
          position_Z : pos
                                                                                       .z,
          rotation_X : rot
                                                                                       .x,
          rotation_Y : rot
                                                                                       .y,
          rotation_Z : rot
                                                                                       .z,
          rotation_W : rot
                                                                                       .w));
      return FBSQuaternionTransformObservation.EndFBSQuaternionTransformObservation (builder : b);
    }

    static Offset<FBSConfigurable> build_configurable (
      FlatBufferBuilder b,
      ConfigurableGameObject configurable,
      string identifier) {
      var n = b.CreateString (s : identifier);

      var observation_offset = 0;
      var observation_type = FBSObserverData.NONE;
      if (configurable is QuaternionTransformConfigurable) {
        observation_offset =
          build_quaternion_transform (
          b : b,
          observer : (QuaternionTransformConfigurable)configurable).Value;
        observation_type = FBSObserverData.FBSQuaternionTransformObservation;
      } else if (configurable is PositionConfigurable) {
        observation_offset = build_position (
          b : b,
          observer : (PositionConfigurable)configurable).Value;
        observation_type = FBSObserverData.FBSPosition;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset =
          build_euler_transform (
          b : b,
          observer : (IHasEulerTransformProperties)configurable).Value;
        observation_type = FBSObserverData.FBSEulerTransform;
      } else {
        FBSConfigurable.StartFBSConfigurable (builder : b);
        FBSConfigurable.AddConfigurableName (
          builder : b,
          configurableNameOffset : n);
        FBSConfigurable.AddValidInput (
          builder : b,
          validInputOffset : FBSRange.CreateFBSRange (
            builder : b,
            DecimalGranularity :
                                                                                 configurable
                                                                                   .ValidInput
                                                                                   .DecimalGranularity,
            MaxValue : configurable
                                                                                              .ValidInput
                                                                                              .MaxValue,
            MinValue : configurable
                                                                                              .ValidInput
                                                                                              .MinValue));
        return FBSConfigurable.EndFBSConfigurable (builder : b);
      }

      FBSConfigurable.StartFBSConfigurable (builder : b);
      FBSConfigurable.AddConfigurableName (
        builder : b,
        configurableNameOffset : n);
      FBSConfigurable.AddValidInput (
        builder : b,
        validInputOffset : FBSRange.CreateFBSRange (
          builder : b,
          DecimalGranularity :
                                                                               configurable
                                                                                 .ValidInput
                                                                                 .DecimalGranularity,
          MaxValue : configurable
                                                                                            .ValidInput
                                                                                            .MaxValue,
          MinValue : configurable
                                                                                            .ValidInput
                                                                                            .MinValue));
      FBSConfigurable.AddObservation (
        builder : b,
        observationOffset : observation_offset);
      FBSConfigurable.AddObservationType (
        builder : b,
        observationType : observation_type);
      return FBSConfigurable.EndFBSConfigurable (builder : b);
    }

    #endregion
  }
}
