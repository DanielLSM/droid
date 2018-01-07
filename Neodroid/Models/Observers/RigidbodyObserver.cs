using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyObserver : Observer,
                                   HasRigidbodyProperties {
    [SerializeField]
    private Vector3 _angular_velocity;

    private Rigidbody _rigidbody;

    [Header(
      "Observation",
      order = 103)]
    [SerializeField]
    private Vector3 _velocity;

    public override string ObserverIdentifier { get { return name + "Rigidbody"; } }

    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }

    public Vector3 AngularVelocity { get { return _angular_velocity; } set { _angular_velocity = value; } }

    protected override void Start() { _rigidbody = GetComponent<Rigidbody>(); }

    public override void UpdateData() {
      _velocity = _rigidbody.velocity;
      _angular_velocity = _rigidbody.angularVelocity;

      var str_rep = "{";
      str_rep += "\"Velocity\": \"" + _velocity;
      str_rep += "\", \"AngularVelocity\": \"" + _angular_velocity;
      str_rep += "\"}";
      //Data = Encoding.ASCII.GetBytes (str_rep);
    }
  }
}
