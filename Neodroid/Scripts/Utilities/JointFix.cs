using UnityEngine;

namespace Neodroid.Utilities {
  [RequireComponent (typeof(Joint))]
  public class JointFix : MonoBehaviour {
    private JointDrive[] _angular_x_drive;
    private Rigidbody[] _connected_bodies;
    private float[] _force_break_limits;
    private System.Type[] _joint_types;

    private Joint[] _joints;
    private JointLimits[] _limits;
    private Quaternion[] _target_rotations;
    private float[] _torque_break_limits;
    private SoftJointLimit[] _x_ang_high_limits;
    private SoftJointLimit[] _x_ang_low_limits;
    private ConfigurableJointMotion[] _x_ang_motion;
    private ConfigurableJointMotion[] _x_motion;
    private ConfigurableJointMotion[] _y_ang_motion;
    private ConfigurableJointMotion[] _y_motion;
    private ConfigurableJointMotion[] _z_ang_motion;
    private ConfigurableJointMotion[] _z_motion;

    private bool hasDisabled;
    private Vector3 initial_local_position;
    private Quaternion initial_local_rotation;
    private Vector3 local_position_on_disable;

    private Quaternion local_rotation_on_disable;

    private void Awake () {
      initial_local_rotation = transform.localRotation;
      initial_local_position = transform.localPosition;
      _joints = GetComponents<Joint> ();
      _connected_bodies = new Rigidbody[_joints.Length];
      _joint_types = new System.Type[_joints.Length];
      _x_ang_low_limits = new SoftJointLimit[_joints.Length];
      _x_ang_high_limits = new SoftJointLimit[_joints.Length];
      _limits = new JointLimits[_joints.Length];
      _force_break_limits = new float[_joints.Length];
      _torque_break_limits = new float[_joints.Length];
      _x_motion = new ConfigurableJointMotion[_joints.Length];
      _y_motion = new ConfigurableJointMotion[_joints.Length];
      _z_motion = new ConfigurableJointMotion[_joints.Length];
      _x_ang_motion = new ConfigurableJointMotion[_joints.Length];
      _y_ang_motion = new ConfigurableJointMotion[_joints.Length];
      _z_ang_motion = new ConfigurableJointMotion[_joints.Length];
      _angular_x_drive = new JointDrive[_joints.Length];
      _target_rotations = new Quaternion[_joints.Length];

      for (var i = 0; i < _joints.Length; i++) {
        _connected_bodies [i] = _joints [i].connectedBody;
        _joint_types [i] = _joints [i].GetType ();
        _force_break_limits [i] = _joints [i].breakForce;
        _torque_break_limits [i] = _joints [i].breakTorque;
        if (_joints [i] is HingeJoint) {
          _limits [i] = ((HingeJoint)_joints [i]).limits;
        } else if (_joints [i] is ConfigurableJoint) {
          _x_ang_low_limits [i] = ((ConfigurableJoint)_joints [i]).lowAngularXLimit;
          _x_ang_high_limits [i] = ((ConfigurableJoint)_joints [i]).highAngularXLimit;
          _x_motion [i] = ((ConfigurableJoint)_joints [i]).xMotion;
          _y_motion [i] = ((ConfigurableJoint)_joints [i]).yMotion;
          _z_motion [i] = ((ConfigurableJoint)_joints [i]).zMotion;
          _x_ang_motion [i] = ((ConfigurableJoint)_joints [i]).angularXMotion;
          _y_ang_motion [i] = ((ConfigurableJoint)_joints [i]).angularYMotion;
          _z_ang_motion [i] = ((ConfigurableJoint)_joints [i]).angularZMotion;
          _angular_x_drive [i] = ((ConfigurableJoint)_joints [i]).angularXDrive;
          _target_rotations [i] = ((ConfigurableJoint)_joints [i]).targetRotation;
        }
      }
    }

    private void OnDisable () {
      local_rotation_on_disable = transform.localRotation;
      transform.localRotation = initial_local_rotation;

      local_position_on_disable = transform.localPosition;
      transform.localPosition = initial_local_position;

      hasDisabled = true;
    }

    private void Update () {
      if (hasDisabled) {
        hasDisabled = false;
        transform.localRotation = local_rotation_on_disable;
        transform.localPosition = local_position_on_disable;
      }
    }

    public void Reset () {
      transform.localRotation = initial_local_rotation;
      transform.localPosition = initial_local_position;
      for (var i = 0; i < _joints.Length; i++) {
        if (_joints [i] == null) {
          _joints [i] = (Joint)gameObject.AddComponent (_joint_types [i]);
          _joints [i].connectedBody = _connected_bodies [i];
        }

        _joints [i].breakForce = _force_break_limits [i];
        _joints [i].breakTorque = _torque_break_limits [i];

        if (_joints [i] is HingeJoint) {
          ((HingeJoint)_joints [i]).limits = _limits [i];
        } else if (_joints [i] is ConfigurableJoint) {
          ((ConfigurableJoint)_joints [i]).lowAngularXLimit = _x_ang_low_limits [i];
          ((ConfigurableJoint)_joints [i]).highAngularXLimit = _x_ang_high_limits [i];
          ((ConfigurableJoint)_joints [i]).xMotion = _x_motion [i];
          ((ConfigurableJoint)_joints [i]).yMotion = _y_motion [i];
          ((ConfigurableJoint)_joints [i]).zMotion = _z_motion [i];
          ((ConfigurableJoint)_joints [i]).angularXMotion = _x_ang_motion [i];
          ((ConfigurableJoint)_joints [i]).angularYMotion = _y_ang_motion [i];
          ((ConfigurableJoint)_joints [i]).angularZMotion = _z_ang_motion [i];
          ((ConfigurableJoint)_joints [i]).angularXDrive = _angular_x_drive [i];
          ((ConfigurableJoint)_joints [i]).targetRotation = _target_rotations [i];
        }
      }
    }
  }
}
