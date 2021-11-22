using System.Collections.Generic;
using Mechanics.GameLevel.Datas;
using Mechanics.GameLevel.Stages.NumbetStageParts.Spark;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.NumbetStageParts
{
    public class BoxGenerator : MonoBehaviour
    {
        [SerializeField] private BoxSpark _templateBox;
        [SerializeField] private Transform[] _pointBox;

        private List<BoxSpark> _currentBox = new List<BoxSpark>();

        public List<BoxSpark> GenerateBox(NumberDataStage data, NumberDataStage.MathPattern currentMathExpamle)
        {
            RemoveBox();
            foreach (var point in _pointBox)
            {
                var box = Instantiate(_templateBox, point);
                box.transform.localPosition = Vector3.zero;
                _currentBox.Add(box);
            }

            SetValueBox(data, currentMathExpamle);
            return _currentBox;
        }

        public void RemoveBox()
        {
            foreach (var boxLink in _currentBox)
            {
                if(boxLink)
                    Destroy(boxLink.gameObject);
            }
            _currentBox = new List<BoxSpark>();
        }

        private void SetValueBox(NumberDataStage data, NumberDataStage.MathPattern currentMathExpamle)
        {
            foreach (var box in _currentBox)
            {
                int newValue = Random.Range(data.Min, data.Max+1);
                if (newValue == currentMathExpamle.FinalValue)
                {
                    if (Random.Range(0, 1f) > 0.5f)
                    {
                        newValue++;
                    }
                    else
                    {
                        newValue--;
                    }
                    
                }
                box.Init(newValue);
            }
            _currentBox[Random.Range(0,_currentBox.Count)].Init(currentMathExpamle.FinalValue);
        }
    }
}