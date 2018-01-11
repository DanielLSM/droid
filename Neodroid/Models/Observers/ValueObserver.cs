using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  public class ValueObserver : Observer,
                               IHasObservationValue {

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    public float ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }

    public override string ObserverIdentifier { get { return this.name + "Value"; } }

    public override void UpdateObservation() { this.ObservationValue = 0; }
  }
}
