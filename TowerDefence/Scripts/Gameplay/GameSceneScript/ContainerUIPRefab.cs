using Gameplay.UI;
using Gameplay.UI.Game;
using Gameplay.UI.Game.Canvas;
using Gameplay.UI.Game.UIElement;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class ContainerUIPRefab : MonoBehaviour
    {
        public CanvasWithStat _CanvasWith;

        public LayoutCanvas CanvasTemplateForInfoBuild => _canvasTemplateForInfoBuild;
        [SerializeField] private LayoutCanvas _canvasTemplateForInfoBuild;

        public LayoutCanvasWithPoint CanvasForButton => _canvasForButton;
        [SerializeField] private LayoutCanvasWithPoint _canvasForButton;
        public RollGrid CanvasConstructButton;

        [Header("Кнопки команд")]
        public UIButton AcceptButton;
        public UIButton CancelButton;
        public UIButton DeleteButton;
        public UIButton RotateButtonRight;
        public UIButton RotateButtonLeft;
        
        [Header("Кнопки для выбора здания или юнита")]
        public ConstructButtonHab BuildButton;
        public ConstructButtonHab UnitButton;
        
        [Header("Канвасы")]
        public FightCanvas FightCanvas;
        public LoseCanvas LoseCanvas;
        public WinCanvas WinCanvas;
        public CanvasButtonChoise TemplateChoiseButton;
    }
}