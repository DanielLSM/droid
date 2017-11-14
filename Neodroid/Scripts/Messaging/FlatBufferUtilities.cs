using FlatBuffers;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.NeodroidEnvironment.Actors;
using Neodroid.NeodroidEnvironment.Observers;
using Neodroid.NeodroidEnvironment.Motors;
using Neodroid.Messaging.FlatBuffer;
using Neodroid.Messaging.Models.State;
using Neodroid.Messaging.Models.Reaction;

namespace Neodroid.Messaging {
  public static class FlatBufferUtilities {

    private static Offset<FlatBufferMotor> build_motor (FlatBufferBuilder b, Motor motor) {
      StringOffset n = b.CreateString (motor.GetMotorIdentifier ());
      FlatBufferMotor.StartFlatBufferMotor (b);
      FlatBufferMotor.AddName (b, n);
      FlatBufferMotor.AddBinary (b, motor._bidirectional);
      FlatBufferMotor.AddEnergyCost (b, motor._energy_cost);
      FlatBufferMotor.AddEnergySpentSinceReset (b, motor.GetEnergySpend ());
      return FlatBufferMotor.EndFlatBufferMotor (b);
    }

    /*private static Offset<FlatBufferPosRotDir> build_posrotdir (FlatBufferBuilder b, Vector3 vec3_pos, Quaternion quat_rot, Quaternion quat_dir) {
      FlatBufferPosRotDir.StartFlatBufferPosRotDir (b);
      FlatBufferPosRotDir.AddPosition (b, FlatBufferVec3.CreateFlatBufferVec3 (b, vec3_pos.x, vec3_pos.y, vec3_pos.z));
      FlatBufferPosRotDir.AddDirection (b, FlatBufferQuat.CreateFlatBufferQuat (b, quat_dir.x, quat_dir.y, quat_dir.z, quat_dir.w));
      FlatBufferPosRotDir.AddRotation (b, FlatBufferQuat.CreateFlatBufferQuat (b, quat_rot.x, quat_rot.y, quat_rot.z, quat_rot.w));
      return FlatBufferPosRotDir.EndFlatBufferPosRotDir (b);
    }*/

    private static Offset<FlatBufferPosRotDir> build_posrotdir (FlatBufferBuilder b, Vector3 vec3_pos, Vector3 vec3_rot, Vector3 vec3_dir) {
      FlatBufferPosRotDir.StartFlatBufferPosRotDir (b);
      FlatBufferPosRotDir.AddPosition (b, FlatBufferVec3.CreateFlatBufferVec3 (b, vec3_pos.x, vec3_pos.y, vec3_pos.z));
      FlatBufferPosRotDir.AddDirection (b, FlatBufferVec3.CreateFlatBufferVec3 (b, vec3_dir.x, vec3_dir.y, vec3_dir.z));
      FlatBufferPosRotDir.AddRotation (b, FlatBufferVec3.CreateFlatBufferVec3 (b, vec3_rot.x, vec3_rot.y, vec3_rot.z));
      return FlatBufferPosRotDir.EndFlatBufferPosRotDir (b);
    }

    private static Offset<FlatBufferActor> build_actor (FlatBufferBuilder b, Offset<FlatBufferMotor>[] motors, Actor actor) {
      var posrotdir = build_posrotdir (b, actor._position, actor._rotation, actor._direction);
      StringOffset n = b.CreateString (actor.name);
      FlatBufferActor.CreateMotorsVector (b, motors);
      var motor_vector = b.EndVector ();
      FlatBufferActor.StartFlatBufferActor (b);
      FlatBufferActor.AddName (b, n);
      FlatBufferActor.AddMotors (b, motor_vector);
      FlatBufferActor.AddPosrotdir (b, posrotdir);
      return FlatBufferActor.EndFlatBufferActor (b);
    }

    private static Offset<FlatBufferObserver> build_observer (FlatBufferBuilder b, Observer observer) {
      var posrotdir = build_posrotdir (b, observer._position, observer._rotation, observer._direction);
      FlatBufferObserver.CreateDataVector (b, observer.GetData ());
      //CustomFlatBufferImplementation.CreateDataVectorAndAddAllDataAtOnce (b, observer.GetData ());
      var data_vector = b.EndVector ();
      StringOffset n = b.CreateString (observer.name);
      FlatBufferObserver.StartFlatBufferObserver (b);
      FlatBufferObserver.AddName (b, n);
      FlatBufferObserver.AddData (b, data_vector);
      FlatBufferObserver.AddPosrotdir (b, posrotdir);
      return FlatBufferObserver.EndFlatBufferObserver (b);
    }

    public static byte[] build_state (EnvironmentState state) {

      var b = new FlatBufferBuilder (1);

      var actors = new Offset<FlatBufferActor>[state._actors.Values.Count];
      int j = 0;
      foreach (Actor actor in state._actors.Values) {
        var motors = new Offset<FlatBufferMotor>[actor._motors.Values.Count];
        int i = 0;
        foreach (Motor motor in actor._motors.Values) {
          motors [i++] = build_motor (b, motor);
        }
        actors [j++] = build_actor (b, motors, actor);
      }

      var observers = new Offset<FlatBufferObserver>[state._observers.Values.Count];
      int k = 0;
      foreach (Observer observer in state._observers.Values) {
        observers [k++] = build_observer (b, observer);
      }

      FlatBufferState.CreateActorsVector (b, actors);
      var actors_vector = b.EndVector ();
      FlatBufferState.CreateObserversVector (b, observers);
      var observers_vector = b.EndVector ();

      FlatBufferState.StartFlatBufferState (b);
      FlatBufferState.AddTotalEnergySpentSinceReset (b, state._total_energy_spent_since_reset);
      FlatBufferState.AddTimeSinceRest (b, state._time_since_reset);
      FlatBufferState.AddRewardForLastStep (b, state._reward_for_last_step);
      FlatBufferState.AddActors (b, actors_vector);
      FlatBufferState.AddObservers (b, observers_vector);
      FlatBufferState.AddLastStepsFrameNumber (b, state._last_steps_frame_number);
      FlatBufferState.AddInterrupted (b, state._interrupted);
      var offset = FlatBufferState.EndFlatBufferState (b);

      FlatBufferState.FinishFlatBufferStateBuffer (b, offset);

      //return b.DataBuffer;
      return b.SizedByteArray ();
    }


    public static Reaction create_reaction (FlatBufferReaction reaction) {
      return new Reaction (create_motions (reaction, reaction.MotionsLength), null, reaction.Reset);
    }

    public static MotorMotion[] create_motions (FlatBufferReaction reaction, int len) {
      MotorMotion[] m = new MotorMotion[len];
      for (int i = 0; i < len; i++) {
        m [i] = create_motion (reaction.Motions (i));
      }
      return m;
    }

    public static MotorMotion create_motion (FlatBufferMotion? motion_maybe) {
      FlatBufferMotion motion;
      try {
        motion = motion_maybe.Value;
        return new MotorMotion (motion.ActorName, motion.MotorName, motion.Strength);
      } catch { 
        return null;
      }
    }
  }
}
