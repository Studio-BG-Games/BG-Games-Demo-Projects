using System;
using System.Collections.Generic;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Gameplay.Map.Generator;
using Gameplay.Units;
using Gameplay.Waves;
using UnityEngine;

namespace Infrastructure.SceneStates
{
    [System.Serializable]
    public class InitSceneData
    {
        public GeneratorMap GeneratorMap => _generator;
        public List<Build> Buildses => _builds;
        public List<UnitCount> Units => _units;

        public Level Level => _level;
        

        [SerializeField] private SimpelGenerator _generator;
        [SerializeField] private List<Build> _builds;
        [SerializeField] private List<UnitCount> _units = new List<UnitCount>();
        [SerializeField] private Level _level;

        public void OnValidate() => Units.ForEach(x => x.OnValidate());
        
        [System.Serializable]
        public class UnitCount
        {
            public Unit Unit => _unit;

            public int Count => _count;
            
            [SerializeField]private Unit _unit;
            [SerializeField][Min(1)]private int _count;

            public UnitCount(Unit unit, int count)
            {
                if (count <= 0)
                    throw new Exception("count is or less 0, fix it");
                _unit = unit;
                _count = count;
            }


            public void OnValidate()
            {
                if (_unit.MainDates.GetOrNull<Team>().Type != Team.Typ.Player)
                    _unit = null;
            }
        }
    }
}