using System;
using Gameplay.GameSceneScript;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class CanvasWithStat : CustomCanvas
    {
        [SerializeField] private Text _goldLabel;

        [DI] private IGold _gold;
        
        private void OnEnable()
        {
            _goldLabel.text = _gold.Current.ToString();
            _gold.Update += OnUpdate;
        }

        private void OnUpdate(int obj) => _goldLabel.text = obj.ToString();

        private void OnDisable()
        {
            _gold.Update -= OnUpdate;
        }
    }
}