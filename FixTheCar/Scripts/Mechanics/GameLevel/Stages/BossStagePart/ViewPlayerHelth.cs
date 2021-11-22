using System.Security.Cryptography;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class ViewPlayerHelth : MonoBehaviour, IInitPlayerHealth
    {
        [SerializeField] private HorizontalLayoutGroup _horizontalLayout;
        [SerializeField] private Image _templateHealthPicture;
        
        private PlayerHelth _healthPlayer;

        public void Init(PlayerHelth health)
        {
            _healthPlayer = health;
            _healthPlayer.HealthUpdate += OnUpdateHealth;
            _healthPlayer.ManualUpdate();
        }

        private void OnUpdateHealth(int obj)
        {
            foreach (Image image in _horizontalLayout.transform.GetComponentsInChildren<Image>()) Destroy(image.gameObject);
            for (int i = 0; i < obj; i++) Instantiate(_templateHealthPicture, _horizontalLayout.transform);
        }
    }
}