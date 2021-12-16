using System;
using System.Collections.Generic;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Waves
{
    [System.Serializable]
    public class Wave
    {
        [SerializeField]public List<ElementWave> ElementWaves;
        [Min(0)] public int Award=0;
        [Min(0.1f)] public float CoolDownSpawn=0;
        [Min(1)] public int CountPortal=1;
        [Min(10)] public int MinDistanceToNeksus=10;
        public BossModule Boss;

        public List<string> GetAllId()
        {
            List<string> ids = new List<string>();
            foreach (var elementWave in ElementWaves)
            {
                var id = elementWave.Template.MainDates.GetOrNull<IdContainer>().ID;
                if(!ids.Contains(id))
                    ids.Add(id);
            }
            return ids;
        }

        public void OnValidate(Level level, int index)
        {
            Boss.OnValidate();
            for (var i = 0; i < ElementWaves.Count; i++) ElementWaves[i].OnValidate(level, index, i);
            if(ElementWaves.Count<=0)
                Debug.LogError($"{level.name} на {index} не имеет моснтров");
        }
        
        [Serializable]
        public class BossModule
        {
            public bool IsEnbale => Unit;
            [SerializeField] private Unit Unit;
            [SerializeField] [Range(0, 1f)] private float _chance=0.5f;

            public Unit GetBossOrNull() => Random.Range(0, 1f) < _chance ? Unit : null;

            public void OnValidate()
            {
                if (Unit?.MainDates.GetOrNull<Team>().Type == Team.Typ.Player)
                    Unit = null;
            }
        }

        [System.Serializable]
        public class ElementWave
        {
            [SerializeField]public Unit Template;
            public int Min=1;
            public int Max=2;

            public CountEnemy Get() => new CountEnemy(Random.Range(Min, Max), Template);
            
            public void OnValidate(Level level, int indexWave, int myIndex)
            {
                if (Min < 1)
                    Min = 1;
                if (Max <= Min)
                    Max = Min + 1;
                if (Template?.MainDates.GetOrNull<Team>().Type == Team.Typ.Player)
                {
                    Debug.LogError($"{Template.name} не в комадне врага");
                    Template = null;
                }
                if(Template == null)
                    Debug.LogError($"У {level.name}, волна {indexWave}, элемент {myIndex} не имеет монстра");
            }
        }
    }
}