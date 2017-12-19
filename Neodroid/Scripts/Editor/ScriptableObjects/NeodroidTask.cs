using UnityEngine;
using Neodroid.Utilities;

#if UNITY_EDITOR
using UnityEditor;

namespace Neodroid.Task {

        public class CreateNeodroidTask
        {
[MenuItem("Neodroid/Create/ScriptableObjects/NeodroidTask")]
            public static void CreateNeodroidTaskAsset()
            {
                var asset = ScriptableObject.CreateInstance<NeodroidTask>();

                AssetDatabase.CreateAsset(asset, "Assets/NewNeodroidTask.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }
#endif
}


