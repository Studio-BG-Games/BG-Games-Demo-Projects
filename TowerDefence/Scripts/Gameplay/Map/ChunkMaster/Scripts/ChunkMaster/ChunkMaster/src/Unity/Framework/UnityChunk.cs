using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Factory;
using UnityEngine;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework
{
    /// <summary>
    /// The chunk class allows the world to store blocks inside chunks.
    /// </summary>
    public class UnityChunk : Chunk, IChunk
    {

        /// <summary>
        /// The associated gameObject.
        /// </summary>
        public GameObject gameObject { get; private set; }

        /// <summary>
        /// Returns the current reference as a vector3 relative to the world space.
        /// </summary>
        public new Vector3 worldPosition
        {
            get
            {
                return base.worldPosition.Vector3();
            }
        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="worldPosition">Requires the sector's world point.</param>
        /// <param name="sector">Requires the chunk's sector.</param>
        public UnityChunk(Vector3 worldPosition, ISector sector)
            : base(worldPosition.Point(), sector)
        {
            gameObject = TransformFactory.CreateTransform(worldPosition.Snap(world.chunkSize).ToString(),
                worldPosition,
                Quaternion.Euler(Vector3.zero),
                Vector3.one,
                ((UnitySector)sector).gameObject.transform
                ).gameObject;

        }

        /// <summary>
        /// Get block by a point in world space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>IBlock</returns>
        public IBlock GetBlock(Vector3 point)
        {
            int index = GetIndexByPoint(point.Point());
            return GetBlockByIndex(index);
        }

        /// <summary>
        /// The Destructor.
        /// </summary>
        public new void Destroy()
        {
            isOccupied = false;
            sector.chunks[sector.GetIndexByPoint(worldPosition.Point())] = null; // Dispose chunk
            GameObject.Destroy(gameObject);
        }


        /// <summary>
        /// Displays the current chunk's details.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
