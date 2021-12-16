﻿using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.Saves;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Framework
{
    /// <summary>
    /// Block class is the meat of the world class. 
    /// The world contains sectors, which contain chunks, 
    /// which contains blocks. The blocks can be edited by 
    /// changing their values. Blocks can be stored in an 
    /// external file using a multidimensional byte array, 
    /// generated by the sector class.
    /// </summary>
    public class Block : IBlock
    {
        /// <inheritdoc />
        public Guid guid { get; set; }

        /// <inheritdoc />
        public byte[] content
        {
            get
            {
                _saveData.PrepareToSeralize();
                return IO.ObjectToByteArray(_saveData);
            }
        }

        private SaveDataBlock _saveData;

        public bool ContentIsNull() => _saveData == null;

        /// <inheritdoc />
        public IWorld world { get; set; }

        /// <inheritdoc />
        public Point worldPosition { get; set; }

        /// <inheritdoc />
        public IChunk chunk { get; set; }

        /// <inheritdoc />
        public ISector sector { get; set; }

        /// <summary>
        /// The block constructor requires the block's world position and the parent chunk object.
        /// </summary>
        /// <param name="worldPosition">The block's world position.</param>
        /// <param name="chunk">The block's parent chunk.</param>
        internal Block(Point worldPosition, IChunk chunk)
        {
            this.chunk = chunk;
            this.sector = chunk.sector;
            this.world = chunk.world;
            this.worldPosition = SnapToGrid.snap(worldPosition, 1);
            guid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public SaveDataBlock GetContent()
        {
            return _saveData;
        }

        /// <inheritdoc />
        public void SetContent(SaveDataBlock obj)
        {
            _saveData = obj;
        }

        /// <inheritdoc />
        public void Destroy() {
            chunk.blocks[chunk.GetIndexByPoint(worldPosition)] = null; // Dispose block
            SetContent(null);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            object objContent = GetContent();
            return "Content: " + ((objContent == null) ? "NULL" : objContent) + ", "
             + "worldPosition: " + worldPosition;
        }

    }
}
