﻿using Neodroid.Scripts.Utilities.NeodroidCamera;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class CameraSynchronisationWindow : EditorWindow {
    SynchroniseCameraProperties[] _cameras;

    Texture _icon;
    Vector2 _scroll_position;
    bool[] _show_camera_properties;

    [MenuItem("Neodroid/CameraSynchronisationWindow")]
    public static void ShowWindow() {
      GetWindow(
          typeof(CameraSynchronisationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      this.Setup();
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/arrow_refresh.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent(
          "Neo:Sync",
          this._icon,
          "Window for controlling syncronisation of cameras");
    }

    void Setup() {
      this._show_camera_properties = new bool[this._cameras.Length];
      for (var i = 0; i < this._cameras.Length; i++) this._show_camera_properties[i] = false;
    }

    void OnGUI() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      if (this._cameras.Length > 0) {
        var serialised_object = new SerializedObject(this);
        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
        if (this._show_camera_properties != null) {
          for (var i = 0; i < this._show_camera_properties.Length; i++) {
            this._show_camera_properties[i] = EditorGUILayout.Foldout(
                this._show_camera_properties[i],
                this._cameras[i].name);
            if (this._show_camera_properties[i]) {
              EditorGUILayout.BeginVertical("Box");
              this._cameras[i].SyncOrthographicSize = EditorGUILayout.Toggle(
                  "Synchronise Orthographic Size",
                  this._cameras[i].SyncOrthographicSize);
              this._cameras[i].SyncNearClipPlane = EditorGUILayout.Toggle(
                  "Synchronise Near Clip Plane",
                  this._cameras[i].SyncNearClipPlane);
              this._cameras[i].SyncFarClipPlane = EditorGUILayout.Toggle(
                  "Synchronise Far Clip Plane",
                  this._cameras[i].SyncFarClipPlane);
              this._cameras[i].SyncCullingMask = EditorGUILayout.Toggle(
                  "Synchronise Culling Mask",
                  this._cameras[i].SyncCullingMask);
              EditorGUILayout.EndVertical();
            }
          }
        }

        EditorGUILayout.EndScrollView();
        serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }

  #endif
}
