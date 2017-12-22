﻿using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class EulerTransformObserver : Observer {

    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    public bool _use_environments_coordinates = true;

    public override void UpdateData () {
      if (_environment && _use_environments_coordinates) {
        _position = _environment.TransformPosition (this.transform.position);
        _direction = _environment.TransformDirection (this.transform.forward);
        _rotation = _environment.TransformDirection (this.transform.up);
      } else {
        _position = this.transform.position;
        _direction = this.transform.forward;
        _rotation = this.transform.up;
      }
    }

    public override string ObserverIdentifier{ get { return name + "EulerTransform"; } }
      
  }
}

