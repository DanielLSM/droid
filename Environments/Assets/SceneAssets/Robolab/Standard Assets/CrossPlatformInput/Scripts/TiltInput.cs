using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityStandardAssets.CrossPlatformInput {
  // helps with managing tilt input on mobile devices
  public class TiltInput : MonoBehaviour {
    // options for the various orientations
    public enum AxisOptions {
      ForwardAxis,
      SidewaysAxis
    }

    public float centreAngleOffset;
    public float fullTiltAngle = 25;

    CrossPlatformInputManager.VirtualAxis m_SteerAxis;

    public AxisMapping mapping;
    public AxisOptions tiltAroundAxis = AxisOptions.ForwardAxis;

    void OnEnable() {
      if (this.mapping.type == AxisMapping.MappingType.NamedAxis) {
        this.m_SteerAxis = new CrossPlatformInputManager.VirtualAxis(name : this.mapping.axisName);
        CrossPlatformInputManager.RegisterVirtualAxis(axis : this.m_SteerAxis);
      }
    }

    void Update() {
      float angle = 0;
      if (Input.acceleration != Vector3.zero)
        switch (this.tiltAroundAxis) {
          case AxisOptions.ForwardAxis:
            angle = Mathf.Atan2(
                                y : Input.acceleration.x,
                                x : -Input.acceleration.y)
                    * Mathf.Rad2Deg
                    + this.centreAngleOffset;
            break;
          case AxisOptions.SidewaysAxis:
            angle = Mathf.Atan2(
                                y : Input.acceleration.z,
                                x : -Input.acceleration.y)
                    * Mathf.Rad2Deg
                    + this.centreAngleOffset;
            break;
        }

      var axisValue = Mathf.InverseLerp(
                                        a : -this.fullTiltAngle,
                                        b : this.fullTiltAngle,
                                        value : angle)
                      * 2
                      - 1;
      switch (this.mapping.type) {
        case AxisMapping.MappingType.NamedAxis:
          this.m_SteerAxis.Update(value : axisValue);
          break;
        case AxisMapping.MappingType.MousePositionX:
          CrossPlatformInputManager.SetVirtualMousePositionX(f : axisValue * Screen.width);
          break;
        case AxisMapping.MappingType.MousePositionY:
          CrossPlatformInputManager.SetVirtualMousePositionY(f : axisValue * Screen.width);
          break;
        case AxisMapping.MappingType.MousePositionZ:
          CrossPlatformInputManager.SetVirtualMousePositionZ(f : axisValue * Screen.width);
          break;
      }
    }

    void OnDisable() { this.m_SteerAxis.Remove(); }

    [Serializable]
    public class AxisMapping {
      public enum MappingType {
        NamedAxis,
        MousePositionX,
        MousePositionY,
        MousePositionZ
      }

      public string axisName;

      public MappingType type;
    }
  }
}

namespace UnityStandardAssets.CrossPlatformInput.Inspector {
  #if UNITY_EDITOR
  [CustomPropertyDrawer(type : typeof(TiltInput.AxisMapping))]
  public class TiltInputAxisStylePropertyDrawer : PropertyDrawer {
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

      var props = new[] {
                          "type",
                          "axisName"
                        };
      var widths = new[] {
                           .4f,
                           .6f
                         };
      if (property.FindPropertyRelative(relativePropertyPath : "type").enumValueIndex > 0) {
        // hide name if not a named axis
        props = new[] {
                        "type"
                      };
        widths = new[] {
                         1f
                       };
      }

      const float lineHeight = 18;
      for (var n = 0; n < props.Length; ++n) {
        var w = widths[n] * inspectorWidth;

        // Calculate rects
        var rect = new Rect(
                            x : x,
                            y : y,
                            width : w,
                            height : lineHeight);
        x += w;

        EditorGUI.PropertyField(
                                position : rect,
                                property : property.FindPropertyRelative(relativePropertyPath : props[n]),
                                label : GUIContent.none);
      }

      // Set indent back to what it was
      EditorGUI.indentLevel = indent;
      EditorGUI.EndProperty();
    }
  }
  #endif
}
