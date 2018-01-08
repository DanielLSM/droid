using Neodroid.Models.Motors;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class MotorSpawner : MonoBehaviour {
    [MenuItem(
      itemName : "GameObject/Neodroid/Motors/Transform",
      isValidateFunction : false,
      priority : 10)]
    static void CreateTransformMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "TransformMotor");
      go.AddComponent<EulerTransformMotor>();
      GameObjectUtility.SetParentAndAlign(
                                          child : go,
                                          parent : menu_command
                                                       .context as
                                                     GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(
                                     objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(
      itemName : "GameObject/Neodroid/Motors/Rigidbody",
      isValidateFunction : false,
      priority : 10)]
    static void CreateRigidbodyMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "RigidbodyMotor");
      go.AddComponent<RigidbodyMotor>();
      GameObjectUtility.SetParentAndAlign(
                                          child : go,
                                          parent : menu_command
                                                       .context as
                                                     GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(
                                     objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
  #endif
}
