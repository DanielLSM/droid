using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.SerialisableDictionary {
  public abstract class SerializableDictionary<K, V> : ISerializationCallbackReceiver {
    public Dictionary<K, V> Dict;

    [SerializeField]
    private K[] _keys;

    [SerializeField]
    private V[] _values;

    public void OnAfterDeserialize() {
      var c = _keys.Length;
      Dict = new Dictionary<K, V>(c);
      for (var i = 0; i < c; i++) Dict[_keys[i]] = _values[i];
      _keys = null;
      _values = null;
    }

    public void OnBeforeSerialize() {
      var c = Dict.Count;
      _keys = new K[c];
      _values = new V[c];
      var i = 0;
      using (var e = Dict.GetEnumerator()) {
        while (e.MoveNext()) {
          var kvp = e.Current;
          _keys[i] = kvp.Key;
          _values[i] = kvp.Value;
          i++;
        }
      }
    }

    public static T New<T>()
      where T : SerializableDictionary<K, V>, new() {
      var result = new T {
                           Dict = new Dictionary<K, V>()
                         };
      return result;
    }
  }
}
