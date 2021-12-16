using Gameplay.UI.Menu.Canvas;

namespace Gameplay.UI.Menu
{
    public class ChoiseEquipmentCanvas : ChoiseCanvasFromFactory
    {
        public override AbsMainMenuCanvas Get() => FactoryUi.GetOrCreate(ContainerUipRefab.EquipmentCanvas);
    }
}