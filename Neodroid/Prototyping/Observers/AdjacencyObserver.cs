﻿using System;
using Neodroid.Prototyping.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.Structs;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  public class AdjacencyObserver : Observer,
                                    IHasArray {
    [SerializeField] RaycastHit _hit;

    [Header ("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] bool _is_2d = false;

    [SerializeField] float _range = 1.0f;

    public override string ObserverIdentifier { get { return this.name + "Adjencency"; } }

    public Single[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    public RaycastHit Hit { get { return this._hit; } set { this._hit = value; } }

    public override void UpdateObservation () {
      if (this._is_2d) {
        var vals = new float[8];
        if (Physics.Raycast (this.transform.position, Vector3.forward, out this._hit, this._range))
          vals [0] = 1;
        else
          vals [0] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.left, out this._hit, this._range))
          vals [1] = 1;
        else
          vals [1] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.right, out this._hit, this._range))
          vals [2] = 1;
        else
          vals [2] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.back, out this._hit, this._range))
          vals [3] = 1;
        else
          vals [3] = 0;

        if (Physics.Raycast (this.transform.position, (Vector3.forward + Vector3.left).normalized, out this._hit, this._range))
          vals [4] = 1;
        else
          vals [4] = 0;

        if (Physics.Raycast (this.transform.position, (Vector3.forward + Vector3.right).normalized, out this._hit, this._range))
          vals [5] = 1;
        else
          vals [5] = 0;

        if (Physics.Raycast (this.transform.position, (Vector3.back + Vector3.left).normalized, out this._hit, this._range))
          vals [6] = 1;
        else
          vals [6] = 0;

        if (Physics.Raycast (this.transform.position, (Vector3.back + Vector3.right).normalized, out this._hit, this._range))
          vals [7] = 1;
        else
          vals [7] = 0;
        this.ObservationArray = vals;
      } else {
        var vals = new float[27];
        if (Physics.Raycast (this.transform.position, Vector3.forward, out this._hit, this._range))
          vals [0] = 1;
        else
          vals [0] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.left, out this._hit, this._range))
          vals [1] = 1;
        else
          vals [1] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.right, out this._hit, this._range))
          vals [2] = 1;
        else
          vals [2] = 0;

        if (Physics.Raycast (this.transform.position, Vector3.back, out this._hit, this._range))
          vals [3] = 1;
        else
          vals [3] = 0;
        if (Physics.Raycast (this.transform.position, Vector3.up, out this._hit, this._range))
          vals [4] = 1;
        else
          vals [4] = 0;
        if (Physics.Raycast (this.transform.position, Vector3.down, out this._hit, this._range))
          vals [5] = 1;
        else
          vals [5] = 0;

        //Missing combinations

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
      Debug.DrawLine(
      this.transform.position,
    this.transform.position - (Vector3.forward + Vector3.left).normalized * this._range,
      this._color);
      Debug.DrawLine(
      this.transform.position,
    this.transform.position - (Vector3.forward + Vector3.right).normalized * this._range,
      this._color);
      Debug.DrawLine(
      this.transform.position,
    this.transform.position - (Vector3.back + Vector3.left).normalized * this._range,
      this._color);
      Debug.DrawLine(
      this.transform.position,
    this.transform.position - (Vector3.back + Vector3.right).normalized * this._range,
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

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.left).normalized * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.right).normalized * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.forward).normalized * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.back).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.left).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.right).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.forward).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.back).normalized * this._range,
            this._color);


        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down + Vector3.forward + Vector3.left).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.forward + Vector3.right).normalized * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down + Vector3.back + Vector3.left).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.down+ Vector3.back + Vector3.right).normalized * this._range,
            this._color);


        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up + Vector3.forward + Vector3.left).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.forward + Vector3.right).normalized * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up + Vector3.back + Vector3.left).normalized * this._range,
            this._color);

        Debug.DrawLine(
            this.transform.position,
            this.transform.position - (Vector3.up+ Vector3.back + Vector3.right).normalized * this._range,
            this._color);


      }
    }
    #endif
  }
}
