using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(Camera))]
  public class CameraObserver : Observer {
    [Header ("Specific", order = 102)]
    [SerializeField]
    Camera _camera;
    [Header ("Observation", order = 103)]
    [SerializeField]
    byte[] _data = new byte[] { };

    protected override void Start () {
      _camera = this.GetComponent<Camera> ();
    }

    protected virtual void Update () {
      Data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG ();
    }

    public byte[] Data {
      get {
        return _data;
      }
      set {
        _data = value;
      }
    }

    public override void UpdateData () {
      //Data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG (); // Must be done on the main thread
    }

    public override string ObserverIdentifier { get { return name + "Camera"; } }
  }
}
