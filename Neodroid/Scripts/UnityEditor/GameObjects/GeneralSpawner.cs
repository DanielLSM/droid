using Neodroid.Models.Actors;
using Neodroid.Models.Environments;
using Neodroid.Models.Managers;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem("GameObject/Neodroid/SimulationManager", false, 10)]
    static void CreateSimulationManagerGameObject(MenuCommand menu_command) {
      var go = new GameObject("SimulationManager");
      go.AddComponent<SimulationManager>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Environment", false, 10)]
    static void CreateEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("Environment");
      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<LearningEnvironment>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Actor", false, 10)]
    static void CreateActorGameObject(MenuCommand menuCommand) {
      var go = new GameObject("Actor");
      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menuCommand
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
  #endif
}
