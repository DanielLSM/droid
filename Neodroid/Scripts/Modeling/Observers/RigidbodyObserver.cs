using UnityEngine;
using Neodroid.Utilities;
using System.Text;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyObserver : Observer {

    Rigidbody _rigidbody;

    protected override void Start () {
      AddToEnvironment ();
      _rigidbody = this.GetComponent<Rigidbody> ();
    }

    public override byte[] GetData () {
      var str_rep = "{";
      str_rep += "\"Velocity\": \"" + _rigidbody.velocity.ToString ();
      str_rep += "\", \"AngularVelocity\": \"" + _rigidbody.angularVelocity.ToString ();
      str_rep += "\"}";
      _data = Encoding.ASCII.GetBytes (str_rep);
      return _data;
    }

    public override string GetObserverIdentifier () {
      return name + "Rigidbody";
    }
  }
}