using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers {

  public enum ObservationSpace {
    Local,
    Global,
    Environment
  }

  [ExecuteInEditMode]
  [System.Serializable]
  public class EulerTransformObserver : Observer, HasEulerTransformProperties {
    [Header ("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;

    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Vector3 _rotation;
    [SerializeField]
    Vector3 _direction;

    public override void UpdateData () {
      if (ParentEnvironment && _space == ObservationSpace.Environment) {
        _position = ParentEnvironment.TransformPosition (this.transform.position);
        _direction = ParentEnvironment.TransformDirection (this.transform.forward);
        _rotation = ParentEnvironment.TransformDirection (this.transform.up);
      } else if (_space == ObservationSpace.Local) {
        _position = this.transform.localPosition;
        _direction = this.transform.forward;
        _rotation = this.transform.up;
      } else {
        _position = this.transform.position;
        _direction = this.transform.forward;
        _rotation = this.transform.up;
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

    public override string ObserverIdentifier{ get { return name + "EulerTransform"; } }
      
  }
}

