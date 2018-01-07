using Neodroid.Observers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.GameObjects {
  #if UNITY_EDITOR
  public class ObserverSpawner : MonoBehaviour {
    [MenuItem(
      "GameObject/Neodroid/Observers/Base",
      false,
      10)]
    private static void CreateObserverGameObject(MenuCommand menuCommand) {
      var go = new GameObject("Observer");
      go.AddComponent<Observer>();
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
      "GameObject/Neodroid/Observers/EulerTransform",
      false,
      10)]
    private static void CreateEulerTransformObserverGameObject(MenuCommand menuCommand) {
      var go = new GameObject("EulerTransformObserver");
      go.AddComponent<EulerTransformObserver>();
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
