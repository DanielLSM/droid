using UnityEngine;

namespace SceneAssets.Satellite.Scripts {
  [RequireComponent(typeof(Rigidbody))]
  public class BodyWithMass : MonoBehaviour {
    const float GravitationalConstant = 667.4f;

    static BodyWithMass[] _attractors;

    Rigidbody _rigidbody;

    public float Mass { get { return this._rigidbody.mass; } }

    void Start() {
      if (!this._rigidbody) this._rigidbody = this.GetComponent<Rigidbody>();
      if (_attractors == null) _attractors = FindObjectsOfType<BodyWithMass>();
    }

    void Update() {
      if (!this._rigidbody) this._rigidbody = this.GetComponent<Rigidbody>();
      if (_attractors == null) _attractors = FindObjectsOfType<BodyWithMass>();
    }

    void FixedUpdate() {
      foreach (var attractor in _attractors) {
        if (attractor != this)
          this.Attract(attractor);
      }
    }

    void Attract(BodyWithMass other_body) {
      var direction = this.transform.position - other_body.transform.position;
      //float distance = direction.sqrMagnitude;
      var distance = direction.magnitude;

      if (Mathf.Approximately(distance, 0)) return;

      var nom = this.Mass * other_body.Mass;
      var denom = distance * distance;

      var force_magnitude = nom / denom;
      force_magnitude *= GravitationalConstant;
      var force = direction.normalized * force_magnitude;

      other_body.ApplyForce(force);
    }

    public void ApplyForce(Vector3 force) { this._rigidbody.AddForce(force); }
  }
}
