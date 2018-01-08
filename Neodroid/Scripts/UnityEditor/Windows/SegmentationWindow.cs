﻿#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.Segmentation;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.Windows {
  public class SegmentationWindow : EditorWindow {
    Texture _icon;

    Vector2 _scroll_position;

    [SerializeField] SegmentationColorByInstance[] _segmentation_colors_by_instance;

    [SerializeField] SegmentationColorByTag[] _segmentation_colors_by_tag;

    [MenuItem("Neodroid/SegmentationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(SegmentationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Icons/color_wheel.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Seg", this._icon, "Window for segmentation");
    }

    void OnGUI() {
      GUILayout.Label("Segmentation Colors", EditorStyles.boldLabel);
      var serialised_object = new SerializedObject(this);
      this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("By Tag");
      var material_changers_by_tag = FindObjectsOfType<ChangeMaterialOnRenderByTag>();
      foreach (var material_changer_by_tag in material_changers_by_tag) {
        this._segmentation_colors_by_tag = material_changer_by_tag.SegmentationColorsByTag;
        if (this._segmentation_colors_by_tag != null) {
          var tag_colors_property = serialised_object.FindProperty("_segmentation_colors_by_tag");
          EditorGUILayout.PropertyField(
              tag_colors_property,
              new GUIContent(material_changer_by_tag.name),
              true); // True means show children
          material_changer_by_tag._replace_untagged_color = EditorGUILayout.Toggle(
              "  -  Replace untagged colors",
              material_changer_by_tag._replace_untagged_color);
          material_changer_by_tag._untagged_color = EditorGUILayout.ColorField(
              "  -  Untagged color",
              material_changer_by_tag._untagged_color);
        }
      }

      EditorGUILayout.EndVertical();

      /*var material_changer = FindObjectOfType<ChangeMaterialOnRenderByTag> ();
    if(material_changer){
      _segmentation_colors_by_game_object = material_changer.SegmentationColors;
      SerializedProperty game_object_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_game_object");
      EditorGUILayout.PropertyField(tag_colors_property, true); // True means show children
    }*/
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("By Instance (Not changable, only for inspection) ");
      var material_changers_by_instance = FindObjectsOfType<ChangeMaterialOnRenderByInstance>();
      foreach (var material_changer_by_instance in material_changers_by_instance) {
        this._segmentation_colors_by_instance = material_changer_by_instance.InstanceColors;
        if (this._segmentation_colors_by_instance != null) {
          var instance_colors_property = serialised_object.FindProperty("_segmentation_colors_by_instance");
          EditorGUILayout.PropertyField(
              instance_colors_property,
              new GUIContent(material_changer_by_instance.name),
              true); // True means show children
        }
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
      serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
    }
  }
}
#endif
