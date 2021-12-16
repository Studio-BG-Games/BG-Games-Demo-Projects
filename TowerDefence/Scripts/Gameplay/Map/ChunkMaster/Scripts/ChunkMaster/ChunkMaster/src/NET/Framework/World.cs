using System;
using System.Collections.Generic;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Factory;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.Saves;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework
{
    /// <summary>
    /// The World class used for creating worlds.
    /// <code>
    /// // Create a world object.
    /// World myWorld = new World("myWorld");
    /// 
    /// // Get a point in world space.
    /// Point worldPoint = new Point(3, -1, 2);
    /// 
    /// // Get a block in world space.
    /// Block block = myWorld.GetBlock(worldPoint) as Block;
    /// 
    /// // Edit a block in world space.
    /// myWorld.SetBlock(worldPoint, 0); // Assign the current point with a value of 0.
    /// </code>
    /// </summary>
    public class World : IWorld
    {
        /// <inheritdoc />
        public Guid guid { get; set; }

        /// <inheritdoc />
        public int chunkSize { get; set; }

        /// <inheritdoc />
        public int sectorSize { get; set; }

        /// <inheritdoc />
        public string name { get; set; }

        /// <inheritdoc />
        public Dictionary<string, ISector> sectors { get; set; }

        /// <summary>
        /// The world constructor.
        /// </summary>
        /// <param name="name">The world's name. Used for file writing.</param>
        /// <param name="chunkSize">The world's chunk size.</param>
        /// <param name="sectorSize">The world's sector size.</param>
        public World(string name = "myWorld", 
            int chunkSize = 8, int sectorSize = 4)
        {
            this.name = name;
            this.chunkSize = chunkSize;
            this.sectorSize = sectorSize;
            sectors = new Dictionary<string, ISector>();

            guid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public IBlock GetBlock(Point point)
        {
            IBlock block;
            block = GetChunk(point).GetBlock(point);
            return block;
        }

        /// <inheritdoc />
        public IBlock SetBlock(Point point, SaveDataBlock content)
        {
            IBlock block;
            block = GetBlock(point);
            block.SetContent(content);
            return block;
        }

        /// <inheritdoc />
        public void ClearBlock(Point point)
        {
            IBlock block;
            block = GetBlock(point);
            block.Destroy();
        }

        /// <inheritdoc />
        public void ClearChunk(Point point)
        {
            IChunk chunk;
            chunk = GetChunk(point);
            chunk.Destroy();
        }

        /// <inheritdoc />
        public void ClearSector(Point point)
        {
            ISector sector;
            sector = GetSector(point);
            sector.Destroy();
        }

        /// <inheritdoc />
        public void ClearSectors()
        {
            Sector[] tmpSectors = new Sector[sectors.Values.Count];
            sectors.Values.CopyTo(tmpSectors, 0);
            foreach (Sector sector in tmpSectors)
                sector.Destroy();
        }

        /// <inheritdoc />
        public bool IsOccupied(Point point)
        {
            IBlock block;
            block = GetBlock(point);
            return block.ContentIsNull() ? false : true; 
        }

        /// <inheritdoc />
        public ISector GetSector(Point point)
        {
            Point snappedPoint = SnapToGrid.snap(point, sectorSize * chunkSize);
            Point matrixPoint = snappedPoint / sectorSize / chunkSize;
            string key = matrixPoint.ToString();
            ISector sector;
            if (!sectors.TryGetValue(key, out sector))
            {
                sector = ChunkMasterFactory.CreateSector(point, this);
                sectors.Add(key, sector);
            }
            return sector;
        }

        /// <inheritdoc />
        public IChunk GetChunk(Point point)
        {
            return GetSector(point).GetChunk(point);
        }

        /// <inheritdoc />
        public string SaveWorld(string url = "")
        {
            string fullPath;

            fullPath = IO.OpenDirectory(url + "/" + name);

            foreach (string key in sectors.Keys)
                IO.SaveSector(sectors[key], fullPath);

            return fullPath;
        }

        /// <inheritdoc />
        public string SaveSector(Point point, string url = "")
        {
            string fullPath;

            fullPath = IO.OpenDirectory(url + "/" + name);

            ISector sector = GetSector(point);

            IO.SaveSector(sector, fullPath);

            return fullPath;
        }

        /// <inheritdoc />
        public ISector LoadSector(Point point, string url = "")
        {
            byte[][] blocksArray;
            ISector sector = GetSector(point);
            sector.ClearChunks();

            Point snappedPoint = SnapToGrid.snap(point, sectorSize * chunkSize);
            Point matrixPoint = snappedPoint / sectorSize / chunkSize;
            string key = matrixPoint.ToString();

            blocksArray = (byte[][])IO.ByteArrayToObject( 
                IO.ReadFile(url + "/" + name + "/" + key) 
                );

            sector.SetBlocksArray(blocksArray);
            return sector;
        }

        /// <inheritdoc />
        public void Destroy() {
            sectors = new Dictionary<string, ISector>();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "World "
                + "name: " + name + ", "
                + "sectors: " + sectors.Count + ", "
                + "sectorSize: " + sectorSize + ", "
                + "chunkSize: " + chunkSize;
        }
    }
}
