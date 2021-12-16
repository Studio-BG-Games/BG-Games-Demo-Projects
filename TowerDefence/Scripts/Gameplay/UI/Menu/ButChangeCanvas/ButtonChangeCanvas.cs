using System;
using Factorys;
using Gameplay.GameSceneScript;
using Gameplay.UI.Menu.Canvas;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonChangeCanvas : MonoBehaviour
    {
        [SerializeField] protected AbsMainMenuCanvas Current;
        [SerializeField] protected ChoiseCanvasFromFactory Choise;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Change);
        }

        public abstract void Change();
    }
}