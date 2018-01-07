using UnityEngine;

public class FestoBehaviour : MonoBehaviour {
  private Rigidbody[] _children;
  public bool _find_global_rigidbodies;
  public float _torque_scalar;

  private void Awake() { _children = _find_global_rigidbodies ? FindObjectsOfType<Rigidbody>() : GetComponentsInChildren<Rigidbody>(); }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.UpArrow))
      _torque_scalar += 100;
    else if (Input.GetKeyDown(KeyCode.DownArrow))
      _torque_scalar -= 100;
  }

  private void FixedUpdate() {
    foreach (var body in _children)
      if (body.gameObject != this)
        body.AddRelativeTorque(Vector3.forward * _torque_scalar);
  }
}
