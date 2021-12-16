using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Factory;
using UnityEngine;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework
{
    /// <summary>
    /// The sector class manipulates the chunks inside the current sector.
    /// </summary>
    public class UnitySector : Sector, ISector
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
        /// <param name="world">Requires the current world that the sector is inside.</param>
        public UnitySector(Vector3 worldPosition, IWorld world)
            : base(worldPosition.Point(), world)
        {
            gameObject = TransformFactory.CreateTransform(
                worldPosition.Snap(world.chunkSize * world.sectorSize).ToString(),
                worldPosition,
                Quaternion.Euler(Vector3.zero),
                Vector3.one,
                ((UnityWorld)world).gameObject.transform
                ).gameObject;
        }

        /// <summary>
        /// Get a chunk by a point in world space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>IChunk</returns>
        public IChunk GetChunk(Vector3 point)
        {
            int index = GetIndexByPoint(point.Point());
            return GetChunkByIndex(index);
        }

        /// <summary>
        /// The Destructor.
        /// </summary>
        public new void Destroy()
        {
            world.sectors.Remove(sectorMatrixPoint.ToString()); // Dispose sector
            isOccupied = false;
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// Displays the current sector's details.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return base.ToString() + " GameObject: " + gameObject.GetInstanceID();
        }

    }
}
