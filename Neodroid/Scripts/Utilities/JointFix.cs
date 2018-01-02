using UnityEngine;

namespace Neodroid.Utilities {
  [RequireComponent (typeof(Joint))]
  public class JointFix : MonoBehaviour {
    Quaternion initial_local_rotation;
    Vector3 initial_local_position;

    Quaternion local_rotation_on_disable;
    Vector3 local_position_on_disable;

    Joint[] joints;

    bool hasDisabled;

    void Awake () {
      this.initial_local_rotation = this.transform.localRotation;
      this.initial_local_position = this.transform.localPosition;
      joints = GetComponents<Joint> ();
    }

    void OnDisable () {
      this.local_rotation_on_disable = this.transform.localRotation;
      this.transform.localRotation = this.initial_local_rotation;

      this.local_position_on_disable = this.transform.localPosition;
      this.transform.localPosition = this.initial_local_position;

      this.hasDisabled = true;
    }

    void Update () {
      if (this.hasDisabled) {
        this.hasDisabled = false;
        this.transform.localRotation = this.local_rotation_on_disable;
        this.transform.localPosition = this.local_position_on_disable;
      }
    }

    public void Reset () {
      this.transform.localRotation = this.initial_local_rotation;
      this.transform.localPosition = this.initial_local_position;
      foreach (var joint in joints) {
        if (joint is HingeJoint) {
          ((HingeJoint)joint).limits = ((HingeJoint)joint).limits;
        } else if (joint is ConfigurableJoint) {
          ((ConfigurableJoint)joint).lowAngularXLimit = ((ConfigurableJoint)joint).lowAngularXLimit;
          ((ConfigurableJoint)joint).highAngularXLimit = ((ConfigurableJoint)joint).highAngularXLimit;
        }
      }
    }
  }
}