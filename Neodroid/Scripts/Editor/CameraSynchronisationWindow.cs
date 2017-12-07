using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Utilities.NeodroidCamera;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class CameraSynchronisationWindow : EditorWindow {

    [MenuItem ("Neodroid/CameraSynchronisationWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(CameraSynchronisationWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    SynchroniseCameraProperties[] _cameras;
    bool[] _show_camera_properties;

    Texture _icon;
    Vector2 _scroll_position;


    void OnEnable () {
      _cameras = FindObjectsOfType<SynchroniseCameraProperties> ();
      Setup ();
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Icons/arrow_refresh.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Sync", _icon, "Window for controlling syncronisation of cameras");
    }

    void Setup () {
      _show_camera_properties = new bool[_cameras.Length];
      for (int i = 0; i < _cameras.Length; i++) {
        _show_camera_properties [i] = false;
      }
    }

    void OnGUI () {
      _cameras = FindObjectsOfType<SynchroniseCameraProperties> ();
      if (_cameras.Length > 0) {
        SerializedObject serialised_object = new SerializedObject (this);
        _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);
        if (_show_camera_properties != null) {
          for (int i = 0; i < _show_camera_properties.Length; i++) {
            _show_camera_properties [i] = EditorGUILayout.Foldout (_show_camera_properties [i], _cameras [i].name);
            if (_show_camera_properties [i]) {
              EditorGUILayout.BeginVertical ("Box");
              _cameras [i]._orthographic_size = EditorGUILayout.Toggle ("Synchronise Orthographic Size", _cameras [i]._orthographic_size);
              _cameras [i]._near_clip_plane = EditorGUILayout.Toggle ("Synchronise Near Clip Plane", _cameras [i]._near_clip_plane);
              _cameras [i]._far_clip_plane = EditorGUILayout.Toggle ("Synchronise Far Clip Plane", _cameras [i]._far_clip_plane);
              EditorGUILayout.EndVertical ();
            }
          }
        }
        EditorGUILayout.EndScrollView ();
        serialised_object.ApplyModifiedProperties (); // Remember to apply modified properties
      }
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}