#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.ScriptableObjects {
  public static class CreatePlayerMotions {
    [MenuItem(itemName : "Neodroid/Create/ScriptableObjects/PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = ScriptableObject.CreateInstance<PlayerMotions>();

      AssetDatabase.CreateAsset(
                                asset : asset,
                                path : "Assets/NewPlayerMotions.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
