using UnityEngine;
using System.Collections;
using SceneSpecificAssets.Grasping.Navigation;
using SceneSpecificAssets.Grasping.Utilities;
using SceneSpecificAssets.Grasping.Grasps;
using System.Collections.Generic;
using Neodroid.Utilities;

namespace SceneSpecificAssets.Grasping {
  public class ScriptedGripper : MonoBehaviour {

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

    [Space (1)]
    [Header ("Game Objects")]
    public GameObject _motor;
    public GameObject _grab_region;
    public GameObject _begin_grab_region;
    public GameObject _claw_1, _claw_2;
    public States _state;
    public Transform _closed_motor_transform;
    public ObstacleSpawner _obstacle_spawner;
    public BezierCurve _bezier_curve_prefab;

    [Space (1)]
    [Header ("Path Finding Parameters")]
    public float _search_boundary = 6f;
    public float _agent_size = 0.3f;
    public float _grid_granularity = 0.4f;
    public float _speed = 0.5f;
    public float _precision = 0.02f;
    public float _sensitivity = 0.2f;
    public float _approach_distance = 0.6f;
    public bool _wait_for_resting_environment = false;

    [Space (1)]
    [Header ("Show debug logs")]
    public bool _debug = false;

    [Space (1)]
    [Header ("Draw Search Boundary")]
    public bool _draw_search_boundary = true;

    #endregion

    #region Setup

    void UpdateMeshFilterBounds () {
      var agent_bounds = GraspingUtilities.GetMaxBounds (this.gameObject);
      //_agent_size = agent_bounds.size.magnitude;
      //_approach_distance = agent_bounds.size.magnitude + _precision;
      _agent_size = agent_bounds.extents.magnitude * 2; //Mathf.Max(agent_bounds.extents.x, Mathf.Max(agent_bounds.extents.y, agent_bounds.extents.z)) * 2;
      _approach_distance = _agent_size + _precision;
    }

    void SetupEnvironment () {
      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (transform, OnCollisionEnterChild, OnTriggerEnterChild, OnCollisionExitChild, OnTriggerExitChild, OnCollisionStayChild, OnTriggerStayChild);
    }

    #endregion

    #region UnityCallbacks

    void Start () {
      _state = new States ();
      _reset_position = transform.position;
      _default_motor_position = _motor.transform.localPosition;
      _closed_motor_position = _closed_motor_transform.localPosition;
      _bezier_curve = FindObjectOfType<BezierCurve> ();
      if (!_bezier_curve) {
        _bezier_curve = Instantiate (_bezier_curve_prefab);
      }

      UpdateMeshFilterBounds ();

      FindTargetAndUpdatePath ();
      _state.ResetStates ();
      SetupEnvironment ();
    }

    void Update () {
      _step_size = _speed * Time.deltaTime;
      if (_draw_search_boundary)
        GraspingUtilities.DrawBoxFromCenter (this.transform.position, _search_boundary, Color.magenta);
      _state.ObstructionMotionState = _state.GetMotionState (FindObjectsOfType<Obstruction> (), _state.ObstructionMotionState, _sensitivity);
      _state.TargetMotionState = _state.GetMotionState (FindObjectsOfType<GraspableObject> (), _state.TargetMotionState, _sensitivity);

      PerformReactionToCurrentState (_state);
    }

    #endregion

    #region Helpers

    void PerformReactionToCurrentState (States state) {
      switch (state.PathFindingState) {
      case PathFindingState.WaitingForTarget:
        FindTargetAndUpdatePath ();
        if (_target_grasp != null)
          state.NavigateToTarget ();
        break;

      case PathFindingState.Navigating:
        if (state.IsEnvironmentMoving () && _wait_for_resting_environment) {
          state.WaitForRestingEnvironment ();
          break;
        }
        if (state.WasEnvironmentMoving ()) {
          FindTargetAndUpdatePath ();
        }
        FollowPathToApproach (_step_size, _target_grasp.transform.rotation, true);
        state.OpenClaw ();
        CheckIfGripperIsOpen ();
        MaybeBeginApproachProcedure ();
        break;

      case PathFindingState.Approaching:
        ApproachTarget (_step_size);
        break;

      case PathFindingState.PickingUpTarget:
        if (state.IsGripperClosed () && state.IsTargetNotGrabbed ()) {
          //state.ReturnToStartPosition();
        }
        break;

      case PathFindingState.WaitingForRestingEnvironment:
        if (state.WasEnvironmentMoving ()) {
          FindTargetAndUpdatePath ();
        }
        if (state.IsEnvironmentAtRest ()) {
          FindTargetAndUpdatePath ();
          if (state.IsTargetGrabbed ())
            state.ReturnToStartPosition ();
          else
            state.NavigateToTarget ();
        }
        break;

      case PathFindingState.Returning:
        if (state.WereObstructionMoving ())
          _path = FindPath (this.transform.position, _reset_position);
        if (_wait_for_resting_environment) {
          if (state.IsObstructionsAtRest ()) {
            FollowPathToApproach (_step_size, Quaternion.Euler (90, 0, 0), true);
            MaybeBeginReleaseProcedure ();
          }
        } else {
          FollowPathToApproach (_step_size, Quaternion.Euler (90, 0, 0), true);
          MaybeBeginReleaseProcedure ();
        }
        break;
      }

      switch (state.GripperState) {
      case GripperState.Opening:
        OpenClaws (_step_size);
        break;

      case GripperState.Closing:
        CloseClaws (_step_size);
        break;

      case GripperState.Closed:
        break;
      }

      if (!state.IsTargetInsideRegionOrTouching ()) {
        state.TargetIsNotGrabbed ();
        if (_target_game_object != null)
          _target_game_object.transform.parent = null;
      }

      if (state.IsTargetTouchingAndInsideRegion ()) {
        _target_game_object.transform.parent = this.transform;
        state.TargetIsGrabbed ();
        _path = FindPath (this.transform.position, _reset_position);
        _intermediate_target = _path.Next (0.001f);
      }

      MaybeClawIsClosed ();
    }

    #endregion

    #region StateTransitions

    void MaybeBeginReleaseProcedure () {
      if (Vector3.Distance (this.transform.position, _reset_position) < _precision) {
        if (_target_game_object) {
          _target_game_object.transform.parent = null;
          //if(_state.IsTargetGrabbed()) Destroy(_target_game_object.gameObject);
        }
        _state.OpenClaw ();
        _state.WaitForTarget ();

        if (_obstacle_spawner != null) {
          _obstacle_spawner.Setup ();
        }
      }
    }

    void MaybeClawIsClosed () {
      if (Vector3.Distance (this._motor.transform.localPosition, _closed_motor_position) < _precision) {
        _state.GripperIsClosed ();
        _path = FindPath (this.transform.position, _reset_position);
        _intermediate_target = _path.Next (0.001f);
        _state.ReturnToStartPosition ();
      }
    }

    void MaybeBeginApproachProcedure () {
      if (Vector3.Distance (this.transform.position, _path._target_position) < _approach_distance + _precision &&
          Quaternion.Angle (this.transform.rotation, _target_grasp.transform.rotation) < _precision &&
          _state.IsGripperOpen ()) {
        _state.ApproachTarget ();
      }
    }

    void CheckIfGripperIsOpen () {
      if (Vector3.Distance (_motor.transform.localPosition, _default_motor_position) < _precision) {
        _state.GripperIsOpen ();
      }
    }

    void CloseClaws (float step_size) {
      //Vector3 current_motor_position = _motor.transform.localPosition;
      //current_motor_position.y += step_size / 16;
      //_motor.transform.localPosition = current_motor_position;
      _motor.transform.localPosition = Vector3.MoveTowards (_motor.transform.localPosition, _closed_motor_position, step_size / 8);
    }

    void OpenClaws (float step_size) {
      _motor.transform.localPosition = Vector3.MoveTowards (_motor.transform.localPosition, _default_motor_position, step_size / 8);
      //StopCoroutine ("claw_movement");
      //StartCoroutine ("claw_movement", OpenClaws1 ());
    }

    IEnumerator OpenClaws1 () {
      while (!_state.IsTargetGrabbed ()) {
        _motor.transform.localPosition = Vector3.MoveTowards (_motor.transform.localPosition, _default_motor_position, Time.deltaTime);
        yield return null; // new WaitForSeconds(waitTime);
      }
    }

    IEnumerator CloseClaws1 () {
      while (!_state.IsTargetGrabbed ()) {
        _motor.transform.localPosition = Vector3.MoveTowards (_motor.transform.localPosition, _closed_motor_position, Time.deltaTime);
        yield return null; // new WaitForSeconds(waitTime);
      }
    }



    #endregion

    #region Collisions

    void OnTriggerEnterChild (GameObject child_game_object, Collider other_game_object) {
      if (other_game_object.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Closing;
      }

      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _begin_grab_region.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.PickUpTarget ();
        }

        if (child_game_object.name == _grab_region.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.TargetIsInsideRegion ();
        }
      }
    }

    void OnTriggerStayChild (GameObject child_game_object, Collider other_game_object) {
      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _grab_region.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.TargetIsInsideRegion ();
        }

        if (child_game_object.name == _begin_grab_region.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.PickUpTarget ();
        }
      }
    }

    void OnCollisionStayChild (GameObject child_game_object, Collision collision) {
      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _claw_1.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw1IsTouchingTarget ();
        }

        if (child_game_object.name == _claw_2.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw2IsTouchingTarget ();
        }
      }
    }

    void OnCollisionExitChild (GameObject child_game_object, Collision collision) {
      if (collision.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Opening;
      }


      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _claw_1.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw1IsNotTouchingTarget ();
        }

        if (child_game_object.name == _claw_2.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw2IsNotTouchingTarget ();
        }
      }
    }

    void OnTriggerExitChild (GameObject child_game_object, Collider other_game_object) {
      if (other_game_object.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Opening;
      }

      var other_maybe_graspable = other_game_object.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _grab_region.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.TargetIsOutsideRegion ();
        }
      }
    }

    void OnCollisionEnterChild (GameObject child_game_object, Collision collision) {
      if (collision.gameObject.GetComponent<Obstruction> () != null) {
        _state.GripperState = GripperState.Closing;
      }

      var other_maybe_graspable = collision.gameObject.GetComponentInParent<GraspableObject> ();
      if (other_maybe_graspable && other_maybe_graspable.tag != "FishPart") {
        if (child_game_object.name == _claw_1.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw1IsTouchingTarget ();
        }

        if (child_game_object.name == _claw_2.name && other_maybe_graspable.name == _target_game_object.name) {
          _state.Claw2IsTouchingTarget ();
        }
      }
    }

    #endregion

    #region PathFinding

    Pair<GraspableObject, Grasp> GetOptimalTargetAndGrasp () {
      var targets = FindObjectsOfType<GraspableObject> ();
      if (targets.Length == 0) {
        return null;
      }
      float shortest_distance = float.MaxValue;
      GraspableObject optimal_target = null;
      Grasp optimal_grasp = null;
      foreach (GraspableObject target in targets) {
        var pair = target.GetOptimalGrasp (this);
        if (pair != null && pair.First != null && !pair.First.IsObstructed ()) {
          var target_grasp = pair.First;
          var distance = pair.Second;
          if (distance < shortest_distance) {
            shortest_distance = distance;
            optimal_grasp = target_grasp;
            optimal_target = target;
          }
        }
      }
      return new Pair<GraspableObject, Grasp> (optimal_target, optimal_grasp);
    }

    public void FindTargetAndUpdatePath () {
      var pair = GetOptimalTargetAndGrasp ();
      if (pair == null || pair.First == null || pair.Second == null) {
        _state.PathFindingState = PathFindingState.Returning;
        _path = FindPath (this.transform.position, _reset_position);
        return;
      }
      _target_game_object = pair.First;
      _target_grasp = pair.Second;
      _approach_position = _target_grasp.transform.position - (_target_grasp.transform.forward * _approach_distance);
      if (Vector3.Distance (this.transform.position, _approach_position) > _search_boundary)
        return;
      _path = FindPath (this.transform.position, _approach_position);
      _intermediate_target = _path.Next (_step_size);
    }

    BezierCurvePath FindPath (Vector3 start_position, Vector3 target_position) {
      UpdateMeshFilterBounds ();
      var _path_list = PathFinding.FindPathAstar (start_position, target_position, _search_boundary, _grid_granularity, _agent_size, _approach_distance);
      if (_path_list != null && _path_list.Count > 0) {
        _path_list = PathFinding.SimplifyPath (_path_list, _agent_size);
        _path_list.Add (target_position);
      } else {
        _path_list = new List<Vector3>{ start_position, target_position };
      }
      var path = new BezierCurvePath (start_position, target_position, _bezier_curve, _path_list);
      return path;
    }

    void ApproachTarget (float step_size) {
      transform.position = Vector3.MoveTowards (this.transform.position, _target_grasp.transform.position, step_size);
      if (_debug)
        Debug.DrawLine (this.transform.position, _target_grasp.transform.position, Color.green);
    }

    void FollowPathToApproach (float step_size, Quaternion rotation, bool rotate = true) {
      if ((Vector3.Distance (this.transform.position, _intermediate_target) <= _precision)) {
        _intermediate_target = _path.Next (step_size);
      }

      if (_debug)
        Debug.DrawRay (_intermediate_target, this.transform.forward, Color.magenta);

      if (rotate)
        transform.rotation = Quaternion.RotateTowards (this.transform.rotation, rotation, step_size * 50);
      transform.position = Vector3.MoveTowards (this.transform.position, _intermediate_target, step_size);
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

      if (_debug) Debug.DrawRay(_intermediate_target, this.transform.forward, Color.green);
      transform.position = Vector3.MoveTowards(this.transform.position, _intermediate_target, 1);
    }
  }

   * 
   * 
public void respawn_obstructions(GripperState state) {
  ObstacleSpawner obstacles_spawner = FindObjectOfType<ObstacleSpawner>();
  obstacles_spawner.SpawnObstacles(obstacles_spawner.number_of_cubes, obstacles_spawner.number_of_spheres);
}*/
  }
}