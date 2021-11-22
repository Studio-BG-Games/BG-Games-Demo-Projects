using Mechanics.GameLevel.Stages;
using Mechanics.GameLevel.Stages.СanistorStageParts;
using UnityEngine;

namespace Mechanics.GameLevel.Datas
{
    [CreateAssetMenu(menuName = "Config/Stages/Canistro", order = 51)]
    public class CanistroStageData : StageData
    {
        protected override bool ValidateMethod(Stage stageTocheck) => stageTocheck is CanistorStage;
    }
}