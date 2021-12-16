using DefaultNamespace.Infrastructure.Data;
using Gameplay.UI.Menu.Canvas;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.UI.Menu
{
    public class ContainerUIPrefabMainMenu : MonoBehaviour
    {
        [Header("Окна")]
        public MainCanvas MainCanvas;
        public SettingCanvas SettingCanvas;
        public EquipmentCanvas EquipmentCanvas;
        public CanvasChoiceLevel ChoiseLelevCanvas;
        
        [Header("Вью даты юнитов игрока")]
        public ViewPlayerProfile PleyerUnitViewBig;
        public ViewPlayerProfile PleyerUnitViewSmall;
        public ViewPlayerProfile FullPanelUnitView;
        
        [Header("Вью даты зданий игрока")]
        public ViewBuildProfile PlayerViewBuildBig;
        public ViewBuildProfile PlayerViewBuildSmall;
        public ViewBuildProfile FullPanelBuildView;

        [Header("Other view")] 
        public ViewLevelSetProfile ViewLevelSetProfileSmall;
        public ViewLevelSetProfile ViewLevelSetProfilePanel;
        public ViewEnemyProfile ViewEnenmyProfileBig;
        public ViewEnemyProfile ViewEnenmyProfilePanel;
    }
}