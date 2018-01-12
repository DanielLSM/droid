using System;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  public enum ObservationSpace {
    Local,
    Global,
    Environment
  }

  [ExecuteInEditMode]
  [Serializable]
  public class EulerTransformObserver : Observer,
                                        IHasEulerTransform {


    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;
    [SerializeField] Vector3 _direction;
    [SerializeField] Vector3 _rotation;

    [Header ("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;

    public ObservationSpace Space { get { return this._space; } }

    public override string ObserverIdentifier { get { return this.name + "EulerTransform"; } }

    public Vector3 Position { get { return this._position; } }

    public Vector3 Rotation { get { return this._rotation; } }

    public Vector3 Direction { get { return this._direction; } }

    public override void UpdateObservation () {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment) {
        this._position = this.ParentEnvironment.TransformPosition (this.transform.position);
        this._direction = this.ParentEnvironment.TransformDirection (this.transform.forward);
        this._rotation = this.ParentEnvironment.TransformDirection (this.transform.up);
      } else if (this._space == ObservationSpace.Local) {
        this._position = this.transform.localPosition;
        this._direction = this.transform.forward;
        this._rotation = this.transform.up;
      } else {
        this._position = this.transform.position;
        this._direction = this.transform.forward;
        this._rotation = this.transform.up;
      }
    }
  }
}
