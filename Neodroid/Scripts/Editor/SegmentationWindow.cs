using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Segmentation;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor;

  public class SegmentationWindow : EditorWindow {

    [MenuItem ("Neodroid/SegmentationWindow")]
    public static void ShowWindow () {
      EditorWindow.GetWindow (typeof(SegmentationWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    public SegmentationColorByTag[] _segmentation_colors_by_tag;
    public SegmentationColorByInstance[] _segmentation_colors_by_instance;

    Vector2 _scroll_position;
    Texture _icon;

    void OnEnable () {
  _icon =  (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Neodroid/Icons/color_wheel.png", typeof(Texture2D));
  this.titleContent = new GUIContent("Neo:Seg",_icon,"Window for segmentation");
    }

    void OnGUI () {
      GUILayout.Label ("Segmentation Colors", EditorStyles.boldLabel);
      SerializedObject serialised_object = new SerializedObject (this);
   
  _scroll_position = EditorGUILayout.BeginScrollView (_scroll_position);
      EditorGUILayout.BeginVertical ("Box");
      GUILayout.Label ("By Tag");
      var material_changers_by_tag = FindObjectsOfType<ChangeMaterialOnRenderByTag> ();
      foreach (ChangeMaterialOnRenderByTag material_changer_by_tag in material_changers_by_tag) {
        _segmentation_colors_by_tag = material_changer_by_tag.SegmentationColorsByTag;
        SerializedProperty tag_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_tag");
        EditorGUILayout.PropertyField (tag_colors_property, new GUIContent (material_changer_by_tag.name), true); // True means show children
        material_changer_by_tag._replace_untagged_color = EditorGUILayout.Toggle ("  -  Replace untagged colors", material_changer_by_tag._replace_untagged_color);
        material_changer_by_tag._untagged_color = EditorGUILayout.ColorField ("  -  Untagged color", material_changer_by_tag._untagged_color);
      }
      EditorGUILayout.EndVertical ();

      /*var material_changer = FindObjectOfType<ChangeMaterialOnRenderByTag> ();
    if(material_changer){
      _segmentation_colors_by_game_object = material_changer.SegmentationColors;
      SerializedProperty game_object_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_game_object");
      EditorGUILayout.PropertyField(tag_colors_property, true); // True means show children
    }*/
      EditorGUILayout.BeginVertical ("Box");
      GUILayout.Label ("By Instance (Not changable, only for inspection) ");
      var material_changers_by_instance = FindObjectsOfType<ChangeMaterialOnRenderByInstance> ();
      foreach (ChangeMaterialOnRenderByInstance material_changer_by_instance in material_changers_by_instance) {
        _segmentation_colors_by_instance = material_changer_by_instance.InstanceColors;
        SerializedProperty instance_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_instance");
        EditorGUILayout.PropertyField (instance_colors_property, new GUIContent (material_changer_by_instance.name), true); // True means show children
      }
      EditorGUILayout.EndVertical ();
      EditorGUILayout.EndScrollView ();
      serialised_object.ApplyModifiedProperties (); // Remember to apply modified properties
    }
  }
  #endif
}