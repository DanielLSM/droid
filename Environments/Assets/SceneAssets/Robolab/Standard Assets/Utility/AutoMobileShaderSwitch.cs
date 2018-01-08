using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityStandardAssets.Utility {
  public class AutoMobileShaderSwitch : MonoBehaviour {
    [SerializeField] ReplacementList m_ReplacementList;

    // Use this for initialization
    void OnEnable() {
      #if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_TIZEN || UNITY_STV
			var renderers = FindObjectsOfType<Renderer>();
			Debug.Log (renderers.Length+" renderers");
			var oldMaterials = new List<Material>();
			var newMaterials = new List<Material>();

			int materialsReplaced = 0;
			int materialInstancesReplaced = 0;

			foreach(ReplacementDefinition replacementDef in m_ReplacementList.items)
			{
				foreach(var r in renderers)
				{
					Material[] modifiedMaterials = null;
					for(int n = 0; n<r.sharedMaterials.Length; ++n)
					{
						var material = r.sharedMaterials[n];
						if (material.shader == replacementDef.original)
						{
							if (modifiedMaterials == null)
							{
								modifiedMaterials = r.materials;
							}
							if (!oldMaterials.Contains(material))
							{
								oldMaterials.Add(material);
								Material newMaterial = (Material)Instantiate(material);
								newMaterial.shader = replacementDef.replacement;
								newMaterials.Add(newMaterial);
								++materialsReplaced;
							}
							Debug.Log ("replacing "+r.gameObject.name+" renderer "+n+" with "+newMaterials[oldMaterials.IndexOf(material)].name);
							modifiedMaterials[n] = newMaterials[oldMaterials.IndexOf(material)];
							++materialInstancesReplaced;
						}
					}
					if (modifiedMaterials != null)
					{
						r.materials = modifiedMaterials;
					}
				}
			}
			Debug.Log (materialInstancesReplaced+" material instances replaced");
			Debug.Log (materialsReplaced+" materials replaced");
			for(int n = 0; n<oldMaterials.Count; ++n)
			{
				Debug.Log (oldMaterials[n].name+" ("+oldMaterials[n].shader.name+")"+" replaced with "+newMaterials[n].name+" ("+newMaterials[n].shader.name+")");
			}
            #endif
    }

    [Serializable]
    public class ReplacementDefinition {
      public Shader original = null;
      public Shader replacement = null;
    }

    [Serializable]
    public class ReplacementList {
      public ReplacementDefinition[] items = new ReplacementDefinition[0];
    }
  }
}

namespace UnityStandardAssets.Utility.Inspector {
  #if UNITY_EDITOR
  [CustomPropertyDrawer(type : typeof(AutoMobileShaderSwitch.ReplacementList))]
  public class ReplacementListDrawer : PropertyDrawer {
    const float k_LineHeight = 18;
    const float k_Spacing = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(
                              totalPosition : position,
                              label : label,
                              property : property);

      var x = position.x;
      var y = position.y;
      var inspectorWidth = position.width;

      // Don't make child fields be indented
      var indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;

      var items = property.FindPropertyRelative(relativePropertyPath : "items");
      var titles = new[] {
                           "Original",
                           "Replacement",
                           ""
                         };
      var props = new[] {
                          "original",
                          "replacement",
                          "-"
                        };
      var widths = new[] {
                           .45f,
                           .45f,
                           .1f
                         };
      const float lineHeight = 18;
      var changedLength = false;
      if (items.arraySize > 0)
        for (var i = -1; i < items.arraySize; ++i) {
          var item = items.GetArrayElementAtIndex(index : i);

          var rowX = x;
          for (var n = 0; n < props.Length; ++n) {
            var w = widths[n] * inspectorWidth;

            // Calculate rects
            var rect = new Rect(
                                x : rowX,
                                y : y,
                                width : w,
                                height : lineHeight);
            rowX += w;

            if (i == -1) {
              // draw title labels
              EditorGUI.LabelField(
                                   position : rect,
                                   label : titles[n]);
            } else {
              if (props[n] == "-" || props[n] == "^" || props[n] == "v") {
                if (GUI.Button(
                               position : rect,
                               text : props[n]))
                  switch (props[n]) {
                    case "-":
                      items.DeleteArrayElementAtIndex(index : i);
                      items.DeleteArrayElementAtIndex(index : i);
                      changedLength = true;
                      break;
                    case "v":
                      if (i > 0)
                        items.MoveArrayElement(
                                               srcIndex : i,
                                               dstIndex : i + 1);
                      break;
                    case "^":
                      if (i < items.arraySize - 1)
                        items.MoveArrayElement(
                                               srcIndex : i,
                                               dstIndex : i - 1);
                      break;
                  }
              } else {
                var prop = item.FindPropertyRelative(relativePropertyPath : props[n]);
                EditorGUI.PropertyField(
                                        position : rect,
                                        property : prop,
                                        label : GUIContent.none);
              }
            }
          }

          y += lineHeight + k_Spacing;
          if (changedLength) break;
        }

      // add button
      var addButtonRect = new Rect(
                                   x : x + position.width - widths[widths.Length - 1] * inspectorWidth,
                                   y : y,
                                   width : widths[widths.Length - 1] * inspectorWidth,
                                   height : lineHeight);
      if (GUI.Button(
                     position : addButtonRect,
                     text : "+")) items.InsertArrayElementAtIndex(index : items.arraySize);

      y += lineHeight + k_Spacing;

      // Set indent back to what it was
      EditorGUI.indentLevel = indent;
      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var items = property.FindPropertyRelative(relativePropertyPath : "items");
      var lineAndSpace = k_LineHeight + k_Spacing;
      return 40 + items.arraySize * lineAndSpace + lineAndSpace;
    }
  }
  #endif
}
