using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor;

  public class RenderTextureConfiguratorWindow : EditorWindow {

    [MenuItem ("Neodroid/RenderTextureConfiguratorWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(RenderTextureConfiguratorWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    Vector2 _scroll_position;

    List<RenderTexture> _render_textures;
    float[] _render_texture_width;
    float[] _render_texture_height;
    int _preview_image_size = 100;
    Vector2 _texture_size;


  Texture _icon;


  void OnEnable () {
  _icon =  (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Neodroid/Icons/images.png", typeof(Texture2D)); 
  this.titleContent = new GUIContent("Neo:Tex",_icon,"Window for RenderTexture configuration");
  }

    void OnGUI () {
      _render_textures = new List<RenderTexture> ();    
      var cameras = FindObjectsOfType<Camera> ();
      foreach (var camera in cameras) {
        if (camera.targetTexture != null)
          _render_textures.Add (camera.targetTexture);
      }

      _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);
      foreach (var render_texture in _render_textures) {
        EditorGUILayout.BeginVertical ("Box");
        EditorGUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label (render_texture.name);
        GUILayout.FlexibleSpace ();
        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        Rect rect = GUILayoutUtility.GetRect (_preview_image_size, _preview_image_size);
        EditorGUI.DrawPreviewTexture (rect, render_texture);
        _texture_size = new Vector2 (render_texture.width, render_texture.height);
        GUILayout.FlexibleSpace ();
        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.EndVertical ();
      }
      EditorGUILayout.EndScrollView ();
      _texture_size = EditorGUILayout.Vector2Field ("Set All Render Texture Sizes:", _texture_size);
      if (GUILayout.Button ("Apply(Does not work yet)")) {
        foreach (var render_texture in _render_textures) {
//render_texture.width = (int)_texture_size[0]; // Read only property to change the asset, it has to be replaced with a new asset
//render_texture.height = (int)_texture_size[1]; // However it is easy to change run time genereted texture by just creating a new texure and replacing the old
        }
      }
    }

    public void OnInspectorUpdate () {
      this.Repaint ();
    }
  }

  #endif
}