using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Traction : MonoBehaviour {


  public Rigidbody[] _tentacles;
  public float _low_limit = 2;

  void FixedUpdate () {
    if (Input.GetKeyDown (KeyCode.LeftArrow)) {
      foreach (var tentacle in _tentacles) {
        //tentacle.AddRelativeForce (Vector3.left);
        if (System.Math.Abs (tentacle.transform.localPosition.x) > 2) {
          tentacle.transform.localPosition = (tentacle.transform.localPosition - tentacle.transform.right);
        }
      }
    } else if (Input.GetKeyDown (KeyCode.RightArrow)) {
      foreach (var tentacle in _tentacles) {
        //tentacle.AddRelativeForce (Vector3.right);
        tentacle.transform.localPosition = (tentacle.transform.localPosition + tentacle.transform.right);
      }
    }
  }
}
