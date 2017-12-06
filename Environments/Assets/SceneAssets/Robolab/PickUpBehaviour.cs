using UnityEngine;
using System;

namespace Robolab {
  public class PickUpBehaviour : MonoBehaviour {
    public float _max_pick_up_distance = 10;
    // Maximum distance from the camera at which the object can be picked up
    public float _throwing_strength = 10;
    public float _follow_strength = 10f;
    public float _holding_distance = 3;

    public GameObject _player;
    public Camera _camera;
    public Vector3 _holding_position;

    private GameObject _picked_up_object;
    private Rigidbody _body;
    private float _original_body_angular_drag;

    private RaycastHit? _raycast;

    private void Start () {
      _player = this.gameObject;
      if (!_camera) {
        _camera = this.GetComponent<Camera> ();
      }
    }

    private void Update () {
      Raycast ();
      if (_raycast.HasValue) {
        if (_raycast.Value.distance < _holding_distance) {
          _holding_position = _raycast.Value.point;
        } else {
          _holding_position = _camera.transform.position + _camera.transform.forward * _holding_distance;
        }
      } else {
        _holding_position = _camera.transform.position + _camera.transform.forward * _holding_distance;
      }
      var scroll_delta = Input.GetAxis ("Mouse ScrollWheel");
      if (scroll_delta * scroll_delta > 0f) {
        _holding_distance += scroll_delta;
      }
      if (Input.GetKeyDown (KeyCode.E)) {
        if (!_picked_up_object) {
          if (_raycast.HasValue) {
            TryPickUpObject ();
          }
        } else {
          ReleaseObject ();
        }
      }
      if (_picked_up_object && Input.GetMouseButtonDown (0))
        ThrowObject ();
      if (_picked_up_object && Input.GetMouseButtonDown (1))
        FreezeObject ();
    }

    private void FixedUpdate () {
      if (_picked_up_object) {
        UpdateHoldableObject ();
      }
    }

    private void Raycast () {
      _raycast = null;
      //const int layerMask = 1 << 8;
      //Debug.DrawLine (_camera.transform.position, _camera.transform.forward * _max_pick_up_distance);
      var raycastHits = Physics.RaycastAll (_camera.transform.position, _camera.transform.forward, _max_pick_up_distance);//, ~layerMask);
      foreach (var hit in raycastHits) {
        if (_picked_up_object) {
          if (hit.collider == _picked_up_object.GetComponent<Collider> ()) {
            continue;
          }
        }
        if (hit.collider == _player.GetComponent<Collider> () || !hit.collider.GetComponent<Rigidbody> ()) { // avoid colliding with the player object itself
          continue;
        }
        _raycast = hit;
      }
    }

    private void UpdateHoldableObject () {
      //_body.velocity = Vector3.Lerp (_picked_up_object.transform.position, _pivot_position, _follow_strength); 
      //_body.velocity = (_holding_position - _picked_up_object.transform.position) * _follow_strength;// + ((1 - _follow_strength) * _body.velocity);
      //_body.velocity = Vector3.SmoothDamp (_picked_up_object.transform.position, _pivot_position, _pivot_position- _picked_up_object.transform.position, _follow_strength);
      //_body.velocity = Vector3.Lerp (_body.velocity, _pivot_position - _picked_up_object.transform.position, .9f);
      var distance = Vector3.Distance (_holding_position, _picked_up_object.transform.position);
      var direction = (_holding_position - _picked_up_object.transform.position).normalized;
      _body.MovePosition (_picked_up_object.transform.position + direction * distance * _follow_strength * Time.deltaTime);

    }

    private void TryPickUpObject () {
      _body = _raycast.Value.rigidbody;
      _body.transform.position = _holding_position;
      _body.useGravity = false;

      _original_body_angular_drag = _body.angularDrag;
      _body.angularDrag = 1;

      _picked_up_object = _body.gameObject;
      Physics.IgnoreCollision (_picked_up_object.GetComponent<Collider> (), this.GetComponent<Collider> (), true);
    }

    private void ReleaseObject (Action onRelease = null) {
      Physics.IgnoreCollision (_picked_up_object.GetComponent<Collider> (), this.GetComponent<Collider> (), false);
      _body.isKinematic = false;
      _body.useGravity = true;
      _body.angularDrag = _original_body_angular_drag;
      if (onRelease != null)
        onRelease ();
      ClearPickedUp ();
    }

    private void ClearPickedUp () {
      _body = null;
      _picked_up_object = null;
    }

    private void FreezeObject () {
      _body.isKinematic = true;
      ClearPickedUp ();
    }

    private void ThrowObject () {
      ReleaseObject (() => _body.AddForce (_camera.transform.forward * _throwing_strength, ForceMode.Impulse));
    }
  }
}