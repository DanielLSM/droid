using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(LineRenderer))]
public class ConnectedLine : MonoBehaviour {

  public Vector3 _offset = Vector3.up;
  public Transform _connection_to;
  LineRenderer _line_renderer;


  // Use this for initialization
  void Start () {
    _line_renderer = GetComponent<LineRenderer> ();
    if (!_connection_to) {
      _connection_to = GetComponentInParent<Transform> ();
    }
  }
	
  // Update is called once per frame
  void Update () {
    if (_connection_to) {
      _line_renderer.SetPosition (1, this.transform.InverseTransformPoint (_connection_to.TransformPoint (_connection_to.localPosition + _offset)));
    }
  }
}
