using Neodroid.Models.Motors;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class MotorSpawner : MonoBehaviour {
    [MenuItem("GameObject/Neodroid/Motors/Transform", false, 10)]
    static void CreateTransformMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject("TransformMotor");
      go.AddComponent<EulerTransformMotor>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Motors/Rigidbody", false, 10)]
    static void CreateRigidbodyMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject("RigidbodyMotor");
      go.AddComponent<RigidbodyMotor>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
  #endif
}
