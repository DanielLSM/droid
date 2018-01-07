using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyWithMass : MonoBehaviour {
  private const float GRAVITATIONAL_CONSTANT = 667.4f;

  private static BodyWithMass[] _attractors;

  private Rigidbody _rigidbody;

  public float Mass { get { return _rigidbody.mass; } }

  private void Start() {
    if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    if (_attractors == null) _attractors = FindObjectsOfType<BodyWithMass>();
  }

  private void Update() {
    if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    if (_attractors == null) _attractors = FindObjectsOfType<BodyWithMass>();
  }

  private void FixedUpdate() {
    foreach (var attractor in _attractors)
      if (attractor != this)
        Attract(attractor);
  }

  private void Attract(BodyWithMass other_body) {
    var direction = transform.position - other_body.transform.position;
    //float distance = direction.sqrMagnitude;
    var distance = direction.magnitude;

    if (Mathf.Approximately(
                            distance,
                            0)) return;

    var nom = Mass * other_body.Mass;
    var denom = distance * distance;

    var force_magnitude = nom / denom;
    force_magnitude *= GRAVITATIONAL_CONSTANT;
    var force = direction.normalized * force_magnitude;

    other_body.ApplyForce(force);
  }

  public void ApplyForce(Vector3 force) { _rigidbody.AddForce(force); }
}
