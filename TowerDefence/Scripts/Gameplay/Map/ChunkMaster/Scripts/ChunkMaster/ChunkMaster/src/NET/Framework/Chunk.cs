using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Factory;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework
{
    /// <summary>
    /// The chunk class allows the world to store blocks inside chunks.
    /// </summary>
    public class Chunk : IChunk
    {
        /// <inheritdoc />
        public Guid guid { get; set; }

        /// <inheritdoc />
        public IWorld world { get; set; }

        /// <inheritdoc />
        public bool isOccupied { get; set; }

        /// <inheritdoc />
        public IBlock[] blocks { get; set; }

        /// <inheritdoc />
        public Point worldPosition { get; set; }

        /// <inheritdoc />
        public ISector sector { get; set; }

        /// <inheritdoc />
        public Point chunkMatrixPoint
        {
            get
            {
                return (worldPosition - sector.worldPosition) / world.chunkSize;
            }
        }

        /// <summary>
        /// The chunk constructor.
        /// </summary>
        /// <param name="worldPosition">The chunk's position in world space.</param>
        /// <param name="sector">The chunk's parent sector.</param>
        internal Chunk(Point worldPosition, ISector sector)
        {
            this.sector = sector;
            this.world = sector.world;
            this.worldPosition = SnapToGrid.snap(worldPosition, world.chunkSize);
            blocks = new IBlock[world.chunkSize * world.chunkSize * world.chunkSize];

            guid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public void ClearBlocks()
        {
            blocks = new IBlock[world.chunkSize * world.chunkSize * world.chunkSize];
        }

        /// <inheritdoc />
        public IBlock GetBlock(Point point)
        {

            int index = GetIndexByPoint(point);
            return GetBlockByIndex(index);
        }

        /// <inheritdoc />
        public void Destroy() {
            sector.chunks[sector.GetIndexByPoint(worldPosition)] = null; // Dispose chunk
            isOccupied = false;
        }

        /// <inheritdoc />
        public IBlock GetBlockByIndex(int index)
        {
            IBlock block;
            Point point = GetPointByIndex(index);

            block = blocks[index];

            if (block == null)
            {
                block = ChunkMasterFactory.CreateBlock(point, this);
                blocks[index] = block;
            }

            return block;
        }

        /// <inheritdoc />
        public Point GetPointByIndex(int index)
        {
            return Point.GetPointByIndex(index, world.chunkSize) + worldPosition;
        }


        /// <inheritdoc />
        public int GetIndexByPoint(Point worldPoint)
        {
            Point localPosition = worldPoint - worldPosition;
            return Point.GetIndexByPoint(localPosition, world.chunkSize);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Chunk " 
                + "worldPosition: " + worldPosition + ", "
                + "chunkMatrixPoint: " + chunkMatrixPoint + ", "
                + "chunkSize: " + world.chunkSize + ", "
                + "blocks: " + blocks.Length;
        }
    }
}
