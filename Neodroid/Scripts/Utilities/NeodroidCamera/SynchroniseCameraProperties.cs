
using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  [RequireComponent (typeof(Camera))]
  [ExecuteInEditMode]
  [System.Serializable]
  public class SynchroniseCameraProperties : MonoBehaviour {
    private Camera _camera;
    private Camera[] _cameras;
    private int _old_culling_mask;
    private float _old_far_clip_plane;

    private float _old_near_clip_plane;
    private float _old_orthographic_size;
    public bool _sync_culling_mask = true;
    public bool _sync_far_clip_plane = true;
    public bool _sync_near_clip_plane = true;

    public bool _sync_orthographic_size = true;

    double TOLERANCE = System.Double.Epsilon;

    public void Start () {
      _camera = GetComponent<Camera> ();
      if (_camera) {
        _old_orthographic_size = _camera.orthographicSize;
        _old_near_clip_plane = _camera.nearClipPlane;
        _old_far_clip_plane = _camera.farClipPlane;
        _old_culling_mask = _camera.cullingMask;

        _cameras = FindObjectsOfType<Camera> ();
      } else {
        print ("No camera component found on gameobject");
      }
    }

    public void Update () {
      if (_camera) {
        if (System.Math.Abs (_old_orthographic_size - _camera.orthographicSize) > TOLERANCE)
        if (_sync_culling_mask) {
          _old_orthographic_size = _camera.orthographicSize;
          foreach (var cam in _cameras)
            if (cam != _camera)
              cam.orthographicSize = _camera.orthographicSize;
        }

        if (System.Math.Abs (_old_near_clip_plane - _camera.nearClipPlane) > TOLERANCE)
        if (_sync_culling_mask) {
          _old_near_clip_plane = _camera.nearClipPlane;
          foreach (var cam in _cameras)
            if (cam != _camera)
              cam.nearClipPlane = _camera.nearClipPlane;
        }

        if (System.Math.Abs (_old_far_clip_plane - _camera.farClipPlane) > TOLERANCE)
        if (_sync_culling_mask) {
          _old_far_clip_plane = _camera.farClipPlane;
          foreach (var cam in _cameras)
            if (cam != _camera)
              cam.farClipPlane = _camera.farClipPlane;
        }

        if (_old_culling_mask != _camera.cullingMask)
        if (_sync_culling_mask) {
          _old_culling_mask = _camera.cullingMask;
          foreach (var cam in _cameras)
            if (cam != _camera)
              cam.cullingMask = _camera.cullingMask;
        }
      } else {
        print ("No camera component found on gameobject");
      }
    }
  }
}
