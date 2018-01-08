using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  public class NearestByTagObserver : Observer,
                                      IHasEulerTransformProperties {
    [SerializeField] Vector3 _direction;

    [Header(
      header : "Specific",
      order = 102)]
    [SerializeField]
    GameObject _nearest_object;

    [Header(
      header : "Observation",
      order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Vector3 _rotation;

    [SerializeField] string _tag = "";

    public override string ObserverIdentifier { get { return this.name + "NearestByTag"; } }

    public Vector3 Position { get { return this._position; } }

    public Vector3 Rotation { get { return this._rotation; } }

    public Vector3 Direction { get { return this._direction; } }

    public override void UpdateData() {
      this.FindNearest();
      if (this.ParentEnvironment) {
        this._position =
          this.ParentEnvironment.TransformPosition(position : this._nearest_object.transform.position);
        this._direction =
          this.ParentEnvironment.TransformDirection(direction : this._nearest_object.transform.forward);
        this._rotation =
          this.ParentEnvironment.TransformDirection(direction : this._nearest_object.transform.up);
      } else {
        this._position = this._nearest_object.transform.position;
        this._direction = this._nearest_object.transform.forward;
        this._rotation = this._nearest_object.transform.up;
      }

      var str_rep = "{";
      if (this._nearest_object)
        str_rep += "\"NearestIdentifier\": \"" + this._nearest_object.name;
      else
        str_rep += "\"NearestIdentifier\": \"" + "None";
      str_rep += "\"}";
      //Data = Encoding.ASCII.GetBytes (str_rep);
    }

    void FindNearest() {
      var candidates = FindObjectsOfType<GameObject>();
      var nearest_distance = -1.0;
      foreach (var candidate in candidates)
        if (candidate.tag == this._tag) {
          var dist = Vector3.Distance(
                                      a : this.transform.position,
                                      b : candidate.transform.position);
          if (nearest_distance > dist || nearest_distance < 0) {
            nearest_distance = dist;
            this._nearest_object = candidate;
          }
        }
    }
  }
}
