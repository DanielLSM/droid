using Neodroid.Segmentation;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class SegmentationWindow : EditorWindow {
    private Texture _icon;

    private Vector2 _scroll_position;
        [SerializeField]
    SegmentationColorByInstance[] _segmentation_colors_by_instance; 
    [SerializeField]
    SegmentationColorByTag[] _segmentation_colors_by_tag;

    [MenuItem("Neodroid/SegmentationWindow")]
    public static void ShowWindow() {
      GetWindow(
                typeof(SegmentationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/color_wheel.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Seg",
                                    _icon,
                                    "Window for segmentation");
    }

    private void OnGUI() {
      GUILayout.Label(
                      "Segmentation Colors",
                      EditorStyles.boldLabel);
      var serialised_object = new SerializedObject(this);

      _scroll_position = EditorGUILayout.BeginScrollView(_scroll_position);
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("By Tag");
      var material_changers_by_tag = FindObjectsOfType<ChangeMaterialOnRenderByTag>();
      foreach (var material_changer_by_tag in material_changers_by_tag) {
        _segmentation_colors_by_tag = material_changer_by_tag.SegmentationColorsByTag;
        var tag_colors_property = serialised_object.FindProperty("_segmentation_colors_by_tag");
        EditorGUILayout.PropertyField(
                                      tag_colors_property,
                                      new GUIContent(material_changer_by_tag.name),
                                      true); // True means show children
        material_changer_by_tag._replace_untagged_color = EditorGUILayout.Toggle(
                                                                                 "  -  Replace untagged colors",
                                                                                 material_changer_by_tag
                                                                                   ._replace_untagged_color);
        material_changer_by_tag._untagged_color = EditorGUILayout.ColorField(
                                                                             "  -  Untagged color",
                                                                             material_changer_by_tag
                                                                               ._untagged_color);
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
        _segmentation_colors_by_instance = material_changer_by_instance.InstanceColors;
        var instance_colors_property =
          serialised_object.FindProperty("_segmentation_colors_by_instance");
        EditorGUILayout.PropertyField(
                                      instance_colors_property,
                                      new GUIContent(material_changer_by_instance.name),
                                      true); // True means show children
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
      serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
    }
  }
  #endif
}
