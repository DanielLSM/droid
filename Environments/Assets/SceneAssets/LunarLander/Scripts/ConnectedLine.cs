using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class ConnectedLine : MonoBehaviour {
  public Transform _connection_to;
  private LineRenderer _line_renderer;

  public Vector3 _offset = Vector3.up;

  // Use this for initialization
  private void Start() {
    _line_renderer = GetComponent<LineRenderer>();
    if (!_connection_to) _connection_to = GetComponentInParent<Transform>();
  }

  // Update is called once per frame
  private void Update() {
    if (_connection_to)
      _line_renderer.SetPosition(
                                 1,
                                 transform.InverseTransformPoint(
                                                                 _connection_to.TransformPoint(
                                                                                               _connection_to
                                                                                                 .localPosition
                                                                                               + _offset)));
  }
}
