using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.SerialisableDictionary {
  [CreateAssetMenu(menuName = "Example Asset")]
  public class Example : ScriptableObject {
    [SerializeField]
    private GameObjectFloatDictionary gameObjectFloatStore =
      GameObjectFloatDictionary.New<GameObjectFloatDictionary>();

    [SerializeField]
    private StringIntDictionary stringIntegerStore = StringIntDictionary.New<StringIntDictionary>();

    private Dictionary<string, int> stringIntegers { get { return stringIntegerStore.Dict; } }

    private Dictionary<GameObject, float> screenshots { get { return gameObjectFloatStore.Dict; } }
  }
}
