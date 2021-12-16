using Gameplay.UI.Menu.Canvas;
using Plugins.DIContainer;

namespace Gameplay.UI.Menu
{
    public class ChoiseMainCanvas : ChoiseCanvasFromFactory
    {
        public override AbsMainMenuCanvas Get() => FactoryUi.GetOrCreate(ContainerUipRefab.MainCanvas);
    }
}