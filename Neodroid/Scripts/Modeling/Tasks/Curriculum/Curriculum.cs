using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Neodroid.Utilities {
  [CreateAssetMenu (fileName = "Curriculum", menuName = "Neodroid/Create/Curriculum", order = 1)]
  public class Curriculum : ScriptableObject {
    public int[] levels;
  }

  #if UNITY_EDITOR
  public class CreateCurriculum {
  [MenuItem ("Neodroid/Create/Curriculum")]
  public static void CreateCurriculumAsset () {
  var asset = ScriptableObject.CreateInstance<Curriculum> ();

  AssetDatabase.CreateAsset (asset, "Assets/NewCurriculum.asset");
  AssetDatabase.SaveAssets ();

  EditorUtility.FocusProjectWindow ();

  Selection.activeObject = asset;
  }
  }
  #endif
}