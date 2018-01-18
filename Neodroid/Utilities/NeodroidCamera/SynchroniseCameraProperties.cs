using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.NeodroidCamera {
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  [Serializable]
  public class SynchroniseCameraProperties : MonoBehaviour {
    readonly double TOLERANCE = Double.Epsilon;

    [SerializeField] Camera _camera;

    [SerializeField] Camera[] _cameras;

    [SerializeField] int _old_culling_mask;

    [SerializeField] float _old_far_clip_plane;

    [SerializeField] float _old_near_clip_plane;

    [SerializeField] float _old_orthographic_size;

    [SerializeField] bool _sync_culling_mask = true;
    [SerializeField] bool _sync_far_clip_plane = true;
    [SerializeField] bool _sync_near_clip_plane = true;

    [SerializeField] bool _sync_orthographic_size = true;

    public Boolean SyncOrthographicSize {
      get { return this._sync_orthographic_size; }
      set { this._sync_orthographic_size = value; }
    }

    public Boolean SyncNearClipPlane {
      get { return this._sync_near_clip_plane; }
      set { this._sync_near_clip_plane = value; }
    }

    public Boolean SyncFarClipPlane {
      get { return this._sync_far_clip_plane; }
      set { this._sync_far_clip_plane = value; }
    }

    public Boolean SyncCullingMask {
      get { return this._sync_culling_mask; }
      set { this._sync_culling_mask = value; }
    }

    public void Awake() {
      this._camera = this.GetComponent<Camera>();
      if (this._camera) {
        this._old_orthographic_size = this._camera.orthographicSize;
        this._old_near_clip_plane = this._camera.nearClipPlane;
        this._old_far_clip_plane = this._camera.farClipPlane;
        this._old_culling_mask = this._camera.cullingMask;

        this._cameras = FindObjectsOfType<Camera>();
      } else
        print("No camera component found on gameobject");
    }

    public void Update() {
      if (this._camera) {
        if (Math.Abs(this._old_orthographic_size - this._camera.orthographicSize) > this.TOLERANCE) {
          if (this._sync_culling_mask) {
            this._old_orthographic_size = this._camera.orthographicSize;
            foreach (var cam in this._cameras) {
              if (cam != this._camera)
                cam.orthographicSize = this._camera.orthographicSize;
            }
          }
        }

        if (Math.Abs(this._old_near_clip_plane - this._camera.nearClipPlane) > this.TOLERANCE) {
          if (this._sync_culling_mask) {
            this._old_near_clip_plane = this._camera.nearClipPlane;
            foreach (var cam in this._cameras) {
              if (cam != this._camera)
                cam.nearClipPlane = this._camera.nearClipPlane;
            }
          }
        }

        if (Math.Abs(this._old_far_clip_plane - this._camera.farClipPlane) > this.TOLERANCE) {
          if (this._sync_culling_mask) {
            this._old_far_clip_plane = this._camera.farClipPlane;
            foreach (var cam in this._cameras) {
              if (cam != this._camera)
                cam.farClipPlane = this._camera.farClipPlane;
            }
          }
        }

        if (this._old_culling_mask != this._camera.cullingMask) {
          if (this._sync_culling_mask) {
            this._old_culling_mask = this._camera.cullingMask;
            foreach (var cam in this._cameras) {
              if (cam != this._camera)
                cam.cullingMask = this._camera.cullingMask;
            }
          }
        }
      } else
        print("No camera component found on gameobject");
    }
  }
}
