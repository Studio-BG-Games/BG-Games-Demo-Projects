using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript.DiFabric
{
    public class FabricDiGameScene : FactoryDI
    {
        [RequireInterface(typeof(IGold))] [SerializeField] private Object _gold;
        
        public override void Create(DiBox container)
        {
            container.RegisterSingle<IGold>(_gold as IGold);
        }

        public override void DestroyDi(DiBox container)
        {
            container.RemoveSingel<IGold>();
        }
    }
}