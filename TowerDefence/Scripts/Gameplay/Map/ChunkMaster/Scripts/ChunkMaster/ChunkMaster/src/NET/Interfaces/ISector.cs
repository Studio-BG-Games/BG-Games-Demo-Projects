using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces
{
    /// <summary>
    /// The sector class manipulates the chunks inside the current sector.
    /// </summary>
    public interface ISector
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
        /// Returns true if the sector is occupied with blocks.
        /// </summary>
        bool isOccupied { get; set; }

        /// <summary>
        /// The chunks associated to the current sector.
        /// </summary>
        IChunk[] chunks { get; set; }

        /// <summary>
        /// The sector's world position.
        /// </summary>
        Point worldPosition { get; set; }

        /// <summary>
        /// Get the sector matrix point.
        /// </summary>
        Point sectorMatrixPoint { get; }


        /// <summary>
        /// Clears all chunks in this sector.
        /// </summary>
        void ClearChunks();

        /// <summary>
        /// Calls the destructor.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Get a chunk by a point in world space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>IChunk</returns>
        IChunk GetChunk(Point point);

        /// <summary>
        /// Get a chunk by index.
        /// </summary>
        /// <param name="index">The chunk's index.</param>
        /// <returns>IChunk</returns>
        IChunk GetChunkByIndex(int index);

        /// <summary>
        /// Gets the index by a point in world space.
        /// </summary>
        /// <param name="worldPoint">Get the index by a point in world space.</param>
        /// <returns>int</returns>
        int GetIndexByPoint(Point worldPoint);

        /// <summary>
        /// Gets a chunk point by index.
        /// </summary>
        /// <param name="index">The index to convert to world point.</param>
        /// <returns>Point</returns>
        Point GetPointByIndex(int index);

        /// <summary>
        /// Retrieves a multidimensional block array of all blocks in this sector.
        /// </summary>
        /// <returns>byte[][]</returns>
        byte[][] GetBlocksArray();

        /// <summary>
        /// Sets all blocks in this sector by block array.
        /// </summary>
        /// <param name="blocksArray">Requires a mutlidimensional byte array that you would like to copy.</param>
        void SetBlocksArray(byte[][] blocksArray);

        /// <summary>
        /// Displays the sector's details.
        /// </summary>
        /// <returns>string</returns>
        string ToString();
    }
}
