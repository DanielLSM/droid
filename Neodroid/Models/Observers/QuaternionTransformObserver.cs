using System;
using Neodroid.Models.Observers.General;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  [Serializable]
  public class QuaternionTransformObserver : Observer {
    [Header(
      header : "Observation",
      order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [Header(
      header : "Specfic",
      order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;

    public bool _use_environments_coordinates = true;

    public ObservationSpace Space { get { return this._space; } }

    public Vector3 Position { get { return this._position; } }

    public Quaternion Rotation { get { return this._rotation; } }

    public override string ObserverIdentifier { get { return this.name + "QuaternionTransform"; } }

    public override void UpdateData() {
      if (this.ParentEnvironment && this._use_environments_coordinates) {
        this._position = this.ParentEnvironment.TransformPosition(position : this.transform.position);
        this._rotation = Quaternion.Euler(
                                          euler : this.ParentEnvironment.TransformDirection(
                                                                                            direction : this
                                                                                                          .transform
                                                                                                          .forward));
      } else {
        this._position = this.transform.position;
        this._rotation = this.transform.rotation;
      }
    }
  }
}
