/*
 * The purpose of this class is to extend the World framework to be easily
 * accessible by the unityengine.
 * 
 * Author: Corey St-Jacques
*/

using System;
using System.Collections.Generic;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.Saves;
using UnityEngine;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework
{
    /// <summary>
    /// The World class used for creating worlds in the unity engine.
    /// </summary>
    public class UnityWorld : World, IWorld
    {

        /// <summary>
        /// The parent transform object.
        /// </summary>
        public GameObject gameObject { get; private set; }

        /// <summary>
        /// Unity world constructor.
        /// </summary>
        /// <param name="parent">Requires the object's parent gameObject.</param>
        /// <param name="name">Requires the world's name.</param>
        /// <param name="chunkSize">Requires the chunk size.</param>
        /// <param name="sectorSize">Requires the sector size.</param>
        public UnityWorld(Transform parent, string name = "myWorld",
            int chunkSize = 8, int sectorSize = 4)
            : base(name, chunkSize, sectorSize)
        {
            gameObject = parent.gameObject;
        }

        /// <summary>
        /// Retrieves UnitySector by Vector3.
        /// If sector does not exist, it will create one
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>UnitySector</returns>
        public UnitySector GetSector(Vector3 point)
        {
            return GetSector(point.Point()) as UnitySector;
        }

        /// <summary>
        /// Retrieves a chunk by a point in world space.
        /// If sector does not exist, it will create one
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>UnityChunk</returns>
        public UnityChunk GetChunk(Vector3 point)
        {
            return GetChunk(point.Point()) as UnityChunk;
        }

        /// <summary>
        /// Gets the block given a point in space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>UnityBlock</returns>
        public UnityBlock GetBlock(Vector3 point)
        {
            return GetBlock(point.Point()) as UnityBlock;
        }

        /// <summary>
        /// Set a block by point.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <param name="content">The content at which you would like to write the block with.</param>
        /// <returns>UnityBlock</returns>
        public UnityBlock SetBlock(Vector3 point, SaveDataBlock content)
        {
            return SetBlock(point.Point(), content) as UnityBlock;

        }

        /// <summary>
        /// Erases the current block's content.
        /// </summary>
        /// <param name="point">Requires the point in world space that you would like to erase.</param>
        public void ClearBlock(Vector3 point)
        {
            GetBlock(point).Destroy();
        }

        /// <summary>
        /// Is the current point in space occupied by another block?
        /// </summary>
        /// <param name="point">Provided with a point in world space.</param>
        /// <returns>bool</returns>
        public bool IsOccupied(Vector3 point)
        {
            return IsOccupied(point.Point());
        }

        /// <inheritdoc />
        public new void ClearSectors()
        {
            UnitySector[] tmpSectors = new UnitySector[sectors.Values.Count];
            sectors.Values.CopyTo(tmpSectors, 0);
            foreach (UnitySector sector in tmpSectors)
                sector.Destroy();

            sectors = new Dictionary<string, ISector>();
        }

        /// <summary>
        /// The Destructor.
        /// </summary>
        public new void Destroy()
        {
            sectors = new Dictionary<string, ISector>();
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// Loads a sector from a file and returns it.
        /// </summary>
        /// <param name="point">Requires sector's matrix point.</param>
        /// <returns>ISector</returns>
        public UnitySector LoadSector(Vector3 point, string url = "")
        {
            if (url.Equals(""))
                url = Application.dataPath;

            return LoadSector(point.Point(), url) as UnitySector;
        }


        /// <summary>
        /// Saves the current world to a given directory.
        /// </summary>
        /// <param name="point">Requires a point in world space.</param>
        /// <param name="url">The directory at which you would like to save your world to. Default is World/WorldName.</param>
        /// <returns>string</returns>
        public string SaveSector(Vector3 point, string url = "")
        {
            if (url.Equals(""))
                url = Application.dataPath;

            return SaveSector(point.Point(), url = "");
        }

        /// <summary>
        /// Saves the current world to a given directory.
        /// </summary>
        /// <param name="url">The directory at which you would like to save your world to. Default is World/WorldName.</param>
        /// <returns>string</returns>
        public new string SaveWorld(string url = "")
        {
            if (url.Equals(""))
                url = Application.dataPath;

            return base.SaveWorld(url);
        }
    }

    /// <summary>
    /// Extension methods for the utilities namespace.
    /// </summary>
    internal static class MyExtensions
    {
        /// <summary>
        /// Extension method to retrieve a Vector3 from a Point object.
        /// </summary>
        /// <param name="point">Returns Vector3 of this current point.</param>
        /// <returns>Vector3</returns>
        public static Vector3 Vector3(this Point point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }

        /// <summary>
        /// Snaps the current point to grid.
        /// </summary>
        /// <param name="point">This current point.</param>
        /// <param name="value">The grid value to snap to.</param>
        /// <returns>Vector3</returns>
        public static Vector3 Snap(this Vector3 point, int value = 1)
        {
            return point.Point().Snap(value).Vector3();
        }

        /// <summary>
        /// Extension method to retrieve a Point from a Vector3 object.
        /// </summary>
        /// <param name="point">The current vector3.</param>
        /// <returns>Point</returns>
        public static Point Point(this Vector3 point)
        {
            return new Point(point.x, point.y, point.z);
        }
    }
}