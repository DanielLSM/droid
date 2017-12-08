using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Neodroid.Utilities
{
    [CreateAssetMenu(fileName = "PlayerMotions", menuName = "Neodroid/Create/PlayerMotions", order = 1)]
    public class PlayerMotions : ScriptableObject
    {
        public PlayerMotion[] _player_motions;
    }

#if UNITY_EDITOR
    public class CreatePlayerMotions
    {
        [MenuItem("Neodroid/Create/PlayerMotions")]
        public static void CreatePlayerMotionsAsset()
        {
            var asset = ScriptableObject.CreateInstance<PlayerMotions>();

            AssetDatabase.CreateAsset(asset, "Assets/NewPlayerMotions.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
#endif
}