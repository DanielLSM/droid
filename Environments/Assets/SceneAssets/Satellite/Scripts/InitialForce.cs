using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class InitialForce : MonoBehaviour {

  [SerializeField]
  Vector3 force;

  public bool _on_awake = true;
  public bool _relative = false;
  public bool _torque = false;

  Rigidbody _rb;


  void ApplyInitialForce () {
    if (_torque) {
      if (_relative)
        _rb.AddRelativeTorque (force, ForceMode.Impulse);
      else
        _rb.AddTorque (force, ForceMode.Impulse);
    } else {
      if (_relative)
        _rb.AddRelativeForce (force, ForceMode.Impulse);
      else
        _rb.AddForce (force, ForceMode.Impulse);
    }
  }

  void Awake () {
    _rb = GetComponent<Rigidbody> ();

    if (_on_awake) {
      ApplyInitialForce ();
    }
  }

  void Start () {
    if (!_on_awake) {
      ApplyInitialForce ();
    }
  }
	
}
