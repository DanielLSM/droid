using Neodroid.Models.Observers;
using Neodroid.Prototyping.Observers;
using Neodroid.Prototyping.Observers.General;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class ObserverSpawner : MonoBehaviour {
    [MenuItem("GameObject/Neodroid/Observers/Base", false, 10)]
    static void CreateObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject("Observer");
      go.AddComponent<Observer>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Observers/EulerTransform", false, 10)]
    static void CreateEulerTransformObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject("EulerTransformObserver");
      go.AddComponent<EulerTransformObserver>();
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
