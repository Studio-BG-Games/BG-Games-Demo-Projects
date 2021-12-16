using UnityEngine;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Factory
{
    /// <summary>
    /// This class allows developers to create transforms with ease of access.
    /// </summary>
    internal static class TransformFactory
    {
        /// <summary>
        /// This method retrieves a new transform object and adds it to your game scene.
        /// </summary>
        /// <param name="name">The transform's name.</param>
        /// <param name="worldPosition">The world postion of the transform.</param>
        /// <param name="rotation">The transform's rotation.</param>
        /// <param name="scale">The transform's scale.</param>
        /// <param name="parent">The transform's parent.</param>
        /// <returns>Transform</returns>
        public static Transform CreateTransform(string name,
            Vector3 worldPosition, Quaternion rotation, Vector3 scale, Transform parent)
        {
            GameObject obj = new GameObject();
            Transform tmp = obj.transform;
            tmp.name = name;
            tmp.position = worldPosition;
            tmp.rotation = rotation;
            tmp.localScale = scale;
            tmp.SetParent(parent);
            return tmp;
        }
    }
}
