#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace Neodroid.Utilities {

    public class CreatePlayerMotions
    {
[MenuItem("Neodroid/Create/ScriptableObjects/PlayerMotions")]
        public static void CreatePlayerMotionsAsset()
        {
            var asset = ScriptableObject.CreateInstance<PlayerMotions>();

            AssetDatabase.CreateAsset(asset, "Assets/NewPlayerMotions.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
#endif
