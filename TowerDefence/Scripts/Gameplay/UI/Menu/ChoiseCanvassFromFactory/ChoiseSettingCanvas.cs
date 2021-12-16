using Gameplay.UI.Menu.Canvas;

namespace Gameplay.UI.Menu
{
    public class ChoiseSettingCanvas : ChoiseCanvasFromFactory
    {
        public override AbsMainMenuCanvas Get() => FactoryUi.GetOrCreate(ContainerUipRefab.SettingCanvas);
    }
}