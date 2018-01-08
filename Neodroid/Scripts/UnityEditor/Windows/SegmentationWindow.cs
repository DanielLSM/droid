#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.Segmentation;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.Windows {
  public class SegmentationWindow : EditorWindow {
    Texture _icon;

    Vector2 _scroll_position;

    [SerializeField] SegmentationColorByInstance[] _segmentation_colors_by_instance;

    [SerializeField] SegmentationColorByTag[] _segmentation_colors_by_tag;

    [MenuItem(itemName : "Neodroid/SegmentationWindow")]
    public static void ShowWindow() {
      GetWindow(
                t : typeof(SegmentationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                            assetPath :
                                                            "Assets/Neodroid/Icons/color_wheel.png",
                                                            type : typeof(Texture2D));
      this.titleContent = new GUIContent(
                                         text : "Neo:Seg",
                                         image : this._icon,
                                         tooltip : "Window for segmentation");
    }

    void OnGUI() {
      GUILayout.Label(
                      text : "Segmentation Colors",
                      style : EditorStyles.boldLabel);
      var serialised_object = new SerializedObject(obj : this);
      this._scroll_position = EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      EditorGUILayout.BeginVertical(style : "Box");
      GUILayout.Label(text : "By Tag");
      var material_changers_by_tag = FindObjectsOfType<ChangeMaterialOnRenderByTag>();
      foreach (var material_changer_by_tag in material_changers_by_tag) {
        this._segmentation_colors_by_tag = material_changer_by_tag.SegmentationColorsByTag;
        if (this._segmentation_colors_by_tag != null) {
          var tag_colors_property =
            serialised_object.FindProperty(propertyPath : "_segmentation_colors_by_tag");
          EditorGUILayout.PropertyField(
                                        property : tag_colors_property,
                                        label : new GUIContent(text : material_changer_by_tag.name),
                                        includeChildren : true); // True means show children
          material_changer_by_tag._replace_untagged_color = EditorGUILayout.Toggle(
                                                                                   label :
                                                                                   "  -  Replace untagged colors",
                                                                                   value :
                                                                                   material_changer_by_tag
                                                                                     ._replace_untagged_color);
          material_changer_by_tag._untagged_color = EditorGUILayout.ColorField(
                                                                               label : "  -  Untagged color",
                                                                               value : material_changer_by_tag
                                                                                 ._untagged_color);
        }
      }

      EditorGUILayout.EndVertical();

      /*var material_changer = FindObjectOfType<ChangeMaterialOnRenderByTag> ();
    if(material_changer){
      _segmentation_colors_by_game_object = material_changer.SegmentationColors;
      SerializedProperty game_object_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_game_object");
      EditorGUILayout.PropertyField(tag_colors_property, true); // True means show children
    }*/
      EditorGUILayout.BeginVertical(style : "Box");
      GUILayout.Label(text : "By Instance (Not changable, only for inspection) ");
      var material_changers_by_instance = FindObjectsOfType<ChangeMaterialOnRenderByInstance>();
      foreach (var material_changer_by_instance in material_changers_by_instance) {
        this._segmentation_colors_by_instance = material_changer_by_instance.InstanceColors;
        if (this._segmentation_colors_by_instance != null) {
          var instance_colors_property =
            serialised_object.FindProperty(propertyPath : "_segmentation_colors_by_instance");
          EditorGUILayout.PropertyField(
                                        property : instance_colors_property,
                                        label : new GUIContent(text : material_changer_by_instance.name),
                                        includeChildren : true); // True means show children
        }
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
      serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
    }
  }
}
#endif
