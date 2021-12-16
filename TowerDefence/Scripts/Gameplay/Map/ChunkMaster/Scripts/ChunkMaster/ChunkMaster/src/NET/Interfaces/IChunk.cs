using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces
{
    /// <summary>
    /// The chunk class allows the world to store blocks inside chunks.
    /// </summary>
    public interface IChunk
    {
        /// <summary>
        /// This object's guid.
        /// </summary>
        Guid guid { get; set; }

        /// <summary>
        /// The current world object.
        /// </summary>
        IWorld world { get; set; }

        /// <summary>
        /// Returns true if the chunk is occupied with blocks.
        /// </summary>
        bool isOccupied { get; set; }

        /// <summary>
        /// The blocks associated to the current chunk.
        /// </summary>
        IBlock[] blocks { get; set; }

        /// <summary> 
        /// The chunk's position in world space.
        /// </summary>
        Point worldPosition { get; set; }

        /// <summary>
        /// The chunk's parent.
        /// </summary>
        ISector sector { get; set; }

        /// <summary>
        /// Retrieve the matrix relative to the parent sector.
        /// </summary>
        Point chunkMatrixPoint { get; }


        /// <summary>
        /// Clears all blocks in this chunk.
        /// </summary>
        void ClearBlocks();

        /// <summary>
        /// Calls the destructor.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Get block by a point in world space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>IBlock</returns>
        IBlock GetBlock(Point point);

        /// <summary>
        /// Get a block by index.
        /// </summary>
        /// <param name="index">The block's index.</param>
        /// <returns>IBlock</returns>
        IBlock GetBlockByIndex(int index);

        /// <summary>
        /// Gets a point point by index.
        /// </summary>
        /// <param name="index">The index to convert to world point.</param>
        /// <returns>Point</returns>
        Point GetPointByIndex(int index);

        /// <summary>
        /// Gets the index by a point in world space.
        /// </summary>
        /// <param name="worldPoint">The point in world space.</param>
        /// <returns>int</returns>
        int GetIndexByPoint(Point worldPoint);

        /// <summary>
        /// Displays the block's details.
        /// </summary>
        /// <returns>string</returns>
        string ToString();

    }
}
