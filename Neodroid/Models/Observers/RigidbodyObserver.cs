﻿using System;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  [RequireComponent (typeof(Rigidbody))]
  public class RigidbodyObserver : Observer,
                                   IHasRigidbody {
    [Header ("Specfic", order = 103)]
    [SerializeField]
    Rigidbody _rigidbody;

    [SerializeField]
    bool _differential = false;
    [SerializeField]
    float _last_update_time;

    [Header ("Observation", order = 103)]
    [SerializeField] Vector3 _angular_velocity;

    [SerializeField]
    Vector3 _velocity;

    public override string ObserverIdentifier {
      get { 
        if (_differential) {
          return this.name + "RigidbodyDifferential"; 
        }
        return this.name + "Rigidbody"; 
      }
    }

    public Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    protected override void Start () {
      this._rigidbody = this.GetComponent<Rigidbody> ();
    }

    public override void UpdateObservation () {
      var update_time_difference = Time.time - this._last_update_time;
      if (this._differential && update_time_difference > 0) {
        var vel_diff = this._velocity - this._rigidbody.velocity;
        var ang_diff = this._angular_velocity - this._rigidbody.angularVelocity;
        if (vel_diff.magnitude > 0) {
          this._velocity = vel_diff / (update_time_difference + float.Epsilon);
        }
        if (ang_diff.magnitude > 0) {
          this._angular_velocity = ang_diff / (update_time_difference + float.Epsilon);
        }
      } else {
        this._velocity = this._rigidbody.velocity;
        this._angular_velocity = this._rigidbody.angularVelocity;
      }

      this._last_update_time = Time.time;
      /*var str_rep = "{";
      str_rep += "\"Velocity\": \"" + this._velocity;
      str_rep += "\", \"AngularVelocity\": \"" + this._angular_velocity;
      str_rep += "\"}";*/
      //Data = Encoding.ASCII.GetBytes (str_rep);
    }
  }
}
