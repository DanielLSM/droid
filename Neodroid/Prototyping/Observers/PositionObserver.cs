using System;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [ExecuteInEditMode]
  [Serializable]
	public class PositionObserver : Observer,
	                                IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);

    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment;

    public ObservationSpace Space { get { return this._space; } }

    public override string ObserverIdentifier { get { return this.name + "Position"; } }

    public Vector3 ObservationValue {
      get { return this._position; }
      set { this._position = this._position_space.ClipNormaliseRound(value); }
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment) {
        this.ObservationValue = this.ParentEnvironment.TransformPosition(this.transform.position);
      } else if (this._space == ObservationSpace.Local) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }

      this.FloatEnumerable = new[] {
          this.ObservationValue.x,
          this.ObservationValue.y,
          this.ObservationValue.z
      };
    }
  }
}

