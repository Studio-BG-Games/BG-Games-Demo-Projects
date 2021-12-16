using System;
using System.Collections.Generic;
using Gameplay.Map;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds.Data
{
    [DisallowMultipleComponent]
    public class BanListBrickForBuild : BuildProperty
    {
        [SerializeField] private List<Brick> _bricks;

        public List<Brick> Bricks => _bricks;
        
        [NonSerialized] private string[] _ids;

        public void GoOverBricks(Action<Brick> callback) => _bricks.ForEach(x=>callback?.Invoke(x));

        public string[] GetAllId()
        {
            if (_ids != null)
                return _ids;
            _ids = new string[_bricks.Count];
            for (int i = 0; i < _ids.Length; i++)
            {
                _ids[i] = _bricks[i].ID;
            }

            return _ids;
        }
    }
}