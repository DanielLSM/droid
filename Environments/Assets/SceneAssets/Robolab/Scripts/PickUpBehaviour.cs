using UnityEngine;


namespace Robolab {
  public class PickUpBehaviour : MonoBehaviour {
    public float _max_pick_up_distance = 10;
    // Maximum distance from the camera at which the object can be picked up
    public float _throwing_strength = 10;
    public float _follow_strength = 10f;
    public float _holding_distance = 3;

    public GameObject _player;
    public Camera _camera;
    public Rigidbody LeftArm;
    public Rigidbody RightArm;
    //readonly VectorPid angularVelocityController = new VectorPid (30.7766f, 0, 0.2553191f);
    //readonly VectorPid headingController = new VectorPid (2.244681f, 0, 0.1382979f);

    public Vector3 _holding_position;

    GameObject _picked_up_object;
    Rigidbody _body;
    float _original_body_angular_drag;

    RaycastHit? _raycast;

    void Start () {
      _player = this.gameObject;
      if (!_camera) {
        _camera = this.GetComponent<Camera> ();
      }
    }

    void Update () {
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

    void FixedUpdate () {
      if (_picked_up_object) {
        UpdateHoldableObject ();
        UpdateArm (LeftArm, _picked_up_object);
        UpdateArm (RightArm, _picked_up_object);
      }
    }

    void Raycast () {
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

    void UpdateArm (Rigidbody arm, GameObject target) {
      //var angularVelocityError = arm.angularVelocity * -1;
      //Debug.DrawRay (transform.position, LeftArm.angularVelocity * 10, Color.black);

      //var angularVelocityCorrection = angularVelocityController.Update (angularVelocityError, Time.deltaTime);
      //Debug.DrawRay (transform.position, angularVelocityCorrection, Color.green);

      //arm.AddTorque (angularVelocityCorrection);

      //var desiredHeading = target.transform.position - transform.position;
      //Debug.DrawRay (transform.position, desiredHeading, Color.magenta);

      //var currentHeading = -transform.up;
      //Debug.DrawRay (transform.position, currentHeading * 15, Color.blue);

      //var headingError = Vector3.Cross (currentHeading, desiredHeading);
      //var headingCorrection = headingController.Update (headingError, Time.deltaTime);

      //arm.AddTorque (headingCorrection);
      arm.transform.LookAt (target.transform);
    }

    void UpdateHoldableObject () {
      //_body.velocity = Vector3.Lerp (_picked_up_object.transform.position, _pivot_position, _follow_strength); 
      //_body.velocity = (_holding_position - _picked_up_object.transform.position) * _follow_strength;// + ((1 - _follow_strength) * _body.velocity);
      //_body.velocity = Vector3.SmoothDamp (_picked_up_object.transform.position, _pivot_position, _pivot_position- _picked_up_object.transform.position, _follow_strength);
      //_body.velocity = Vector3.Lerp (_body.velocity, _pivot_position - _picked_up_object.transform.position, .9f);
      var distance = Vector3.Distance (_holding_position, _picked_up_object.transform.position);
      var direction = (_holding_position - _picked_up_object.transform.position).normalized;
      _body.MovePosition (_picked_up_object.transform.position + direction * distance * _follow_strength * Time.deltaTime);

    }

    void TryPickUpObject () {
      _body = _raycast.Value.rigidbody;
      _body.transform.position = _holding_position;
      _body.useGravity = false;

      _original_body_angular_drag = _body.angularDrag;
      _body.angularDrag = 1;

      _picked_up_object = _body.gameObject;
      Physics.IgnoreCollision (_picked_up_object.GetComponent<Collider> (), this.GetComponent<Collider> (), true);
    }

    void ReleaseObject (System.Action onRelease = null) {
      Physics.IgnoreCollision (_picked_up_object.GetComponent<Collider> (), this.GetComponent<Collider> (), false);
      _body.isKinematic = false;
      _body.useGravity = true;
      _body.angularDrag = _original_body_angular_drag;
      if (onRelease != null)
        onRelease ();
      ClearPickedUp ();
    }

    void ClearPickedUp () {
      _body = null;
      _picked_up_object = null;
    }

    void FreezeObject () {
      _body.isKinematic = true;
      ClearPickedUp ();
    }

    void ThrowObject () {
      ReleaseObject (() => _body.AddForce (_camera.transform.forward * _throwing_strength, ForceMode.Impulse));
    }
  }

  public class VectorPid {
    public float pFactor, iFactor, dFactor;

    Vector3 integral;
    Vector3 lastError;

    public VectorPid (float pFactor, float iFactor, float dFactor) {
      this.pFactor = pFactor;
      this.iFactor = iFactor;
      this.dFactor = dFactor;
    }

    public Vector3 Update (Vector3 currentError, float timeFrame) {
      integral += currentError * timeFrame;
      var deriv = (currentError - lastError) / timeFrame;
      lastError = currentError;
      return currentError * pFactor
      + integral * iFactor
      + deriv * dFactor;
    }
  }
}