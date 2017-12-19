using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Observers;

namespace Neodroid.GameObjects {

  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class ObserverSpawner : MonoBehaviour {
 

  [MenuItem ("GameObject/Neodroid/Observers/Base", false, 10)]
  static void CreateObserverGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("Observer");
  go.AddComponent<Observer>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  [MenuItem ("GameObject/Neodroid/Observers/EulerTransform", false, 10)]
  static void CreateEulerTransformObserverGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("EulerTransformObserver");
  go.AddComponent<EulerTransformObserver>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  }
  #endif
}