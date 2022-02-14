using System;
using UnityEngine;

namespace Baby_vs_Aliens.LevelEditor
{
    [Serializable]
    public struct TileMaterial
    {
        public ArenaObjectType Type;
        public Material Material;
    }
}