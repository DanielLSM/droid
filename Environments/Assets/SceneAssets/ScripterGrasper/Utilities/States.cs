using System;

namespace SceneAssets.ScripterGrasper.Utilities {
  #region Enums

  public enum MotionState {
    IsAtRest,
    WasMoving,
    IsMoving
  }

  public enum PathFindingState {
    WaitingForTarget,
    WaitingForRestingEnvironment,
    Navigating,
    Approaching,
    PickingUpTarget,
    Returning
  }

  public enum GripperState {
    Closed,
    Open,
    Closing,
    Opening
  }

  public enum ClawState {
    TouchingTarget,
    NotTouchingTarget
  }

  public enum TargetState {
    Grabbed,
    NotGrabbed,
    OutsideRegion,
    InsideRegion
  }

  #endregion

  public class States {
    readonly Action _on_state_update_callback;

    ClawState _current_claw_1_state, _current_claw_2_state;

    GripperState _current_gripper_state;
    PathFindingState _current_path_finding_state;

    TargetState _current_target_state;

    MotionState _obstruction_motion_state, _target_motion_state;

    public States(Action on_state_update_callback = null) {
      this._on_state_update_callback = on_state_update_callback;
    }

    public ClawState Claw1State {
      get { return this._current_claw_1_state; }
      set {
        this._current_claw_1_state = value;
        this._on_state_update_callback();
      }
    }

    public ClawState Claw2State {
      get { return this._current_claw_2_state; }
      set {
        this._current_claw_2_state = value;
        this._on_state_update_callback();
      }
    }

    public TargetState TargetState {
      get { return this._current_target_state; }
      set {
        this._current_target_state = value;
        this._on_state_update_callback();
      }
    }

    public GripperState GripperState {
      get { return this._current_gripper_state; }
      set {
        this._current_gripper_state = value;
        this._on_state_update_callback();
      }
    }

    public PathFindingState PathFindingState {
      get { return this._current_path_finding_state; }
      set {
        this._current_path_finding_state = value;
        this._on_state_update_callback();
      }
    }

    public MotionState ObstructionMotionState {
      get { return this._obstruction_motion_state; }
      set {
        this._obstruction_motion_state = value;
        this._on_state_update_callback();
      }
    }

    public MotionState TargetMotionState {
      get { return this._target_motion_state; }
      set {
        this._target_motion_state = value;
        this._on_state_update_callback();
      }
    }

    public MotionState GetMotionState<T>(T[] objects, MotionState previous_state, float sensitivity = 0.1f)
        where T : IMotionTracker {
      foreach (var o in objects) {
        if (o.IsInMotion(sensitivity))
          return MotionState.IsMoving;
      }

      return previous_state != MotionState.IsMoving ? MotionState.IsAtRest : MotionState.WasMoving;
    }

    public void TargetIsGrabbed() {
      this.TargetState = TargetState.Grabbed;
      this.GripperState = GripperState.Closed;
      this.PathFindingState = PathFindingState.Returning;
    }

    public void OpenClaw() { this.GripperState = GripperState.Opening; }

    public void WaitForRestingEnvironment() {
      this.PathFindingState = PathFindingState.WaitingForRestingEnvironment;
    }

    public void ReturnToStartPosition() { this.PathFindingState = PathFindingState.Returning; }

    public void NavigateToTarget() { this.PathFindingState = PathFindingState.Navigating; }

    public bool IsTargetGrabbed() { return this.TargetState == TargetState.Grabbed; }

    public void WaitForTarget() { this.PathFindingState = PathFindingState.WaitingForTarget; }

    public bool IsTargetNotGrabbed() { return this.TargetState != TargetState.Grabbed; }

    public bool IsGripperClosed() { return this.GripperState == GripperState.Closed; }

    public bool IsEnvironmentAtRest() {
      return this.TargetMotionState == MotionState.IsAtRest
             && this.ObstructionMotionState == MotionState.IsAtRest;
    }

    public bool WasEnvironmentMoving() {
      return this.ObstructionMotionState == MotionState.WasMoving
             || this.TargetMotionState == MotionState.WasMoving;
    }

    public bool IsEnvironmentMoving() {
      return this.ObstructionMotionState == MotionState.IsMoving
             || this.TargetMotionState == MotionState.IsMoving;
    }

    public bool WereObstructionMoving() { return this.ObstructionMotionState == MotionState.WasMoving; }

    public bool IsObstructionsAtRest() { return this.ObstructionMotionState == MotionState.IsAtRest; }

    public bool AreBothClawsTouchingTarget() {
      return this.Claw1State == ClawState.NotTouchingTarget && this.Claw2State == ClawState.NotTouchingTarget;
    }

    public void TargetIsNotGrabbed() { this.TargetState = TargetState.NotGrabbed; }

    public bool IsTargetTouchingAndInsideRegion() {
      return this.Claw1State == ClawState.TouchingTarget
             && this.Claw2State == ClawState.TouchingTarget
             && this.TargetState == TargetState.InsideRegion;
    }

    public bool IsTargetInsideRegionOrTouching() {
      return this.Claw1State == ClawState.TouchingTarget
             || this.Claw2State == ClawState.TouchingTarget
             || this.TargetState == TargetState.InsideRegion;
    }

    public bool IsGripperOpen() { return this.GripperState == GripperState.Open; }

    public void ApproachTarget() { this.PathFindingState = PathFindingState.Approaching; }

    public void GripperIsOpen() { this.GripperState = GripperState.Open; }

    public void PickUpTarget() {
      //TargetState = TargetState.InsideRegion;
      this.GripperState = GripperState.Closing;
      this.PathFindingState = PathFindingState.PickingUpTarget;
    }

    public void GripperIsClosed() { this.GripperState = GripperState.Closed; }

    public void Claw1IsNotTouchingTarget() {
      this.Claw1State = ClawState.NotTouchingTarget;
      //TargetIsNotGrabbed();
    }

    public void Claw2IsNotTouchingTarget() {
      this.Claw2State = ClawState.NotTouchingTarget;
      //TargetIsNotGrabbed();
    }

    public void Claw2IsTouchingTarget() { this.Claw2State = ClawState.TouchingTarget; }

    public void Claw1IsTouchingTarget() { this.Claw1State = ClawState.TouchingTarget; }

    public void TargetIsOutsideRegion() { this.TargetState = TargetState.OutsideRegion; }

    public void TargetIsInsideRegion() { this.TargetState = TargetState.InsideRegion; }

    public bool IsPickingUpTarget() { return this.PathFindingState == PathFindingState.PickingUpTarget; }

    public void ResetStates() {
      this.TargetState = TargetState.OutsideRegion;
      this.ObstructionMotionState = MotionState.IsAtRest;
      this.TargetMotionState = MotionState.IsAtRest;
      this.GripperState = GripperState.Open;
      this.PathFindingState = PathFindingState.WaitingForTarget;
      this.Claw1State = ClawState.NotTouchingTarget;
      this.Claw2State = ClawState.NotTouchingTarget;
    }
  }
}
