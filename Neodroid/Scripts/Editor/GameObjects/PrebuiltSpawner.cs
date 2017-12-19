using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Environments;
using Neodroid.Actors;
using Neodroid.Observers;
using Neodroid.Motors;
using Neodroid.Configurables;

namespace Neodroid.GameObjects {

  #if UNITY_EDITOR
    using UnityEditor.AnimatedValues;
    using UnityEditor;

    public class PrebuiltSpawner : MonoBehaviour {

    [MenuItem ("GameObject/Neodroid/Prebuilt/SimpleEnvironment", false, 10)]
    static void CreateSingleEnvironmentGameObject (MenuCommand menuCommand) {
    GameObject go = new GameObject ("SingleEnvironment");
    go.AddComponent<SimulationManager>();
    go.AddComponent<LearningEnvironment>();

    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
    plane.transform.parent = go.transform;

  GameObject actor = new GameObject ("Actor");
    actor.AddComponent<Actor>();
    actor.AddComponent<TriTransformMotor>();
    actor.AddComponent<EulerTransformObserver>();
    actor.AddComponent<TriTransformConfigurable>();
    actor.transform.parent = go.transform;

  GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
  capsule.transform.parent = actor.transform;
  capsule.transform.localPosition = Vector3.up;

    GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);    // Ensure it gets reparented if this was a context click (otherwise does nothing)
    Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);     // Register the creation in the undo system
    Selection.activeObject = go;
    }

    }
    #endif
}