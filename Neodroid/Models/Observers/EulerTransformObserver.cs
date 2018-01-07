
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Observers {
  public enum ObservationSpace {
    Local,
    Global,
    Environment
  }

  [ExecuteInEditMode]
  [System.Serializable]
  public class EulerTransformObserver : Observer,
                                        HasEulerTransformProperties {
    [SerializeField]
    private Vector3 _direction;

    [Header(
      "Observation",
      order = 103)]
    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private Vector3 _rotation;

    [Header(
      "Specfic",
      order = 102)]
    [SerializeField]
    private ObservationSpace _space = ObservationSpace.Environment;

    public ObservationSpace Space { get { return _space; } }

    public override string ObserverIdentifier { get { return name + "EulerTransform"; } }

    public Vector3 Position { get { return _position; } }

    public Vector3 Rotation { get { return _rotation; } }

    public Vector3 Direction { get { return _direction; } }

    public override void UpdateData() {
      if (ParentEnvironment && _space == ObservationSpace.Environment) {
        _position = ParentEnvironment.TransformPosition(transform.position);
        _direction = ParentEnvironment.TransformDirection(transform.forward);
        _rotation = ParentEnvironment.TransformDirection(transform.up);
      } else if (_space == ObservationSpace.Local) {
        _position = transform.localPosition;
        _direction = transform.forward;
        _rotation = transform.up;
      } else {
        _position = transform.position;
        _direction = transform.forward;
        _rotation = transform.up;
      }
    }
  }
}
