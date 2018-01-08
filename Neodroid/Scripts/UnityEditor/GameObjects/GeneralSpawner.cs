using Neodroid.Environments;
using Neodroid.Models.Actors;
using Neodroid.Models.Managers;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem(
      itemName : "GameObject/Neodroid/SimulationManager",
      isValidateFunction : false,
      priority : 10)]
    static void CreateSimulationManagerGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "SimulationManager");
      go.AddComponent<SimulationManager>();
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
      itemName : "GameObject/Neodroid/Environment",
      isValidateFunction : false,
      priority : 10)]
    static void CreateEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "Environment");
      var plane = GameObject.CreatePrimitive(type : PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<LearningEnvironment>();
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
      itemName : "GameObject/Neodroid/Actor",
      isValidateFunction : false,
      priority : 10)]
    static void CreateActorGameObject(MenuCommand menuCommand) {
      var go = new GameObject(name : "Actor");
      var capsule = GameObject.CreatePrimitive(type : PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
      GameObjectUtility.SetParentAndAlign(
                                          child : go,
                                          parent : menuCommand
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
