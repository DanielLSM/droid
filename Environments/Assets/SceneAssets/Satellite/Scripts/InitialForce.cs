using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InitialForce : MonoBehaviour {
  public bool _on_awake = true;

  private Rigidbody _rb;
  public bool _relative;
  public bool _torque;

  [SerializeField]
  private Vector3 force;

  private void ApplyInitialForce() {
    if (_torque) {
      if (_relative)
        _rb.AddRelativeTorque(
                              force,
                              ForceMode.Impulse);
      else
        _rb.AddTorque(
                      force,
                      ForceMode.Impulse);
    } else {
      if (_relative)
        _rb.AddRelativeForce(
                             force,
                             ForceMode.Impulse);
      else
        _rb.AddForce(
                     force,
                     ForceMode.Impulse);
    }
  }

  private void Awake() {
    _rb = GetComponent<Rigidbody>();

    if (_on_awake) ApplyInitialForce();
  }

  private void Start() {
    if (!_on_awake) ApplyInitialForce();
  }
}
