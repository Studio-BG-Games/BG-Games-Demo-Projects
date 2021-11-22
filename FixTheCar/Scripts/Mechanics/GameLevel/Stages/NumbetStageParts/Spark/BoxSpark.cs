using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.Spark
{
    public class BoxSpark : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _label;
        
        public int ValueResult { get; private set; }
        public event Action<BoxSpark> ClickedOn;
        
        public void Init(int value)
        {
            ValueResult = value;
            _label.text = value.ToString();
        }

        private void OnDestroy() => ClickedOn = (Action<BoxSpark>)Delegate.RemoveAll(ClickedOn, ClickedOn);

        public void OnPointerClick(PointerEventData eventData) => ClickedOn?.Invoke(this);
    }
}