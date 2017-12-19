using UnityEngine;
using Neodroid.Utilities;
using System.Text;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyObserver : Observer {

    public Vector3 _velocity;
    public Vector3 _angular_velocity;

    Rigidbody _rigidbody;


    protected override void Start () {
      _rigidbody = this.GetComponent<Rigidbody> ();
    }

    public override void UpdateData () {
      _velocity = _rigidbody.velocity;
      _angular_velocity = _rigidbody.angularVelocity;

      var str_rep = "{";
      str_rep += "\"Velocity\": \"" + _velocity;
      str_rep += "\", \"AngularVelocity\": \"" + _angular_velocity;
      str_rep += "\"}";
      _data = Encoding.ASCII.GetBytes (str_rep);
    }

    public override string GetObserverIdentifier () {
      return name + "Rigidbody";
    }
  }
}