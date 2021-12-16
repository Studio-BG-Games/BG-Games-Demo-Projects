using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Factory;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.Saves;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework
{
    /// <summary>
    /// The sector class manipulates the chunks inside the current sector.
    /// </summary>
    public class Sector : ISector
    {
        /// <inheritdoc />
        public Guid guid { get; set; }

        /// <inheritdoc />
        public IWorld world { get; set; }

        /// <inheritdoc />
        public IChunk[] chunks { get; set; }

        /// <inheritdoc />
        public Point worldPosition { get; set; }

        /// <inheritdoc />
        public bool isOccupied { get; set; }

        /// <inheritdoc />
        public Point sectorMatrixPoint {
            get
            {
                return worldPosition / world.sectorSize / world.chunkSize;
            }
        }

        /// <summary>
        /// The sector constructor.
        /// </summary>
        /// <param name="worldPosition">The sector's position in world space.</param>
        /// <param name="world">The sector's parent world.</param>
        internal protected Sector(Point worldPosition, IWorld world)
        {
            this.world = world;
            this.worldPosition = SnapToGrid.snap(worldPosition, world.sectorSize * world.chunkSize);
            chunks = new IChunk[world.sectorSize * world.sectorSize * world.sectorSize];

            guid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public void ClearChunks()
        {
            chunks = new IChunk[world.sectorSize * world.sectorSize * world.sectorSize];
        }

        /// <inheritdoc />
        public void Destroy() {
            world.sectors.Remove(sectorMatrixPoint.ToString()); // Dispose sector
            isOccupied = false;
        }

        /// <inheritdoc />
        public IChunk GetChunk(Point point)
        {
            int index = GetIndexByPoint(point);
            return GetChunkByIndex(index);
        }

        /// <inheritdoc />
        public IChunk GetChunkByIndex(int index)
        {
            IChunk chunk;
            Point point = GetPointByIndex(index);

            chunk = chunks[index];

            if (chunk == null)
            {
                chunk = ChunkMasterFactory.CreateChunk(point, this);
                chunks[index] = chunk;
            }
            return chunk;
        }

        /// <inheritdoc />
        public int  GetIndexByPoint(Point worldPoint)
        {
            Point localPosition = SnapToGrid.snap(worldPoint, world.chunkSize) - worldPosition;
            return Point.GetIndexByPoint(localPosition / world.chunkSize, world.sectorSize);
        }

        /// <inheritdoc />
        public Point GetPointByIndex(int index)
        {
            return Point.GetPointByIndex(index, world.sectorSize) * world.chunkSize + worldPosition;
        }

        /// <inheritdoc />
        public byte[][] GetBlocksArray()
        {
            IBlock tmpBlock;
            IChunk tmpChunk;
            byte[][] blocksArray = new byte[(world.chunkSize * world.chunkSize * world.chunkSize) * world.sectorSize * world.sectorSize * world.sectorSize][];

            // Only iterate through block data is the sector is occupied
            if (isOccupied) // If sector has blocks
            {
                // Iterate through all blocks inside all chunks.
                for (int chunkIndex = 0; chunkIndex < world.sectorSize * world.sectorSize * world.sectorSize; chunkIndex++)
                {
                    tmpChunk = chunks[chunkIndex];
                    if (tmpChunk != null)
                    {
                        if (tmpChunk.isOccupied) // If chunk has blocks
                        {
                            for (int blockIndex = 0; blockIndex < world.chunkSize * world.chunkSize * world.chunkSize; blockIndex++)
                            {

                                tmpBlock = tmpChunk.blocks[blockIndex];

                                if (tmpBlock != null)
                                {
                                    blocksArray[chunkIndex *
                                        (world.chunkSize * world.chunkSize * world.chunkSize)
                                        + blockIndex] = tmpBlock.content;
                                }

                            }
                        }
                    }
                }
            }
            return blocksArray;
        }

        /// <inheritdoc />
        public void SetBlocksArray(byte[][] blocksArray)
        {
            byte[] inputArray;
            IChunk tmpChunk;
            IBlock tmpBlock;
            if (blocksArray != null) // If array is not null Continue
            {
                for (int chunkIndex = 0; chunkIndex < world.sectorSize * world.sectorSize * world.sectorSize; chunkIndex++)
                {
                    
                    for (int blockIndex = 0; blockIndex < world.chunkSize * world.chunkSize * world.chunkSize; blockIndex++)
                    {
                        inputArray = blocksArray[chunkIndex *
                                    (world.chunkSize * world.chunkSize * world.chunkSize)
                                    + blockIndex];
                        if (inputArray != null)
                        {
                            tmpChunk = GetChunkByIndex(chunkIndex);
                            tmpBlock = tmpChunk.GetBlock(tmpChunk.GetPointByIndex(blockIndex));
                            tmpBlock.SetContent((SaveDataBlock)IO.ByteArrayToObject(inputArray));
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Sector "
                + "worldPosition: " + worldPosition + ", "
                + "sectorMatrixPoint: " + sectorMatrixPoint + ", "
                + "sectorSize: " + world.sectorSize + ", "
                + "chunks: " + chunks.Length;
        }

    }
}
