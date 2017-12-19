using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class QuaternionTransformObserver : Observer {

    public Vector3 _position;
    public Quaternion _rotation;

    public bool _use_environments_coordinates = true;

    public override void UpdateData () {
      if (_environment && _use_environments_coordinates) {
        _position = _environment.TransformPosition (this.transform.position);
        _rotation = Quaternion.Euler (_environment.TransformDirection (this.transform.forward));
      } else {
        _position = this.transform.position;
        _rotation = this.transform.rotation;
      }
    }

    public override string GetObserverIdentifier () {
      return name + "QuaternionTransform";
    }
  }
}
