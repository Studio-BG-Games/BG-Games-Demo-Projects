using System;
using System.Collections.Generic;
using Gameplay.Builds;
using Gameplay.Builds.Beh;
using Gameplay.Builds.Data.Marks;
using UnityEngine;

namespace Gameplay.Waves
{
    [CreateAssetMenu(menuName = "GameSO/Level", order = 49)]
    public class Level : ScriptableObject
    {
        public Build Neksus;
        public Build Portal;
        [Min(0)] public int StartGold;
        [Min(0)]public int AddingGoldByAd;
        [Range(0, 1f)] public float ChangeToRecoveryUnit;
        
        public Wave[] Waves = new Wave[0];

        public List<string> GetAllId()
        {
            List<string> result = new List<string>();
            foreach (var wave in Waves)
            {
                wave.GetAllId().ForEach(x =>
                {
                    if(!result.Contains(x))
                        result.Add(x);
                });
            }

            return result;
        }
        
        private void OnValidate()
        {
            if (Portal && !Portal.ComponentShell.Get<Portal>())
                Portal = null;
            if (Neksus && !Neksus.MainDates.GetOrNull<NeksusMark>())
                Neksus = null;
            for (var i = 0; i < Waves.Length; i++)
            {
                Waves[i].OnValidate(this, i);
            }
            if(!Portal)
                Debug.LogError($"{this.name}, не имеетт здание с поведением портала");
            if (!Neksus)
            {
                Debug.LogError($"{this.name}, не имеетт здание с даттой нексуса");
                throw new Exception($"{this.name}, не имеетт здание с даттой нексуса");
            }
        }
    }
}