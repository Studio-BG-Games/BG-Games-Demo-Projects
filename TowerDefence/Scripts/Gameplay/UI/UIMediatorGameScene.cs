using Factorys;
using Gameplay.StateMachine.GameScene;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI
{
    public class UIMediatorGameScene : MonoBehaviour
    {
        [DI] private FactoryUIForGameScene _factoryUi;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;

        public void ChangeState<T>() where T : GameSceneState, new() => _gameSceneStateMachine.Enter<T>();
    }
}