using Neodroid.Utilities.NeodroidCamera;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class CameraSynchronisationWindow : EditorWindow {
    private SynchroniseCameraProperties[] _cameras;

    private Texture _icon;
    private Vector2 _scroll_position;
    private bool[] _show_camera_properties;

    [MenuItem("Neodroid/CameraSynchronisationWindow")]
    public static void ShowWindow() {
      GetWindow(
                typeof(CameraSynchronisationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      Setup();
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/arrow_refresh.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Sync",
                                    _icon,
                                    "Window for controlling syncronisation of cameras");
    }

    private void Setup() {
      _show_camera_properties = new bool[_cameras.Length];
      for (var i = 0; i < _cameras.Length; i++) _show_camera_properties[i] = false;
    }

    private void OnGUI() {
      _cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      if (_cameras.Length > 0) {
        var serialised_object = new SerializedObject(this);
        _scroll_position = EditorGUILayout.BeginScrollView(_scroll_position);
        if (_show_camera_properties != null)
          for (var i = 0; i < _show_camera_properties.Length; i++) {
            _show_camera_properties[i] =
              EditorGUILayout.Foldout(
                                      _show_camera_properties[i],
                                      _cameras[i].name);
            if (_show_camera_properties[i]) {
              EditorGUILayout.BeginVertical("Box");
              _cameras[i]._sync_orthographic_size = EditorGUILayout.Toggle(
                                                                           "Synchronise Orthographic Size",
                                                                           _cameras[i]
                                                                             ._sync_orthographic_size);
              _cameras[i]._sync_near_clip_plane =
                EditorGUILayout.Toggle(
                                       "Synchronise Near Clip Plane",
                                       _cameras[i]._sync_near_clip_plane);
              _cameras[i]._sync_far_clip_plane =
                EditorGUILayout.Toggle(
                                       "Synchronise Far Clip Plane",
                                       _cameras[i]._sync_far_clip_plane);
              _cameras[i]._sync_culling_mask =
                EditorGUILayout.Toggle(
                                       "Synchronise Culling Mask",
                                       _cameras[i]._sync_culling_mask);
              EditorGUILayout.EndVertical();
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

    public void OnInspectorUpdate() { Repaint(); }
  }

  #endif
}
