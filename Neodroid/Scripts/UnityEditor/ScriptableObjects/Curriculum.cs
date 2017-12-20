
#if UNITY_EDITOR
using Neodroid.Configurables;
using UnityEngine;
using Neodroid.Utilities;
using UnityEditor;

namespace Neodroid.ScriptableObjects {

    public class CreateCurriculum
    {
  [MenuItem("Neodroid/Create/ScriptableObjects/Curriculum")]
        public static void CreateCurriculumAsset()
        {
            var asset = ScriptableObject.CreateInstance<Curriculum>();

            AssetDatabase.CreateAsset(asset, "Assets/NewCurriculum.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
#endif
