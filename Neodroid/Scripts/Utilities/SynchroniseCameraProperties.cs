﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[Serializable]
public class SynchroniseCameraProperties : MonoBehaviour {

  public bool _orthographic_size = true;
  public bool _near_clip_plane = true;
  public bool _far_clip_plane = true;

  float _old_near_clip_plane;
  float _old_far_clip_plane;
  float _old_orthographic_size;

  Camera _camera;
  Camera[] _cameras;

  public void Start(){
    _camera = GetComponent<Camera> ();
    if (_camera) {
      _old_orthographic_size = _camera.orthographicSize;
      _old_near_clip_plane = _camera.nearClipPlane;
      _old_far_clip_plane = _camera.farClipPlane;

      _cameras = FindObjectsOfType<Camera> ();
    } else {
      print ("No camera component found on gameobject");
    }

  }

  public void Update(){
    if (_camera) {
      if (_old_orthographic_size != _camera.orthographicSize) {
        _old_orthographic_size = _camera.orthographicSize;
        foreach (Camera camera in _cameras) {
          if (camera != _camera) {
            camera.orthographicSize = _camera.orthographicSize;
          }
        }

      }
      if (_old_near_clip_plane != _camera.nearClipPlane) {
        _old_near_clip_plane = _camera.nearClipPlane;
        foreach (Camera camera in _cameras) {
          if (camera != _camera) {
            camera.nearClipPlane = _camera.nearClipPlane;
          }
        }
      }
      if (_old_far_clip_plane != _camera.farClipPlane) {
        _old_far_clip_plane = _camera.farClipPlane;
        foreach (Camera camera in _cameras) {
          if (camera != _camera) {
            camera.farClipPlane = _camera.farClipPlane;
          }
        }
      }

    }
    else {
      print ("No camera component found on gameobject");
    }

  }
}
