using UnityEngine;

namespace Mechanics.GameLevel.Stages
{
    public abstract class StageData : ScriptableObject
    {
        public Stage StageTemplate;

        protected abstract bool ValidateMethod(Stage stageTocheck);

        private void OnValidate()
        {
            if(ValidateMethod(StageTemplate) || !StageTemplate)
                return;
            Debug.LogWarning($"StageData - {name}, не принимает данный шаблон stage - {StageTemplate.GetType()}");
            StageTemplate = null;
        }
    }
}