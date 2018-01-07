#if UNITY_EDITOR
using Neodroid.Utilities;
using UnityEditor;
using UnityEngine;

namespace Neodroid.ScriptableObjects {
  public static class CreateCurriculum {
    [MenuItem("Neodroid/Create/ScriptableObjects/Curriculum")]
    public static void CreateCurriculumAsset() {
      var asset = ScriptableObject.CreateInstance<Curriculum>();

      AssetDatabase.CreateAsset(
                                asset,
                                "Assets/NewCurriculum.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
