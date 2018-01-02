using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class QuaternionTransformObserver : Observer {
    [Header ("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Quaternion _rotation;

    public bool _use_environments_coordinates = true;

    public override void UpdateData () {
      if (ParentEnvironment && _use_environments_coordinates) {
        _position = ParentEnvironment.TransformPosition (this.transform.position);
        _rotation = Quaternion.Euler (ParentEnvironment.TransformDirection (this.transform.forward));
      } else {
        _position = this.transform.position;
        _rotation = this.transform.rotation;
      }
    }

    public ObservationSpace Space {
      get {
        return _space;
      }
    }

    public Vector3 Position {
      get {
        return _position;
      }
    }

    public Quaternion Rotation {
      get {
        return _rotation;
      }
    }

    public override string ObserverIdentifier { get { return name + "QuaternionTransform"; } }
  }
}
