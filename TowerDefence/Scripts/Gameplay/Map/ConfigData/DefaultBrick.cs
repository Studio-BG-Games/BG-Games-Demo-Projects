using System;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [Serializable]
    public class DefaultBrick
    {
        public Brick Brick => _bricktemplate;
        public Types Type => _type;

        public DefaultBrick(Types typeBrick) => _type = typeBrick;
        
        [SerializeField] private Types _type;
        [SerializeField] private Brick _bricktemplate;
        
        public enum Types
        {
            Road, Water, underground
        }
    }
}