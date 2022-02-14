using UnityEditor;

namespace Baby_vs_Aliens
{
    [CustomPropertyDrawer(typeof(ArenaObjectPrefabDictionary))]
    public class AnySerializableDictionaryPropertyDrawer :
        SerializableDictionaryPropertyDrawer
    { }
}