using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.UI.Game.Canvas
{
    public class CheckDrag : MonoBehaviour, IDragHandler
    {
        public event Action InDrag;
        
        public void OnDrag(PointerEventData eventData)
        {
            InDrag?.Invoke();
        }
    }
}