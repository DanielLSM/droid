using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class SteeringMotor : Motor {

    WheelCollider _wheel_collider;

    private void Start () {
      _wheel_collider = GetComponent<WheelCollider> ();
      RegisterComponent ();
    }

    private void FixedUpdate () {
      ApplyLocalPositionToVisuals (_wheel_collider);
    }

    public override void ApplyMotion (MotorMotion motion) {
      if (_debug)
        Debug.Log ("Applying " + motion.ToString () + " To " + name);
      if (motion.Strength < _min_strength || motion.Strength > _max_strength) {
        Debug.Log ("It does not accept input, outside allowed range");
        return; // Do nothing
      }
      _wheel_collider.steerAngle = motion.Strength;
      _energy_spend_since_reset += _energy_cost * motion.Strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Steering";
    }

    public void ApplyLocalPositionToVisuals (WheelCollider collider) {
      if (collider.transform.childCount == 0) {
        return;
      }

      Transform visualWheel = collider.transform.GetChild (0);

      Vector3 position;
      Quaternion rotation;
      collider.GetWorldPose (out position, out rotation);

      visualWheel.transform.position = position;
      visualWheel.transform.rotation = rotation;
    }
  }
}
