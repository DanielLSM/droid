using Neodroid.Models.Observers;
using Neodroid.Models.Observers.General;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Scripts.UnityEditor.GameObjects {
  #if UNITY_EDITOR
  public class ObserverSpawner : MonoBehaviour {
    [MenuItem(
      itemName : "GameObject/Neodroid/Observers/Base",
      isValidateFunction : false,
      priority : 10)]
    static void CreateObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "Observer");
      go.AddComponent<Observer>();
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
      itemName : "GameObject/Neodroid/Observers/EulerTransform",
      isValidateFunction : false,
      priority : 10)]
    static void CreateEulerTransformObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject(name : "EulerTransformObserver");
      go.AddComponent<EulerTransformObserver>();
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
