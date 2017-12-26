using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neodroid.Utilities.NeodroidCamera {
  [RequireComponent (typeof(Camera))]
  [ExecuteInEditMode]
  [System.Serializable]
  public class SynchroniseCameraProperties : MonoBehaviour {

    public bool _sync_orthographic_size = true;
    public bool _sync_near_clip_plane = true;
    public bool _sync_far_clip_plane = true;
    public bool _sync_culling_mask = true;

    float _old_near_clip_plane;
    float _old_far_clip_plane;
    float _old_orthographic_size;
    int _old_culling_mask;

    Camera _camera;
    Camera[] _cameras;

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
        if (_old_orthographic_size != _camera.orthographicSize) {
          if (_sync_culling_mask) {
            _old_orthographic_size = _camera.orthographicSize;
            foreach (Camera camera in _cameras) {
              if (camera != _camera) {
                camera.orthographicSize = _camera.orthographicSize;
              }
            }
          }
        }
        if (_old_near_clip_plane != _camera.nearClipPlane) {
          if (_sync_culling_mask) {
            _old_near_clip_plane = _camera.nearClipPlane;
            foreach (Camera camera in _cameras) {
              if (camera != _camera) {
                camera.nearClipPlane = _camera.nearClipPlane;
              }
            }
          }
        }
        if (_old_far_clip_plane != _camera.farClipPlane) {
          if (_sync_culling_mask) {
            _old_far_clip_plane = _camera.farClipPlane;
            foreach (Camera camera in _cameras) {
              if (camera != _camera) {
                camera.farClipPlane = _camera.farClipPlane;
              }
            }
          }
        }
        if (_old_culling_mask != _camera.cullingMask) {
          if (_sync_culling_mask) {
            _old_culling_mask = _camera.cullingMask;
            foreach (Camera camera in _cameras) {
              if (camera != _camera) {
                camera.cullingMask = _camera.cullingMask;
              }
            }
          }
        }

      } else {
        print ("No camera component found on gameobject");
      }

    }
  }
}