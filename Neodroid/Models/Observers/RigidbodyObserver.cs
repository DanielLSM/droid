using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyObserver : Observer,
                                   IHasRigidbodyProperties {
    [SerializeField] Vector3 _angular_velocity;

    Rigidbody _rigidbody;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _velocity;

    public override string ObserverIdentifier { get { return this.name + "Rigidbody"; } }

    public Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    protected override void Start() { this._rigidbody = this.GetComponent<Rigidbody>(); }

    public override void UpdateData() {
      this._velocity = this._rigidbody.velocity;
      this._angular_velocity = this._rigidbody.angularVelocity;

      var str_rep = "{";
      str_rep += "\"Velocity\": \"" + this._velocity;
      str_rep += "\", \"AngularVelocity\": \"" + this._angular_velocity;
      str_rep += "\"}";
      //Data = Encoding.ASCII.GetBytes (str_rep);
    }
  }
}
