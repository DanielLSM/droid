using System;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.SerialisableDictionary {
  [Serializable] public class StringIntDictionary : SerializableDictionary<string, int> { }

  [Serializable] public class GameObjectFloatDictionary : SerializableDictionary<GameObject, float> { }

  [Serializable] public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> { }
}