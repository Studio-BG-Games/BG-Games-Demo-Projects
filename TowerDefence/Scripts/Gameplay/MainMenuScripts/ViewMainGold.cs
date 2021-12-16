using System.Runtime.InteropServices;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Menu
{
    public class ViewMainGold : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [DI] private MainGold _mainGold;

        private void Start()
        {
            _label.text = _mainGold.Current.ToString(); 
            _mainGold.NewValue += OnNewValue;
        }

        private void OnNewValue(int obj) => _label.text = obj.ToString();
    }
}