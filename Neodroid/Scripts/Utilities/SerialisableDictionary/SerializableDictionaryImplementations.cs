using UnityEngine;

namespace Neodroid.Utilities.SerialisableDictionary {
  [System.Serializable]
  public class StringIntDictionary : SerializableDictionary<string, int> {

  }

  [System.Serializable]
  public class GameObjectFloatDictionary : SerializableDictionary<GameObject, float> {

  }

  [System.Serializable]
  public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> {

  }
}
