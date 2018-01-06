﻿using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;
using UnityEngine;
using System.Text;

namespace Neodroid.Observers {
  public class NearestByTagObserver : Observer, HasEulerTransformProperties {
    [Header ("Specific", order = 102)]
    [SerializeField]
    GameObject _nearest_object;
    [SerializeField]
    string _tag = "";

    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Vector3 _rotation;
    [SerializeField]
    Vector3 _direction;

    public Vector3 Position {
      get {
        return _position;
      }
    }

    public Vector3 Rotation {
      get {
        return _rotation;
      }
    }

    public Vector3 Direction {
      get {
        return _direction;
      }
    }


    public override void UpdateData () {
      FindNearest ();
      if (ParentEnvironment) {
        _position = ParentEnvironment.TransformPosition (_nearest_object.transform.position);
        _direction = ParentEnvironment.TransformDirection (_nearest_object.transform.forward);
        _rotation = ParentEnvironment.TransformDirection (_nearest_object.transform.up);
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
      //Data = Encoding.ASCII.GetBytes (str_rep);
    }

    public override string ObserverIdentifier{ get { return name + "NearestByTag"; } }

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