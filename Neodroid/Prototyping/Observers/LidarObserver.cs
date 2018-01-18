using System;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  public class LidarObserver : Observer,
                                    IHasArray {
    [SerializeField] RaycastHit _hit;

    [Header ("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] bool _is_2d = false;

    [SerializeField]
    ValueSpace _observation_value_space =
      new ValueSpace { DecimalGranularity = int.MaxValue, MinValue = 0.0f, MaxValue = 100.0f };

    [SerializeField] float _range = 100.0f;

    public override string ObserverIdentifier { get { return this.name + "Lidar"; } }

    public Single[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    public override void UpdateObservation () {
      if (this._is_2d) {
        var vals = new float[4];
        if (Physics.Raycast (this.transform.position, Vector3.forward, out this._hit, this._range))
          vals [0] = this._hit.distance;
        else
          vals [0] = this._range;
        vals [0] = this._observation_value_space.ClipNormaliseRound (vals [0]);
        if (Physics.Raycast (this.transform.position, Vector3.left, out this._hit, this._range))
          vals [1] = this._hit.distance;
        else
          vals [1] = this._range;
        vals [1] = this._observation_value_space.ClipNormaliseRound (vals [1]);
        if (Physics.Raycast (this.transform.position, Vector3.right, out this._hit, this._range))
          vals [2] = this._hit.distance;
        else
          vals [2] = this._range;
        vals [2] = this._observation_value_space.ClipNormaliseRound (vals [2]);
        if (Physics.Raycast (this.transform.position, Vector3.back, out this._hit, this._range))
          vals [3] = this._hit.distance;
        else
          vals [3] = this._range;
        vals [3] = this._observation_value_space.ClipNormaliseRound (vals [3]);
        this.ObservationArray = vals;
      } else {
        var vals = new float[6];
        if (Physics.Raycast (this.transform.position, Vector3.forward, out this._hit, this._range))
          vals [0] = this._hit.distance;
        else
          vals [0] = this._range;
        vals [0] = this._observation_value_space.ClipNormaliseRound (vals [0]);
        if (Physics.Raycast (this.transform.position, Vector3.left, out this._hit, this._range))
          vals [1] = this._hit.distance;
        else
          vals [1] = this._range;
        vals [1] = this._observation_value_space.ClipNormaliseRound (vals [1]);
        if (Physics.Raycast (this.transform.position, Vector3.right, out this._hit, this._range))
          vals [2] = this._hit.distance;
        else
          vals [2] = this._range;
        vals [2] = this._observation_value_space.ClipNormaliseRound (vals [2]);
        if (Physics.Raycast (this.transform.position, Vector3.back, out this._hit, this._range))
          vals [3] = this._hit.distance;
        else
          vals [3] = this._range;
        vals [3] = this._observation_value_space.ClipNormaliseRound (vals [3]);
        if (Physics.Raycast (this.transform.position, Vector3.up, out this._hit, this._range))
          vals [4] = this._hit.distance;
        else
          vals [4] = this._range;
        vals [4] = this._observation_value_space.ClipNormaliseRound (vals [4]);
        if (Physics.Raycast (this.transform.position, Vector3.down, out this._hit, this._range))
          vals [5] = this._hit.distance;
        else
          vals [5] = this._range;
        vals [5] = this._observation_value_space.ClipNormaliseRound (vals [5]);
        this.ObservationArray = vals;
      }

      this.FloatEnumerable = this.ObservationArray;
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.forward * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.left * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.right * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.back * this._range,
          this._color);
      if (!this._is_2d) {
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - Vector3.up * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - Vector3.down * this._range,
            this._color);
      }
    }
    #endif
  }
}
