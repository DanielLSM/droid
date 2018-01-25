using Neodroid.Prototyping.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  public class ValueObserver : Observer,
                               IHasSingle {
    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] ValueSpace _observation_value_space;

    public override string ObserverIdentifier { get { return this.name + "Value"; } }

    public float ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = this._observation_value_space.ClipNormaliseRound(value); }
    }

    public override void UpdateObservation() { this.FloatEnumerable = new[] {this.ObservationValue}; }
  }
}
