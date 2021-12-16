using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Factory
{
    /// <summary>
    /// The chunkmaster factory creates all necessary objects.
    /// </summary>
    internal static class ChunkMasterFactory
    {
        /// <summary>
        /// Creates a block of interface IBlock.
        /// </summary>
        /// <param name="worldPosition">The block in world space.</param>
        /// <param name="chunk">The block's associated parent.</param>
        /// <returns>IBlock</returns>
        public static IBlock CreateBlock(Point worldPosition, IChunk chunk)
        {
            IBlock block;

            if (chunk.GetType() == typeof(UnityChunk))
            {
                block = new UnityBlock(worldPosition.Vector3(), chunk);
            }
            else
            {
                
                block = new Block(worldPosition, chunk);
            }

            block.chunk.sector.isOccupied = true;   // Sector is now occupied.
            block.chunk.isOccupied = true;          // Chunk is now occupied.
            return block;
        }

        /// <summary>
        /// Creates a chunk of interface IChunk.
        /// </summary>
        /// <param name="worldPosition">The world position of this chunk.</param>
        /// <param name="sector">The chunk's associated sector.</param>
        /// <returns>IChunk</returns>
        public static IChunk CreateChunk(Point worldPosition, ISector sector)
        {
            if (sector.GetType() == typeof(UnitySector))
                return new UnityChunk(worldPosition.Vector3(), sector);
            else
                return new Chunk(worldPosition, sector);
        }

        /// <summary>
        /// Creates a sector of interface ISector.
        /// </summary>
        /// <param name="worldPosition">The sector's world position.</param>
        /// <param name="world">The world associated to this sector.</param>
        /// <returns>ISector</returns>
        public static ISector CreateSector(Point worldPosition, IWorld world)
        {
            if (world.GetType() == typeof(UnityWorld))
                return new UnitySector(worldPosition.Vector3(), world);
            else
                return new Sector(worldPosition, world);
        }
    }
}
