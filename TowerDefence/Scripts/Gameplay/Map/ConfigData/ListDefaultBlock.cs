using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [Serializable]
    public class ListDefaultBlock
    {
        [SerializeField] private List<DefaultBrick> _defaultBricks;

        private Dictionary<DefaultBrick.Types, DefaultBrick> _dictionary;
        
        public Brick GetOrNull(DefaultBrick.Types typeTarget)
        {
            if (_dictionary == null)
                UpdateDictionary();
            _dictionary.TryGetValue(typeTarget, out var result);
            if (result != null)
                return result.Brick;
            return null;
        }

        private void UpdateDictionary()
        {
            _dictionary = new Dictionary<DefaultBrick.Types, DefaultBrick>();
            foreach (var defaultBrick in _defaultBricks)
            {
                if(_dictionary.ContainsKey(defaultBrick.Type))
                    continue;
                _dictionary.Add(defaultBrick.Type, defaultBrick);
            }
        }
    }
}