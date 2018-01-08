using System;
using System.Collections;
using System.Collections.Generic;
using Neodroid.Scripts.Utilities;
using SceneAssets.ScripterGrasper.Grasps;
using SceneAssets.ScripterGrasper.Navigation;
using SceneAssets.ScripterGrasper.Utilities;
using SceneSpecificAssets.Grasping.Navigation;
using SceneSpecificAssets.Grasping.Utilities;
using UnityEngine;

namespace SceneAssets.ScripterGrasper.Scripts {
  public class ScriptedGripper : MonoBehaviour {
    #region Helpers

    void PerformReactionToCurrentState(States state) {
      switch (state.PathFindingState) {
        case PathFindingState.WaitingForTarget:
          this.FindTargetAndUpdatePath();
          if (this._target_grasp != null)
            state.NavigateToTarget();
          break;

        case PathFindingState.Navigating:
          if (state.IsEnvironmentMoving() && this._wait_for_resting_environment) {
            state.WaitForRestingEnvironment();
            break;
          }

          if (state.WasEnvironmentMoving()) this.FindTargetAndUpdatePath();
          this.FollowPathToApproach(
                                    step_size : this._step_size,
                                    rotation : this._target_grasp.transform.rotation);
          state.OpenClaw();
          this.CheckIfGripperIsOpen();
          this.MaybeBeginApproachProcedure();
          break;

        case PathFindingState.Approaching:
          this.ApproachTarget(step_size : this._step_size);
          break;

        case PathFindingState.PickingUpTarget:
          if (state.IsGripperClosed() && state.IsTargetNotGrabbed()) {
            //state.ReturnToStartPosition();
          }

          break;

        case PathFindingState.WaitingForRestingEnvironment:
          if (state.WasEnvironmentMoving()) this.FindTargetAndUpdatePath();
          if (state.IsEnvironmentAtRest()) {
            this.FindTargetAndUpdatePath();
            if (state.IsTargetGrabbed())
              state.ReturnToStartPosition();
            else
              state.NavigateToTarget();
          }

          break;

        case PathFindingState.Returning:
          if (state.WereObstructionMoving())
            this._path = this.FindPath(
                                       start_position : this.transform.position,
                                       target_position : this._reset_position);
          if (this._wait_for_resting_environment) {
            if (state.IsObstructionsAtRest()) {
              this.FollowPathToApproach(
                                        step_size : this._step_size,
                                        rotation : Quaternion.Euler(
                                                                    x : 90,
                                                                    y : 0,
                                                                    z : 0));
              this.MaybeBeginReleaseProcedure();
            }
          } else {
            this.FollowPathToApproach(
                                      step_size : this._step_size,
                                      rotation : Quaternion.Euler(
                                                                  x : 90,
                                                                  y : 0,
                                                                  z : 0));
            this.MaybeBeginReleaseProcedure();
          }

          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      switch (state.GripperState) {
        case GripperState.Opening:
          this.OpenClaws(step_size : this._step_size);
          break;

        case GripperState.Closing:
          this.CloseClaws(step_size : this._step_size);
          break;

        case GripperState.Closed:
          break;
      }

      if (!state.IsTargetInsideRegionOrTouching()) {
        state.TargetIsNotGrabbed();
        if (this._target_game_object != null) this._target_game_object.transform.parent = null;
      }

      if (state.IsTargetTouchingAndInsideRegion()) {
        if (this._target_game_object != null) this._target_game_object.transform.parent = this.transform;
        state.TargetIsGrabbed();
        this._path = this.FindPath(
                                   start_position : this.transform.position,
                                   target_position : this._reset_position);
        this._intermediate_target = this._path.Next(step_size : 0.001f);
      }

      this.MaybeClawIsClosed();
    }

    #endregion

    #region PrivateMembers

    GraspableObject _target_game_object;
    Vector3 _approach_position;
    Grasp _target_grasp;
    BezierCurvePath _path;

    Vector3 _default_motor_position;
    Vector3 _closed_motor_position;

    Vector3 _intermediate_target;
    //GameObject[] _targets;

    Vector3 _reset_position;
    BezierCurve _bezier_curve;

    float _step_size;

    #endregion

    #region PublicMembers

    [Space(height : 1)] [Header(header : "Game Objects")]
    [SerializeField]  GameObject _motor;

    [SerializeField]  GameObject _grab_region;
    [SerializeField]  GameObject _begin_grab_region;

    [SerializeField] GameObject _claw_1;
    [SerializeField]  GameObject _claw_2;

    [SerializeField]  States _state;
    [SerializeField]  Transform _closed_motor_transform;
    [SerializeField]  ObstacleSpawner _obstacle_spawner;
    [SerializeField]  BezierCurve _bezier_curve_prefab;

    [Space(height : 1)] [Header(header : "Path Finding Parameters")]
    [SerializeField]  float _search_boundary = 6f;

    [SerializeField]  float _actor_size = 0.3f;
    [SerializeField]  float _grid_granularity = 0.4f;
    [SerializeField]  float _speed = 0.5f;
    [SerializeField]  float _precision = 0.02f;
    [SerializeField]  float _sensitivity = 0.2f;
    [SerializeField]  float _approach_distance = 0.6f;
    [SerializeField]  bool _wait_for_resting_environment;

    [Space(height : 1)] [Header(header : "Show debug logs")]
    [SerializeField]  bool _debugging;

    [Space(height : 1)] [Header(header : "Draw Search Boundary")]
    [SerializeField]  bool _draw_search_boundary = true;

    #endregion

    #region Setup

    void UpdateMeshFilterBounds() {
      var actor_bounds = GraspingUtilities.GetMaxBounds(g : this.gameObject);
      //_environment_size = agent_bounds.size.magnitude;
      //_approach_distance = agent_bounds.size.magnitude + _precision;
      this._actor_size =
        actor_bounds.extents.magnitude
        * 2; //Mathf.Max(agent_bounds.extents.x, Mathf.Max(agent_bounds.extents.y, agent_bounds.extents.z)) * 2;
      this._approach_distance = this._actor_size + this._precision;
    }

    void SetupEnvironment() {
      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this.transform,
                                                                    on_collision_enter_child : this
                                                                      .OnCollisionEnterChild,
                                                                    on_trigger_enter_child : this
                                                                      .OnTriggerEnterChild,
                                                                    on_collision_exit_child : this
                                                                      .OnCollisionExitChild,
                                                                    on_trigger_exit_child : this
                                                                      .OnTriggerExitChild,
                                                                    on_collision_stay_child : this
                                                                      .OnCollisionStayChild,
                                                                    on_trigger_stay_child : this
                                                                      .OnTriggerStayChild,
                                                                    debug : this._debugging);
    }

    #endregion

    #region UnityCallbacks

    void Start() {
      this._state = new States();
      this._reset_position = this.transform.position;
      this._default_motor_position = this._motor.transform.localPosition;
      this._closed_motor_position = this._closed_motor_transform.localPosition;
      this._bezier_curve = FindObjectOfType<BezierCurve>();
      if (!this._bezier_curve) this._bezier_curve = Instantiate(original : this._bezier_curve_prefab);

      this.UpdateMeshFilterBounds();

      this.FindTargetAndUpdatePath();
      this._state.ResetStates();
      this.SetupEnvironment();
    }

    void Update() {
      this._step_size = this._speed * Time.deltaTime;
      if (this._draw_search_boundary)
        GraspingUtilities.DrawBoxFromCenter(
                                            p : this.transform.position,
                                            r : this._search_boundary,
                                            c : Color.magenta);
      this._state.ObstructionMotionState = this._state.GetMotionState(
                                                                      objects :
                                                                      FindObjectsOfType<Obstruction>(),
                                                                      previous_state : this
                                                                                         ._state
                                                                                         .ObstructionMotionState,
                                                                      sensitivity : this._sensitivity);
      this._state.TargetMotionState = this._state.GetMotionState(
                                                                 objects :
                                                                 FindObjectsOfType<GraspableObject>(),
                                                                 previous_state : this
                                                                                    ._state.TargetMotionState,
                                                                 sensitivity : this._sensitivity);

      this.PerformReactionToCurrentState(state : this._state);
    }

    #endregion

    #region StateTransitions

    void MaybeBeginReleaseProcedure() {
      if (!(Vector3.Distance(
                             a : this.transform.position,
                             b : this._reset_position)
            < this._precision))
        return;
      if (this._target_game_object) this._target_game_object.transform.parent = null;
      this._state.OpenClaw();
      this._state.WaitForTarget();

      if (this._obstacle_spawner != null) this._obstacle_spawner.Setup();
    }

    void MaybeClawIsClosed() {
      if (!(Vector3.Distance(
                             a : this._motor.transform.localPosition,
                             b : this._closed_motor_position)
            < this._precision))
        return;
      this._state.GripperIsClosed();
      this._path = this.FindPath(
                                 start_position : this.transform.position,
                                 target_position : this._reset_position);
      this._intermediate_target = this._path.Next(step_size : 0.001f);
      this._state.ReturnToStartPosition();
    }

    void MaybeBeginApproachProcedure() {
      if (Vector3.Distance(
                           a : this.transform.position,
                           b : this._path.TargetPosition)
          < this._approach_distance + this._precision
          && Quaternion.Angle(
                              a : this.transform.rotation,
                              b : this._target_grasp.transform.rotation)
          < this._precision
          && this._state.IsGripperOpen())
        this._state.ApproachTarget();
    }

    void CheckIfGripperIsOpen() {
      if (Vector3.Distance(
                           a : this._motor.transform.localPosition,
                           b : this._default_motor_position)
          < this._precision)
        this._state.GripperIsOpen();
    }

    void CloseClaws(float step_size) {
      //Vector3 current_motor_position = _motor.transform.localPosition;
      //current_motor_position.y += step_size / 16;
      //_motor.transform.localPosition = current_motor_position;
      this._motor.transform.localPosition = Vector3.MoveTowards(
                                                                current : this._motor.transform.localPosition,
                                                                target : this._closed_motor_position,
                                                                maxDistanceDelta : step_size / 8);
    }

    void OpenClaws(float step_size) {
      this._motor.transform.localPosition = Vector3.MoveTowards(
                                                                current : this._motor.transform.localPosition,
                                                                target : this._default_motor_position,
                                                                maxDistanceDelta : step_size / 8);
      //StopCoroutine ("claw_movement");
      //StartCoroutine ("claw_movement", OpenClaws1 ());
    }

    IEnumerator OpenClaws1() {
      while (!this._state.IsTargetGrabbed()) {
        this._motor.transform.localPosition = Vector3.MoveTowards(
                                                                  current : this
                                                                            ._motor.transform.localPosition,
                                                                  target : this._default_motor_position,
                                                                  maxDistanceDelta : Time.deltaTime);
        yield return null; // new WaitForSeconds(waitTime);
      }
    }

    IEnumerator CloseClaws1() {
      while (!this._state.IsTargetGrabbed()) {
        this._motor.transform.localPosition = Vector3.MoveTowards(
                                                                  current : this
                                                                            ._motor.transform.localPosition,
                                                                  target : this._closed_motor_position,
                                                                  maxDistanceDelta : Time.deltaTime);
        yield return null; // new WaitForSeconds(waitTime);
      }
    }

    #endregion

    #region Collisions

    void Ab(GameObject child_game_object, GraspableObject other_maybe_graspable) {
      if (other_maybe_graspable) {
        if (child_game_object == this._grab_region.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject) {
          if (this._debugging)
            print(
                  message : string.Format(
                                          format : "Target {0} is inside region",
                                          arg0 : other_maybe_graspable.name));
          this._state.TargetIsInsideRegion();
        }

        if (child_game_object == this._begin_grab_region.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject
            && !this._state.IsTargetGrabbed()) {
          if (this._debugging)
            print(
                  message : string.Format(
                                          format : "Picking up target {0}",
                                          arg0 : other_maybe_graspable.name));
          this._state.PickUpTarget();
        }
      }
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject>();
      this.Ab(
              child_game_object : child_game_object,
              other_maybe_graspable : other_maybe_graspable);
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject>();
      this.Ab(
              child_game_object : child_game_object,
              other_maybe_graspable : other_maybe_graspable);
    }

    void OnCollisionStayChild(GameObject child_game_object, Collision collision) {
      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject>();
      if (other_maybe_graspable) {
        if (child_game_object == this._claw_1.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject) {
          if (this._debugging)
            print(
                  message : string.Format(
                                          format : "Target {0} is touching {1}",
                                          arg0 : other_maybe_graspable.name,
                                          arg1 : child_game_object.name));
          this._state.Claw1IsTouchingTarget();
        }

        if (child_game_object == this._claw_2.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject) {
          if (this._debugging)
            print(
                  message : string.Format(
                                          format : "Target {0} is touching {1}",
                                          arg0 : other_maybe_graspable.name,
                                          arg1 : child_game_object.name));
          this._state.Claw2IsTouchingTarget();
        }
      }
    }

    void OnCollisionExitChild(GameObject child_game_object, Collision collision) {
      /*if (collision.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Opening;
      }*/

      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject>();
      if (other_maybe_graspable) {
        if (child_game_object == this._claw_1.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject)
          this._state.Claw1IsNotTouchingTarget();

        if (child_game_object == this._claw_2.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject)
          this._state.Claw2IsNotTouchingTarget();
      }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      /*if (other_game_object.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Opening;
      }*/

      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject>();
      if (other_maybe_graspable)
        if (child_game_object == this._grab_region.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject)
          this._state.TargetIsOutsideRegion();
    }

    void OnCollisionEnterChild(GameObject child_game_object, Collision collision) {
      /*if (collision.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Closing;
      }*/

      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject>();
      if (other_maybe_graspable) {
        if (child_game_object == this._claw_1.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject)
          this._state.Claw1IsTouchingTarget();

        if (child_game_object == this._claw_2.gameObject
            && other_maybe_graspable.gameObject == this._target_game_object.gameObject)
          this._state.Claw2IsTouchingTarget();
      }
    }

    #endregion

    #region PathFinding

    Pair<GraspableObject, Grasp> GetOptimalTargetAndGrasp() {
      var targets = FindObjectsOfType<GraspableObject>();
      if (targets.Length == 0)
        return null;
      var shortest_distance = float.MaxValue;
      GraspableObject optimal_target = null;
      Grasp optimal_grasp = null;
      foreach (var target in targets) {
        var pair = target.GetOptimalGrasp(gripper : this);
        if (pair == null || pair.First == null || pair.First.IsObstructed())
          continue;
        var target_grasp = pair.First;
        var distance = pair.Second;
        if (distance < shortest_distance) {
          shortest_distance = distance;
          optimal_grasp = target_grasp;
          optimal_target = target;
        }
      }

      return new Pair<GraspableObject, Grasp>(
                                              first : optimal_target,
                                              second : optimal_grasp);
    }

    public void FindTargetAndUpdatePath() {
      var pair = this.GetOptimalTargetAndGrasp();
      if (pair == null || pair.First == null || pair.Second == null) {
        this._state.PathFindingState = PathFindingState.Returning;
        this._path = this.FindPath(
                                   start_position : this.transform.position,
                                   target_position : this._reset_position);
        return;
      }

      this._target_game_object = pair.First;
      this._target_grasp = pair.Second;
      this._approach_position = this._target_grasp.transform.position
                                - this._target_grasp.transform.forward * this._approach_distance;
      if (Vector3.Distance(
                           a : this.transform.position,
                           b : this._approach_position)
          > this._search_boundary)
        return;
      this._path = this.FindPath(
                                 start_position : this.transform.position,
                                 target_position : this._approach_position);
      this._intermediate_target = this._path.Next(step_size : this._step_size);
    }

    BezierCurvePath FindPath(Vector3 start_position, Vector3 target_position) {
      this.UpdateMeshFilterBounds();
      var path_list = PathFinding.FindPathAstar(
                                                 source : start_position,
                                                 destination : target_position,
                                                 search_boundary : this._search_boundary,
                                                 grid_granularity : this._grid_granularity,
                                                 agent_size : this._actor_size,
                                                 near_stopping_distance : this._approach_distance);
      if (path_list != null && path_list.Count > 0) {
        path_list = PathFinding.SimplifyPath(
                                              path : path_list,
                                              sphere_cast_radius : this._actor_size);
        path_list.Add(item : target_position);
      } else {
        path_list = new List<Vector3> {
                                         start_position,
                                         target_position
                                       };
      }

      var path = new BezierCurvePath(
                                     start_position : start_position,
                                     target_position : target_position,
                                     game_object : this._bezier_curve,
                                     path_list : path_list);
      return path;
    }

    void ApproachTarget(float step_size) {
      this.transform.position =
        Vector3.MoveTowards(
                            current : this.transform.position,
                            target : this._target_grasp.transform.position,
                            maxDistanceDelta : step_size);
      if (this._debugging)
        Debug.DrawLine(
                       start : this.transform.position,
                       end : this._target_grasp.transform.position,
                       color : Color.green);
    }

    void FollowPathToApproach(float step_size, Quaternion rotation, bool rotate = true) {
      if (Vector3.Distance(
                           a : this.transform.position,
                           b : this._intermediate_target)
          <= this._precision)
        this._intermediate_target = this._path.Next(step_size : step_size);

      if (this._debugging)
        Debug.DrawRay(
                      start : this._intermediate_target,
                      dir : this.transform.forward,
                      color : Color.magenta);

      if (rotate)
        this.transform.rotation = Quaternion.RotateTowards(
                                                           @from : this.transform.rotation,
                                                           to : rotation,
                                                           maxDegreesDelta : step_size * 50);
      this.transform.position = Vector3.MoveTowards(
                                                    current : this.transform.position,
                                                    target : this._intermediate_target,
                                                    maxDistanceDelta : step_size);
    }

    #endregion

    /*
       *  
      public GraspableObject TargetGameObject
      {
        get { return _target_game_object; }
        set
        {
          _target_game_object = value;

          StopCoroutine("gripper_movement");
          StartCoroutine("gripper_movement", FollowPathToApproach1(_target_game_object.transform));
        }
      }

       IEnumerator FollowPathToApproach1(Transform trans){
        while (true){
          if ((Vector3.Distance(this.transform.position, _intermediate_target) <= _precision)) {
            _intermediate_target = _path.Next(1);
          }

          if (Debugging) Debug.DrawRay(_intermediate_target, this.transform.forward, Color.green);
          transform.position = Vector3.MoveTowards(this.transform.position, _intermediate_target, 1);
        }
      }

       * 
       * 
    public void respawn_obstructions(GripperState state) {
      ObstacleSpawner obstacles_spawner = FindObjectOfType<ObstacleSpawner>();
      obstacles_spawner.SpawnObstacles(obstacles_spawner.number_of_cubes, obstacles_spawner.number_of_spheres);
    }*/
    public States State { get { return this._state; } set { this._state = value; } }
  }
}
