using System;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  public enum ObservationSpace {
    Local,
    Global,
    Environment
  }

  [ExecuteInEditMode]
  [Serializable]
  public class EulerTransformObserver : Observer,
                                        IHasEulerTransform {
    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = new Space3(int.MaxValue);

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(int.MaxValue);
    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = new Space3(int.MaxValue);

    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;

    public ObservationSpace Space { get { return this._space; } }

    public override string ObserverIdentifier { get { return this.name + "EulerTransform"; } }

    public Vector3 Position {
      get { return this._position; }
      set { this._position = this._position_space.ClipNormalise(value); }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set { this._rotation = this._rotation_space.ClipNormalise(value); }
    }

    public Vector3 Direction {
      get { return this._direction; }
      set { this._direction = this._direction_space.ClipNormalise(value); }
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment) {
        this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
      } else if (this._space == ObservationSpace.Local) {
        this.Position = this.transform.localPosition;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      } else {
        this.Position = this.transform.position;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      }

      this.FloatEnumerable = new[] {
          this.Position.x,
          this.Position.y,
          this.Position.z,
          this.Direction.x,
          this.Direction.y,
          this.Direction.z,
          this.Rotation.x,
          this.Rotation.y,
          this.Rotation.z
      };
    }
  }
}
