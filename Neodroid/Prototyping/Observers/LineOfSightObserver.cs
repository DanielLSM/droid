using System;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [ExecuteInEditMode]
  [Serializable]
  public class LineOfSightObserver :Observer,
	                                  IHasSingle {
    [Header ("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    RaycastHit _hit;

    [SerializeField]
    float _obs_value;

    public Single ObservationValue { get { return this._obs_value; } private set { this._obs_value = value; } }

    public override string ObserverIdentifier { get { return this.name + "LineOfSight"; } }

    public override void UpdateObservation () {
      var distance = Vector3.Distance (this.transform.position, this._target.position);
      if (Physics.Raycast (this.transform.position, this._target.position - this.transform.position, out this._hit, distance)) {
        if (this.Debugging)
          print (this._hit.distance);
        if (this._hit.collider.gameObject != this._target.gameObject)
          this.ObservationValue = 0;
        else {
          this.ObservationValue = 1;
        }
      } else {
        this.ObservationValue = 1;
      }

      this.FloatEnumerable = new[] { this.ObservationValue };
    }
  }
}
