using System;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  public class RaycastObserver : Observer,
                                 IHasSingle {
    [SerializeField] Vector3 _direction = Vector3.forward;

    [SerializeField] RaycastHit _hit;

    [SerializeField]
    ValueSpace _observation_space =
        new ValueSpace {DecimalGranularity = 3, MinValue = 0, MaxValue = 100.0f};

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] float _range = 100.0f;

    public override string ObserverIdentifier {
      get {
        return this.name
               + "Raycast"
               + String.Format("{0}{1}{2}", this._direction.x, this._direction.y, this._direction.z);
      }
    }

    public Single ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = this._observation_space.ClipNormaliseRound(value); }
    }

    public override void UpdateObservation() {
      if (Physics.Raycast(this.transform.position, this._direction, out this._hit, this._range))
        this.ObservationValue = this._hit.distance;
      else
        this.ObservationValue = this._range;

      this.FloatEnumerable = new[] {this.ObservationValue};
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;
    void OnDrawGizmosSelected() {
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - this._direction * this._range,
          this._color);
    }
    #endif
  }
}
