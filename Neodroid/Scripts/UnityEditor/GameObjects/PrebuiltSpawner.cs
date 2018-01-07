using Assets.Neodroid.Models.Actors;
using Neodroid.Configurables;
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Motors;
using Neodroid.Observers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.GameObjects {
  #if UNITY_EDITOR
  public class PrebuiltSpawner : MonoBehaviour {
    [MenuItem(
      "GameObject/Neodroid/Prebuilt/SimpleEnvironment",
      false,
      10)]
    private static void CreateSingleEnvironmentGameObject(MenuCommand menuCommand) {
      var go = new GameObject("SingleEnvironment");
      go.AddComponent<SimulationManager>();
      go.AddComponent<LearningEnvironment>();

      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;

      var actor = new GameObject("Actor");
      actor.AddComponent<Actor>();
      actor.AddComponent<TriTransformMotor>();
      actor.AddComponent<EulerTransformObserver>();
      actor.AddComponent<TriTransformConfigurable>();
      actor.transform.parent = go.transform;

      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = actor.transform;
      capsule.transform.localPosition = Vector3.up;

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
