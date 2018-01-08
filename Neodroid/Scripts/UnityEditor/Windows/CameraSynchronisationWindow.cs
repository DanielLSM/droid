using Neodroid.Scripts.Utilities.NeodroidCamera;

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

    [MenuItem(itemName : "Neodroid/CameraSynchronisationWindow")]
    public static void ShowWindow() {
      GetWindow(
                t : typeof(CameraSynchronisationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      this.Setup();
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                            assetPath :
                                                            "Assets/Neodroid/Icons/arrow_refresh.png",
                                                            type : typeof(Texture2D));
      this.titleContent = new GUIContent(
                                         text : "Neo:Sync",
                                         image : this._icon,
                                         tooltip : "Window for controlling syncronisation of cameras");
    }

    void Setup() {
      this._show_camera_properties = new bool[this._cameras.Length];
      for (var i = 0; i < this._cameras.Length; i++) this._show_camera_properties[i] = false;
    }

    void OnGUI() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      if (this._cameras.Length > 0) {
        var serialised_object = new SerializedObject(obj : this);
        this._scroll_position = EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
        if (this._show_camera_properties != null)
          for (var i = 0; i < this._show_camera_properties.Length; i++) {
            this._show_camera_properties[i] =
              EditorGUILayout.Foldout(
                                      foldout : this._show_camera_properties[i],
                                      content : this._cameras[i].name);
            if (this._show_camera_properties[i]) {
              EditorGUILayout.BeginVertical(style : "Box");
              this._cameras[i].SyncOrthographicSize = EditorGUILayout.Toggle(
                                                                                label :
                                                                                "Synchronise Orthographic Size",
                                                                                value : this._cameras[i]
                                                                                            .SyncOrthographicSize);
              this._cameras[i].SyncNearClipPlane =
                EditorGUILayout.Toggle(
                                       label : "Synchronise Near Clip Plane",
                                       value : this._cameras[i].SyncNearClipPlane);
              this._cameras[i].SyncFarClipPlane =
                EditorGUILayout.Toggle(
                                       label : "Synchronise Far Clip Plane",
                                       value : this._cameras[i].SyncFarClipPlane);
              this._cameras[i].SyncCullingMask =
                EditorGUILayout.Toggle(
                                       label : "Synchronise Culling Mask",
                                       value : this._cameras[i].SyncCullingMask);
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

    public void OnInspectorUpdate() { this.Repaint(); }
  }

  #endif
}
