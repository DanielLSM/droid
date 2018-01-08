#if UNITY_EDITOR
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Scripts.UnityEditor.ScriptableObjects {
  public static class CreateCurriculum {
    [MenuItem(itemName : "Neodroid/Create/ScriptableObjects/Curriculum")]
    public static void CreateCurriculumAsset() {
      var asset = ScriptableObject.CreateInstance<Curriculum>();

      AssetDatabase.CreateAsset(
                                asset : asset,
                                path : "Assets/NewCurriculum.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
