using DefaultNamespace.Infrastructure.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class CloseViewPanelButton : ViewPartProfile, IInitByIOpenViewProfilePanel
    {
        [SerializeField] private Button _button;
        private IOpenViewProfilePanel _panel;

        public void Init(IOpenViewProfilePanel viewProfilePanel) => _panel = viewProfilePanel;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(()=>_panel.Close());
        }
    }
}