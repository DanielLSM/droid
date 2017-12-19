using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Motors;

namespace Neodroid.GameObjects {

  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class MotorSpawner : MonoBehaviour {

  [MenuItem ("GameObject/Neodroid/Motors/Transform", false, 10)]
  static void CreateTransformMotorGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("TransformMotor");
  go.AddComponent<EulerTransformMotor>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  [MenuItem ("GameObject/Neodroid/Motors/Rigidbody", false, 10)]
  static void CreateRigidbodyMotorGameObject (MenuCommand menuCommand) {
  GameObject go = new GameObject ("RigidbodyMotor");
  go.AddComponent<RigidbodyMotor>();
  GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
  Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
  Selection.activeObject = go;
  }

  }
  #endif
}