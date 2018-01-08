using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  [ExecuteInEditMode]
  [RequireComponent( typeof(LineRenderer))]
  public class ConnectedLine : MonoBehaviour {
    public Transform _connection_to;
    LineRenderer _line_renderer;

    public Vector3 _offset = Vector3.up;

    // Use this for initialization
    void Start() {
      this._line_renderer = this.GetComponent<LineRenderer>();
      if (!this._connection_to) this._connection_to = this.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update() {
      if (this._connection_to)
        this._line_renderer.SetPosition(
                                        index : 1,
                                        position : this.transform.InverseTransformPoint(
                                                                                        position : this
                                                                                                   ._connection_to
                                                                                                   .TransformPoint(
                                                                                                                   position
                                                                                                                   : this
                                                                                                                       ._connection_to
                                                                                                                       .localPosition
                                                                                                                     + this
                                                                                                                       ._offset)));
    }
  }
}
