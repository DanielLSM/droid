using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class CameraObserver : Observer {
    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera;

    [Header("Observation", order = 103)]
    [SerializeField]
    byte[] _data = { };

    public byte[] Data { get { return this._data; } set { this._data = value; } }

    public override string ObserverIdentifier { get { return this.name + "Camera"; } }

    protected override void Start() { this._camera = this.GetComponent<Camera>(); }

    protected virtual void Update() {
      this.Data = NeodroidUtilities.RenderTextureImage(this._camera).EncodeToPNG();
    }

    public override void UpdateData() {
      //Data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG (); // Must be done on the main thread
    }
  }
}
