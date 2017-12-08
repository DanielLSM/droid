using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

namespace Neodroid.Utilities.SerialisableDictionary
{
    [UnityEditor.CustomPropertyDrawer(typeof(StringIntDictionary))]
    public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int>
    {
        protected override SerializableKeyValueTemplate<string, int> GetTemplate()
        {
            return GetGenericTemplate<SerializableStringIntTemplate>();
        }
    }

    internal class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int>
    {

    }


    [UnityEditor.CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
    public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float>
    {
        protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate()
        {
            return GetGenericTemplate<SerializableGameObjectFloatTemplate>();
        }
    }

    internal class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float>
    {

    }


    [UnityEditor.CustomPropertyDrawer(typeof(StringGameObjectDictionary))]
    public class StringGameObjectDictionaryDrawer : SerializableDictionaryDrawer<string, GameObject>
    {
        protected override SerializableKeyValueTemplate<string, GameObject> GetTemplate()
        {
            return GetGenericTemplate<SerializableStringGameObjectTemplate>();
        }
    }

    internal class SerializableStringGameObjectTemplate : SerializableKeyValueTemplate<string, GameObject>
    {

    }
}
#endif