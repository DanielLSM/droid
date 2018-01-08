#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.ScriptableObjects {
  public static class CreatePlayerMotions {
    [MenuItem("Neodroid/Create/ScriptableObjects/PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = ScriptableObject.CreateInstance<PlayerMotions>();

      AssetDatabase.CreateAsset(asset, "Assets/NewPlayerMotions.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
