using System.Collections;
using System.Collections.Generic;
using Neodroid.Configurations;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Neodroid.Utilities {
  [Serializable]
  public struct level {
    public configurable_entry[] configurable_entries;
    public float min_reward;
    public float max_reward;
  }

  [Serializable]
  public struct configurable_entry {
    public string configurable_name;
    public float min_value;
    public float max_value;
  }

  [CreateAssetMenu (fileName = "Curriculum", menuName = "Neodroid/Create/Curriculum", order = 1)]
  public class Curriculum : ScriptableObject {
    public level[] _levels;
  }

  #if UNITY_EDITOR
    public class CreateCurriculum
    {
        [MenuItem("Neodroid/Create/Curriculum")]
        public static void CreateCurriculumAsset()
        {
            var asset = ScriptableObject.CreateInstance<Curriculum>();

            AssetDatabase.CreateAsset(asset, "Assets/NewCurriculum.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
#endif
}