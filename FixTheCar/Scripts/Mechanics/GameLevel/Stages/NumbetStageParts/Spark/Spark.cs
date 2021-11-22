using System.Collections.Generic;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.Spark
{
    public class Spark : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [DI] private IInput _input;
        
        public void OnDrag(PointerEventData eventData)
        {
            Vector3 newPosition =Camera.main.ScreenToWorldPoint(_input.InputScreenPosition);
            newPosition.z = 0;
            transform.position = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);
            ShadowSpark shadow = null;
            foreach (var l in list)
                if(l.gameObject.TryGetComponent<ShadowSpark>(out shadow))
                    break;
            if(shadow && shadow.ApplaySpark(this))
                return;
            else
                transform.localPosition = Vector3.zero;
        }
    }
}