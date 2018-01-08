using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class RenderTextureConfiguratorWindow : EditorWindow {
    readonly int _preview_image_size = 100;
    Texture _icon;
    float[] _render_texture_height;
    float[] _render_texture_width;

    List<RenderTexture> _render_textures;

    Vector2 _scroll_position;
    Vector2 _texture_size;

    [MenuItem(itemName : "Neodroid/RenderTextureConfiguratorWindow")]
    public static void ShowWindow() {
      GetWindow(
                t : typeof(RenderTextureConfiguratorWindow
                )); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                            assetPath : "Assets/Neodroid/Icons/images.png",
                                                            type : typeof(Texture2D));
      this.titleContent = new GUIContent(
                                         text : "Neo:Tex",
                                         image : this._icon,
                                         tooltip : "Window for RenderTexture configuration");
    }

    void OnGUI() {
      this._render_textures = new List<RenderTexture>();
      var cameras = FindObjectsOfType<Camera>();
      foreach (var camera in cameras)
        if (camera.targetTexture != null)
          this._render_textures.Add(item : camera.targetTexture);

      this._scroll_position = EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      foreach (var render_texture in this._render_textures) {
        EditorGUILayout.BeginVertical(style : "Box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text : render_texture.name);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var rect = GUILayoutUtility.GetRect(
                                            width : this._preview_image_size,
                                            height : this._preview_image_size);
        EditorGUI.DrawPreviewTexture(
                                     position : rect,
                                     image : render_texture);
        this._texture_size = new Vector2(
                                         x : render_texture.width,
                                         y : render_texture.height);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
      }

      EditorGUILayout.EndScrollView();
      this._texture_size = EditorGUILayout.Vector2Field(
                                                        label : "Set All Render Texture Sizes:",
                                                        value : this._texture_size);
      if (GUILayout.Button(text : "Apply(Does not work yet)"))
        foreach (var render_texture in this._render_textures) {
//render_texture.width = (int)_texture_size[0]; // Read only property to change the asset, it has to be replaced with a new asset
//render_texture.height = (int)_texture_size[1]; // However it is easy to change run time genereted texture by just creating a new texure and replacing the old
        }
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }

  #endif
}
