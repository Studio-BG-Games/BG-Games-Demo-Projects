using System;
using System.Collections.Generic;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.Saves;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces
{
    /// <summary>
    /// The World class used for creating worlds.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// This object's guid.
        /// </summary>
        Guid guid { get; set; }

        /// <summary>
        /// The chunk size.
        /// </summary>
        int chunkSize { get; set; }

        /// <summary>
        /// The sector size.
        /// </summary>
        int sectorSize { get; set; }

        /// <summary>
        /// The world name.
        /// </summary>
        string name { get; set; }

        /// <summary>
        /// The current loaded sectors. Given the fact that a world 
        /// can be infinite, we only load sectors if needed.
        /// </summary>
        Dictionary<string, ISector> sectors { get; set; }

        /// <summary>
        /// Gets the block given a point in space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>IBlock</returns>
        IBlock GetBlock(Point point);

        /// <summary>
        /// Set a block by point.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <param name="content">The content at which you would like to write the block with.</param>
        /// <returns>IBlock</returns>
        IBlock SetBlock(Point point, SaveDataBlock  content);

        /// <summary>
        /// Erases the current block's content.
        /// </summary>
        /// <param name="point">Requires the point in world space that you would like to erase.</param>
        void ClearBlock(Point point);

        /// <summary>
        /// Erases the current chunk's content.
        /// </summary>
        /// <param name="point">Requires the point in world space that you would like to erase.</param>
        void ClearChunk(Point point);

        /// <summary>
        /// Erases the current sector's content.
        /// </summary>
        /// <param name="point">Requires the point in world space that you would like to erase.</param>
        void ClearSector(Point point);

        /// <summary>
        /// Calls the destructor.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Is the current point in space occupied by another block?
        /// </summary>
        /// <param name="point">Provided with a point in world space.</param>
        /// <returns>bool</returns>
        bool IsOccupied(Point point);

        /// <summary>
        /// Retrieves sector by point.
        /// If sector does not exist, it will create one
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>ISector</returns>
        ISector GetSector(Point point);

        /// <summary>
        /// Retrieves the chunk given a point in world space.
        /// </summary>
        /// <param name="point">Provided with a point in world space.</param>
        /// <returns>IChunk</returns>
        IChunk GetChunk(Point point);

        /// <summary>
        /// Saves the current world to a given directory.
        /// </summary>
        /// <param name="url">The directory at which you would like to save your world to. Default is World/WorldName.</param>
        /// <returns>string</returns>
        string SaveWorld(string url = "");

        /// <summary>
        /// Saves the current world to a given directory.
        /// </summary>
        /// <param name="point">Requires a point in world space.</param>
        /// <param name="url">The directory at which you would like to save your world to. Default is World/WorldName.</param>
        /// <returns>string</returns>
        string SaveSector(Point point, string url = "");

        /// <summary>
        /// Loads a sector from a file and returns  it.
        /// </summary>
        /// <param name="point">Requires a point in world space.</param>
        /// <param name="url">The url where the file is loaded.  </param>
        /// <returns>Sector</returns>
        ISector LoadSector(Point point, string url = "");

        /// <summary>
        /// Displays the world details.
        /// </summary>
        /// <returns>string</returns>
        string ToString();
    }
}
