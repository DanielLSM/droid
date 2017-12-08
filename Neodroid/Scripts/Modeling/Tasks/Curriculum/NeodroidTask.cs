using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Task {

  namespace Neodroid.Utilities {
    [CreateAssetMenu (fileName = "NeodroidTask", menuName = "Neodroid/Create/NeodroidTask", order = 1)]
    public class NeodroidTask : ScriptableObject {
      public Vector3 position;
      public float radius;
    }

    #if UNITY_EDITOR
    public class CreateNeodroidTask {
    [MenuItem ("Neodroid/Create/NeodroidTask")]
    public static void CreateNeodroidTaskAsset () {
    var asset = ScriptableObject.CreateInstance<NeodroidTask> ();

    AssetDatabase.CreateAsset (asset, "Assets/NewNeodroidTask.asset");
    AssetDatabase.SaveAssets ();

    EditorUtility.FocusProjectWindow ();

    Selection.activeObject = asset;
    }
    }
    #endif
  }
  public class NeodroidTask : MonoBehaviour {
    public NeodroidTask () {
    }
  }
}

