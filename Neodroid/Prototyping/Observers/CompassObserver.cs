

using System;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [ExecuteInEditMode]
  [Serializable]

  public class CompassObserver : Observer,
IHasDouble {

    [Header ("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Vector2 _2d_position;


    [Header ("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    [SerializeField] Space3 _position_space = new Space3 {
        DecimalGranularity = 1,
        MaxValues = Vector3.one,
        MinValues = -Vector3.one
    };

    public override string ObserverIdentifier { get { return this.name + "Compass"; } }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.ClipNormaliseRound (value);
        this._2d_position = new Vector2 (this._position.x, this._position.z);
      }
    }

    public Vector2 ObservationValue {
      get { return this._2d_position; }
      set {
        this._position = this._position_space.ClipNormaliseRound (value);
        this._2d_position = new Vector2 (this._position.x, this._position.z);
      }
    }

    public override void UpdateObservation () {
      this.Position = this.transform.InverseTransformVector (this.transform.position - this._target.position).normalized;

      this.FloatEnumerable = new[] {
        this.Position.x,
        this.Position.z
      };
    }
  }
}

