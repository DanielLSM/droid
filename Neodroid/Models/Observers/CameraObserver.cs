using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class CameraObserver : Observer {
    [Header(
      "Specific",
      order = 102)]
    [SerializeField]
    private Camera _camera;

    [Header(
      "Observation",
      order = 103)]
    [SerializeField]
    private byte[] _data = { };

    public byte[] Data { get { return _data; } set { _data = value; } }

    public override string ObserverIdentifier { get { return name + "Camera"; } }

    protected override void Start() { _camera = GetComponent<Camera>(); }

    protected virtual void Update() { Data = NeodroidUtilities.RenderTextureImage(_camera).EncodeToPNG(); }

    public override void UpdateData() {
      //Data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG (); // Must be done on the main thread
    }
  }
}
