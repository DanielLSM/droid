using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Observers
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraObserver : Observer
    {

        Camera _camera;

        protected override void Start()
        {
            AddToEnvironment();
            _camera = this.GetComponent<Camera>();
        }

        public override byte[] GetData()
        {
            _data = NeodroidUtilities.RenderTextureImage(_camera).EncodeToPNG();
            return _data;
        }

        public override string GetObserverIdentifier()
        {
            return name + "Camera";
        }
    }
}
