using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class SelectorImageByState : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _oilSprite;
        [SerializeField] private Sprite _fuelSprite;
        [SerializeField] private Sprite _spriteEmpty;
        
        public void SelectImageBy(CanistroState state)
        {
            if (state is FuelState) _image.sprite = _fuelSprite;
            else if (state is EmptyState) _image.sprite = _spriteEmpty;
            else if (state is OilState) _image.sprite = _oilSprite;
            else throw new Exception("don't have sprite for - " + state.GetType());
        }
    }
}