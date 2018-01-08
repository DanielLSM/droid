using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityStandardAssets.Utility {
  public class TimedObjectActivator : MonoBehaviour {
    public enum Action {
      Activate,
      Deactivate,
      Destroy,
      ReloadLevel,
      Call
    }

    public Entries entries = new Entries();

    void Awake() {
      foreach (var entry in this.entries.entries)
        switch (entry.action) {
          case Action.Activate:
            this.StartCoroutine(routine : this.Activate(entry : entry));
            break;
          case Action.Deactivate:
            this.StartCoroutine(routine : this.Deactivate(entry : entry));
            break;
          case Action.Destroy:
            Destroy(
                    obj : entry.target,
                    t : entry.delay);
            break;

          case Action.ReloadLevel:
            this.StartCoroutine(routine : this.ReloadLevel(entry : entry));
            break;
          case Action.Call:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator Activate(Entry entry) {
      yield return new WaitForSeconds(seconds : entry.delay);
      entry.target.SetActive(value : true);
    }

    IEnumerator Deactivate(Entry entry) {
      yield return new WaitForSeconds(seconds : entry.delay);
      entry.target.SetActive(value : false);
    }

    IEnumerator ReloadLevel(Entry entry) {
      yield return new WaitForSeconds(seconds : entry.delay);
      SceneManager.LoadScene(sceneName : SceneManager.GetSceneAt(index : 0).name);
    }

    [Serializable]
    public class Entry {
      public Action action;
      public float delay;
      public GameObject target;
    }

    [Serializable]
    public class Entries {
      public Entry[] entries;
    }
  }
}

namespace UnityStandardAssets.Utility.Inspector {
  #if UNITY_EDITOR
  [CustomPropertyDrawer(type : typeof(TimedObjectActivator.Entries))]
  public class EntriesDrawer : PropertyDrawer {
    const float k_LineHeight = 18;
    const float k_Spacing = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(
                              totalPosition : position,
                              label : label,
                              property : property);

      var x = position.x;
      var y = position.y;
      var width = position.width;

      // Draw label
      EditorGUI.PrefixLabel(
                            totalPosition : position,
                            id : GUIUtility.GetControlID(focus : FocusType.Passive),
                            label : label);

      // Don't make child fields be indented
      var indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;

      var entries = property.FindPropertyRelative(relativePropertyPath : "entries");

      if (entries.arraySize > 0) {
        var actionWidth = .25f * width;
        var targetWidth = .6f * width;
        var delayWidth = .1f * width;
        var buttonWidth = .05f * width;

        for (var i = 0; i < entries.arraySize; ++i) {
          y += k_LineHeight + k_Spacing;

          var entry = entries.GetArrayElementAtIndex(index : i);

          var rowX = x;

          // Calculate rects
          var actionRect = new Rect(
                                    x : rowX,
                                    y : y,
                                    width : actionWidth,
                                    height : k_LineHeight);
          rowX += actionWidth;

          var targetRect = new Rect(
                                    x : rowX,
                                    y : y,
                                    width : targetWidth,
                                    height : k_LineHeight);
          rowX += targetWidth;

          var delayRect = new Rect(
                                   x : rowX,
                                   y : y,
                                   width : delayWidth,
                                   height : k_LineHeight);
          rowX += delayWidth;

          var buttonRect = new Rect(
                                    x : rowX,
                                    y : y,
                                    width : buttonWidth,
                                    height : k_LineHeight);
          rowX += buttonWidth;

          // Draw fields - passs GUIContent.none to each so they are drawn without labels

          if (entry.FindPropertyRelative(relativePropertyPath : "action").enumValueIndex
              != (int)TimedObjectActivator.Action.ReloadLevel) {
            EditorGUI.PropertyField(
                                    position : actionRect,
                                    property : entry.FindPropertyRelative(relativePropertyPath : "action"),
                                    label : GUIContent.none);
            EditorGUI.PropertyField(
                                    position : targetRect,
                                    property : entry.FindPropertyRelative(relativePropertyPath : "target"),
                                    label : GUIContent.none);
          } else {
            actionRect.width = actionRect.width + targetRect.width;
            EditorGUI.PropertyField(
                                    position : actionRect,
                                    property : entry.FindPropertyRelative(relativePropertyPath : "action"),
                                    label : GUIContent.none);
          }

          EditorGUI.PropertyField(
                                  position : delayRect,
                                  property : entry.FindPropertyRelative(relativePropertyPath : "delay"),
                                  label : GUIContent.none);
          if (GUI.Button(
                         position : buttonRect,
                         text : "-")) {
            entries.DeleteArrayElementAtIndex(index : i);
            break;
          }
        }
      }

      // add & sort buttons
      y += k_LineHeight + k_Spacing;

      var addButtonRect = new Rect(
                                   x : position.x + position.width - 120,
                                   y : y,
                                   width : 60,
                                   height : k_LineHeight);
      if (GUI.Button(
                     position : addButtonRect,
                     text : "Add"))
        entries.InsertArrayElementAtIndex(index : entries.arraySize);

      var sortButtonRect = new Rect(
                                    x : position.x + position.width - 60,
                                    y : y,
                                    width : 60,
                                    height : k_LineHeight);
      if (GUI.Button(
                     position : sortButtonRect,
                     text : "Sort")) {
        var changed = true;
        while (entries.arraySize > 1 && changed) {
          changed = false;
          for (var i = 0; i < entries.arraySize - 1; ++i) {
            var e1 = entries.GetArrayElementAtIndex(index : i);
            var e2 = entries.GetArrayElementAtIndex(index : i + 1);

            if (e1.FindPropertyRelative(relativePropertyPath : "delay").floatValue
                > e2.FindPropertyRelative(relativePropertyPath : "delay").floatValue) {
              entries.MoveArrayElement(
                                       srcIndex : i + 1,
                                       dstIndex : i);
              changed = true;
              break;
            }
          }
        }
      }

      // Set indent back to what it was
      EditorGUI.indentLevel = indent;
      //

      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var entries = property.FindPropertyRelative(relativePropertyPath : "entries");
      var lineAndSpace = k_LineHeight + k_Spacing;
      return 40 + entries.arraySize * lineAndSpace + lineAndSpace;
    }
  }
  #endif
}
