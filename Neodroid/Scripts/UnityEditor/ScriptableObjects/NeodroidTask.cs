#if UNITY_EDITOR
using Neodroid.Utilities;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Task {
  public static class CreateNeodroidTask {
    [MenuItem("Neodroid/Create/ScriptableObjects/NeodroidTask")]
    public static void CreateNeodroidTaskAsset() {
      var asset = ScriptableObject.CreateInstance<NeodroidTask>();

      AssetDatabase.CreateAsset(
                                asset,
                                "Assets/NewNeodroidTask.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
