using Neodroid.Motors;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.GameObjects {
  #if UNITY_EDITOR
  public class MotorSpawner : MonoBehaviour {
    [MenuItem(
      "GameObject/Neodroid/Motors/Transform",
      false,
      10)]
    private static void CreateTransformMotorGameObject(MenuCommand menuCommand) {
      var go = new GameObject("TransformMotor");
      go.AddComponent<EulerTransformMotor>();
      GameObjectUtility.SetParentAndAlign(
                                          go,
                                          menuCommand
                                              .context as
                                            GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(
                                     go,
                                     "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(
      "GameObject/Neodroid/Motors/Rigidbody",
      false,
      10)]
    private static void CreateRigidbodyMotorGameObject(MenuCommand menuCommand) {
      var go = new GameObject("RigidbodyMotor");
      go.AddComponent<RigidbodyMotor>();
      GameObjectUtility.SetParentAndAlign(
                                          go,
                                          menuCommand
                                              .context as
                                            GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(
                                     go,
                                     "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
  #endif
}
