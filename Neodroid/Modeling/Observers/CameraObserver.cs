using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(Camera))]
  public class CameraObserver : Observer {

    Camera _camera;

    protected override void Start () {
      _camera = this.GetComponent<Camera> ();
    }

    protected virtual void Update () {
      Data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG ();
    }

    public override void UpdateData () {
      //_data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG ();
    }

    public override string ObserverIdentifier { get { return name + "Camera"; } }
  }
}
