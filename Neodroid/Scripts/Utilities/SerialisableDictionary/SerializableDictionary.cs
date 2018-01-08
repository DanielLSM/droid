using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.SerialisableDictionary {
  public abstract class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver {
    [SerializeField] TK[] _keys;

    [SerializeField] TV[] _values;

    public Dictionary<TK, TV> Dict;

    public void OnAfterDeserialize() {
      var c = this._keys.Length;
      this.Dict = new Dictionary<TK, TV>(capacity : c);
      for (var i = 0; i < c; i++) this.Dict[key : this._keys[i]] = this._values[i];
      this._keys = null;
      this._values = null;
    }

    public void OnBeforeSerialize() {
      var c = this.Dict.Count;
      this._keys = new TK[c];
      this._values = new TV[c];
      var i = 0;
      using (var e = this.Dict.GetEnumerator()) {
        while (e.MoveNext()) {
          var kvp = e.Current;
          this._keys[i] = kvp.Key;
          this._values[i] = kvp.Value;
          i++;
        }
      }
    }

    public static T New<T>()
      where T : SerializableDictionary<TK, TV>, new() {
      var result = new T {
                           Dict = new Dictionary<TK, TV>()
                         };
      return result;
    }
  }
}
