using Gameplay.UI.Menu.Canvas;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI.Menu
{
    public abstract class ChoiseCanvasFromFactory : MonoBehaviour
    {
        [DI] protected FactoryUIForMainMenu FactoryUi;
        [DI] protected ContainerUIPrefabMainMenu ContainerUipRefab;
        
        public abstract AbsMainMenuCanvas Get();
    }
}