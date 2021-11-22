using System;
using System.Collections.Generic;
using Factories;
using Mechanics.GameLevel.Datas;
using Mechanics.GameLevel.Stages.NumbetStageParts.Spark;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.NumbetStageParts
{
    public class BoxClickHandler : MonoBehaviour
    {
        public event Action SuccessClick;
        public event Action FailClick;

        private NumberDataStage.MathPattern _mathPattern;
        private Engine _engine;
        
        [DI] private FactorySpark _factorySpark;
        
        public void Init(List<BoxSpark> boxes, NumberDataStage.MathPattern currentPattern, Engine engine)
        {
            _engine = engine;
            _mathPattern = currentPattern;
            foreach (var box in boxes) box.ClickedOn += ClickOnBox;
        }
        
        private void ClickOnBox(BoxSpark box)
        {
            if (box.ValueResult == _mathPattern.FinalValue)
            {
                var spark = _factorySpark.Create(_engine.CurrentSparkType);
                spark.transform.SetParent(box.transform.parent);
                spark.transform.localPosition = Vector3.zero;
                Destroy(box.gameObject);
                SuccessClick?.Invoke();
            }
            else
            {
                FailClick?.Invoke();
            }
        }
    }
}