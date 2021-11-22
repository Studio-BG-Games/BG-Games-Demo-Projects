using System.Collections.Generic;
using System.Text;
using Mechanics.GameLevel.Stages;
using Mechanics.GameLevel.Stages.NumbetStageParts;
using Mechanics.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Datas
{
    [CreateAssetMenu(menuName = "Config/Stages/Numbers", order = 51)]
    public class NumberDataStage : StageData
    {
        public int Min;
        public int Max;
        public List<MathPattern> Patterns;

        public MathPattern RandomPattern => Patterns[UnityEngine.Random.Range(0, Patterns.Count)];
        
        protected override bool ValidateMethod(Stage stageTocheck)
        {
            if (Min >= Max)
                Min = Max - 1;
            for (int i = 0; i < Patterns.Count; i++)
            {
                int finalValue = Patterns[i].FinalValue;
                if(finalValue<Min || finalValue>Max)
                    Debug.LogWarning($"У {name} пример номер {i} не входит в заданные пределы. Исправьте либо пределы, либо пример");
            }
            return stageTocheck is NumberStage;
        }

        [System.Serializable]
        public class MathPattern
        {
            public int FinalValue
            {
                get
                {
                    int result = 0;
                    foreach (var VARIABLE in Numbers)
                        result += VARIABLE;
                    return result;
                }
            }
            public List<int> Numbers;

            public string GetPatternString()
            {
                StringBuilder builder =new StringBuilder();
                foreach (var num in Numbers)
                {
                    if (num > 0)
                        builder.Append("+");
                    else
                        builder.Append("-");
                    builder.Append(num.ToString());
                }
                builder.Append("=?");
                string result = builder.ToString();
                if (result[0] == '+')
                    result = result.Remove(0, 1);
                return result;
            }
        }
    }

    
}