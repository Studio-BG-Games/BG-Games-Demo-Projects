using DefaultNamespace.Infrastructure.Data;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class OpenViewPanelButtom : ViewPartProfile, IInitByIOpenViewProfilePanel
    {
        [SerializeField] private Button _button;
        
        private IOpenViewProfilePanel _panel;
        
        [DI] private ContainerUIPrefabMainMenu _containerUipRefab;
        
        public void Init(IOpenViewProfilePanel viewProfilePanel) => _panel = viewProfilePanel;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            _button.onClick.RemoveAllListeners();
            if(profileToView is PlayerUnitProfile)
                _button.onClick.AddListener(()=>_panel.Open(_containerUipRefab.FullPanelUnitView, profileToView as PlayerUnitProfile));
            else if(profileToView is PlayerBuildProfile)
                _button.onClick.AddListener(()=>_panel.Open(_containerUipRefab.FullPanelBuildView, profileToView as PlayerBuildProfile));
            else if (profileToView is LevelSetProfile)
                _button.onClick.AddListener(()=>_panel.Open(_containerUipRefab.ViewLevelSetProfilePanel, profileToView as LevelSetProfile));
            else
                _button.onClick.AddListener(()=>_panel.Open(_containerUipRefab.ViewEnenmyProfilePanel, profileToView as EnemyUnitProfile));

            
            
        }
    }
}