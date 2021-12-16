using Gameplay.UI.Menu.Canvas;

namespace Gameplay.UI.Menu
{
    public class ChoiseCanvasChoiceLevel : ChoiseCanvasFromFactory
    {
        public override AbsMainMenuCanvas Get() => FactoryUi.GetOrCreate(ContainerUipRefab.ChoiseLelevCanvas);
    }
}