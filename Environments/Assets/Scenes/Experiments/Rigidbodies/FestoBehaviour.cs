using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestoBehaviour : MonoBehaviour {

  Rigidbody[] _children;
  public float _torque_scalar = 0;
  public bool _find_global_rigidbodies = false;


  void Awake () {
    if (_find_global_rigidbodies)
      _children = FindObjectsOfType<Rigidbody> ();
    else
      _children = GetComponentsInChildren<Rigidbody> ();
  }

  void Update () {
    if (Input.GetKeyDown (KeyCode.UpArrow)) {
      _torque_scalar += 100; 
    } else if (Input.GetKeyDown (KeyCode.DownArrow)) {
      _torque_scalar -= 100; 
    }
  }

  void FixedUpdate () {
    foreach (var body in _children) {
      if (body.gameObject != this)
        body.AddRelativeTorque (Vector3.forward * _torque_scalar);
    }
  }
}
