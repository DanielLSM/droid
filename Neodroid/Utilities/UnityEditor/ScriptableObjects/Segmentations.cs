#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.ScriptableObjects {
  public static class CreateSegmentations {
    [MenuItem("Neodroid/Create/ScriptableObjects/Segmentations")]
    public static void CreateSegmentationsAsset() {
      var asset = ScriptableObject.CreateInstance<Segmentations>();

      AssetDatabase.CreateAsset(asset, "Assets/NewSegmentations.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
