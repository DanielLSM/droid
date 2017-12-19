using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Environments;
using Neodroid.Actors;

namespace Neodroid.GameObjects {

  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

public class GeneralSpawner : MonoBehaviour {

  [MenuItem ("GameObject/Neodroid/SimulationManager", false, 10)]
  static void CreateSimulationManagerGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("SimulationManager");
  go.AddComponent<SimulationManager>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  [MenuItem ("GameObject/Neodroid/Environment", false, 10)]
  static void CreateEnvironmentGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("Environment");
  GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
  plane.transform.parent = go.transform;
  go.AddComponent<LearningEnvironment>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  [MenuItem ("GameObject/Neodroid/Actor", false, 10)]
  static void CreateActorGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("Actor");
  GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
  capsule.transform.parent = go.transform;
  go.AddComponent<Actor>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }


}
#endif
}