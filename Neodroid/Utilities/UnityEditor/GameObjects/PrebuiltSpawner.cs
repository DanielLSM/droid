using Neodroid.Managers;
using Neodroid.Models.Actors;
using Neodroid.Models.Configurables;
using Neodroid.Models.Environments;
using Neodroid.Models.Managers;
using Neodroid.Models.Motors;
using Neodroid.Models.Observers;
using Neodroid.Prototyping.Observers;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class PrebuiltSpawner : MonoBehaviour {
    [MenuItem("GameObject/Neodroid/Prebuilt/SimpleEnvironment", false, 10)]
    static void CreateSingleEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("SingleEnvironment");
      go.AddComponent<PausableManager>();
      go.AddComponent<PrototypingEnvironment>();

      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;

      var actor = new GameObject("Actor");
      actor.AddComponent<Actor>();
      actor.AddComponent<TriTransformMotor>();
      actor.AddComponent<EulerTransformObserver>();
      actor.AddComponent<PositionConfigurable>();
      actor.transform.parent = go.transform;

      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = actor.transform;
      capsule.transform.localPosition = Vector3.up;

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
