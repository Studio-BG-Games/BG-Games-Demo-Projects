using Mechanics.GameLevel.Stages;
using Mechanics.GameLevel.Stages.ElectroStageParts;
using UnityEngine;

namespace Mechanics.GameLevel.Datas
{
    [CreateAssetMenu(menuName = "Config/Stages/Electro", order = 51)]
    public class ElectroStageData : StageData
    {
        [Min(1)] public int MinBreakWires;
        [Min(2)]public int MaxBreakeWires;
        
        protected override bool ValidateMethod(Stage stageTocheck)
        {
            if (!(stageTocheck is ElectroStage))
            {
                MinBreakWires = 1;
                MaxBreakeWires = 2;
                return false;
            }

            var stage = stageTocheck as ElectroStage;
            if (MaxBreakeWires > stage.MaxBreakeWires)
                MaxBreakeWires = stage.MaxBreakeWires;
            if (MinBreakWires >= MaxBreakeWires)
                MinBreakWires = MaxBreakeWires-1;
            return true;
        }
    }
}