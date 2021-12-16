using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Builds;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.Map.Saves
{
    [Serializable]
    public class SaveDataBlock
    {
        [NonSerialized] private List<Brick> _bricks;
        [NonSerialized] private HabObject _hab;

        public HabObject Hab => _hab;

        public void PrepareToSeralize()
        {
            
        }
        
        public void GoOverBrick(Action<Brick> callback) => _bricks.ForEach(x=>callback?.Invoke(x));

        public bool TryAddHabObject(HabObject build)
        {
            if (_hab == null && build!=null)
            {
                _hab = build;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveHabObject() => _hab = null;

        public void AddBrick(Brick brick)
        {
            if(_bricks==null)
                _bricks=new List<Brick>();
            if(!_bricks.Contains(brick))
                _bricks.Add(brick);
        }


        public Brick GetUpestBrick()
        {
            return _bricks.OrderByDescending(x => x.transform.position.y).First();
        }
    }
}