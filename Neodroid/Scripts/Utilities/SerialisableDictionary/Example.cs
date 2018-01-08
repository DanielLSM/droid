using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.SerialisableDictionary {
  [CreateAssetMenu(menuName = "Example Asset")]
  public class Example : ScriptableObject {
    [SerializeField]
    GameObjectFloatDictionary _game_object_float_store =
        GameObjectFloatDictionary.New<GameObjectFloatDictionary>();

    [SerializeField]
    StringIntDictionary _string_integer_store = StringIntDictionary.New<StringIntDictionary>();

    Dictionary<string, int> StringIntegers { get { return this._string_integer_store.Dict; } }

    Dictionary<GameObject, float> Screenshots { get { return this._game_object_float_store.Dict; } }
  }
}
