using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    public static class ExtensionVector
    {
        public static Vector3 ToVector3XZ(this Vector2Int vector2Int)
        {
            return new Vector3(vector2Int.x, 0, vector2Int.y);
        }
        
        public static Vector3 ToVector3XZ(this Vector2 vector2Int)
        {
            return new Vector3(vector2Int.x, 0, vector2Int.y);
        }

        public static T Random<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];
    }
}