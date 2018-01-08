using Neodroid.Environments;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables;
using Neodroid.Models.Managers;
using Neodroid.Models.Motors;
using Neodroid.Models.Observers;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class PrebuiltSpawner : MonoBehaviour {
    [MenuItem(
      itemName : "GameObject/Neodroid/Prebuilt/SimpleEnvironment",
      isValidateFunction : false,
      priority : 10)]
    static void CreateSingleEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "SingleEnvironment");
      go.AddComponent<SimulationManager>();
      go.AddComponent<LearningEnvironment>();

      var plane = GameObject.CreatePrimitive(type : PrimitiveType.Plane);
      plane.transform.parent = go.transform;

      var actor = new GameObject(name : "Actor");
      actor.AddComponent<Actor>();
      actor.AddComponent<TriTransformMotor>();
      actor.AddComponent<EulerTransformObserver>();
      actor.AddComponent<PositionConfigurable>();
      actor.transform.parent = go.transform;

      var capsule = GameObject.CreatePrimitive(type : PrimitiveType.Capsule);
      capsule.transform.parent = actor.transform;
      capsule.transform.localPosition = Vector3.up;

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
