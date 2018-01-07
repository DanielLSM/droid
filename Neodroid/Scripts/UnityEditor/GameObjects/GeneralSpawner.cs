using Assets.Neodroid.Models.Actors;
using Neodroid.Environments;
using Neodroid.Managers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.GameObjects {
  #if UNITY_EDITOR
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem(
      "GameObject/Neodroid/SimulationManager",
      false,
      10)]
    private static void CreateSimulationManagerGameObject(MenuCommand menuCommand) {
      var go = new GameObject("SimulationManager");
      go.AddComponent<SimulationManager>();
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
      "GameObject/Neodroid/Environment",
      false,
      10)]
    private static void CreateEnvironmentGameObject(MenuCommand menuCommand) {
      var go = new GameObject("Environment");
      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<LearningEnvironment>();
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
      "GameObject/Neodroid/Actor",
      false,
      10)]
    private static void CreateActorGameObject(MenuCommand menuCommand) {
      var go = new GameObject("Actor");
      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
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
