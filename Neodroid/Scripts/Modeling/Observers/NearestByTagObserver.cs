using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;
using UnityEngine;
using System.Text;

namespace Neodroid.Observers {
  public class NearestByTagObserver : Observer {
 
    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    GameObject _nearest_object;
    public string _tag = "";

    public override void UpdateData () {
      FindNearest ();
      if (_environment) {
        _position = _environment.TransformPosition (_nearest_object.transform.position);
        _direction = _environment.TransformDirection (_nearest_object.transform.forward);
        _rotation = _environment.TransformDirection (_nearest_object.transform.up);
      } else {
        _position = _nearest_object.transform.position;
        _direction = _nearest_object.transform.forward;
        _rotation = _nearest_object.transform.up;
      }

      var str_rep = "{";
      if (_nearest_object) {
        str_rep += "\"NearestIdentifier\": \"" + _nearest_object.name;
      } else {
        str_rep += "\"NearestIdentifier\": \"" + "None";
      }
      str_rep += "\"}";
      _data = Encoding.ASCII.GetBytes (str_rep);
    }

    public override string GetObserverIdentifier () {
      return name + "NearestByTag";
    }

    void FindNearest () {
      var candidates = FindObjectsOfType<GameObject> ();
      var nearest_distance = -1.0;
      foreach (var candidate in candidates) {
        if (candidate.tag == _tag) {
          var dist = Vector3.Distance (this.transform.position, candidate.transform.position);
          if (nearest_distance > dist || nearest_distance < 0) {
            nearest_distance = dist;
            _nearest_object = candidate;
          }
        }
      }
    }
  }
}