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

    public override void UpdateData () {
      _data = NeodroidUtilities.RenderTextureImage (_camera).EncodeToPNG ();
    }

    public override string GetObserverIdentifier () {
      return name + "Camera";
    }
  }
}
