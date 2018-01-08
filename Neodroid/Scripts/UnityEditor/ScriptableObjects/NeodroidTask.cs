#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.ScriptableObjects {
  public static class CreateNeodroidTask {
    [MenuItem(itemName : "Neodroid/Create/ScriptableObjects/NeodroidTask")]
    public static void CreateNeodroidTaskAsset() {
      var asset = ScriptableObject.CreateInstance<NeodroidTask>();

      AssetDatabase.CreateAsset(
                                asset : asset,
                                path : "Assets/NewNeodroidTask.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
