using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class RenderTextureConfiguratorWindow : EditorWindow {
    private Texture _icon;
    private readonly int _preview_image_size = 100;
    private float[] _render_texture_height;
    private float[] _render_texture_width;

    private List<RenderTexture> _render_textures;

    private Vector2 _scroll_position;
    private Vector2 _texture_size;

    [MenuItem("Neodroid/RenderTextureConfiguratorWindow")]
    public static void ShowWindow() {
      GetWindow(
                typeof(RenderTextureConfiguratorWindow
                )); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/images.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Tex",
                                    _icon,
                                    "Window for RenderTexture configuration");
    }

    private void OnGUI() {
      _render_textures = new List<RenderTexture>();
      var cameras = FindObjectsOfType<Camera>();
      foreach (var camera in cameras)
        if (camera.targetTexture != null)
          _render_textures.Add(camera.targetTexture);

      _scroll_position = EditorGUILayout.BeginScrollView(_scroll_position);
      foreach (var render_texture in _render_textures) {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(render_texture.name);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var rect = GUILayoutUtility.GetRect(
                                            _preview_image_size,
                                            _preview_image_size);
        EditorGUI.DrawPreviewTexture(
                                     rect,
                                     render_texture);
        _texture_size = new Vector2(
                                    render_texture.width,
                                    render_texture.height);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
      }

      EditorGUILayout.EndScrollView();
      _texture_size = EditorGUILayout.Vector2Field(
                                                   "Set All Render Texture Sizes:",
                                                   _texture_size);
      if (GUILayout.Button("Apply(Does not work yet)"))
        foreach (var render_texture in _render_textures) {
//render_texture.width = (int)_texture_size[0]; // Read only property to change the asset, it has to be replaced with a new asset
//render_texture.height = (int)_texture_size[1]; // However it is easy to change run time genereted texture by just creating a new texure and replacing the old
        }
    }

    public void OnInspectorUpdate() { Repaint(); }
  }

  #endif
}
