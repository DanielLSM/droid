
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class QuaternionTransformObserver : Observer {
    [Header(
      "Observation",
      order = 103)]
    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private Quaternion _rotation;

    [Header(
      "Specfic",
      order = 102)]
    [SerializeField]
    private ObservationSpace _space = ObservationSpace.Environment;

    public bool _use_environments_coordinates = true;

    public ObservationSpace Space { get { return _space; } }

    public Vector3 Position { get { return _position; } }

    public Quaternion Rotation { get { return _rotation; } }

    public override string ObserverIdentifier { get { return name + "QuaternionTransform"; } }

    public override void UpdateData() {
      if (ParentEnvironment && _use_environments_coordinates) {
        _position = ParentEnvironment.TransformPosition(transform.position);
        _rotation = Quaternion.Euler(ParentEnvironment.TransformDirection(transform.forward));
      } else {
        _position = transform.position;
        _rotation = transform.rotation;
      }
    }
  }
}
