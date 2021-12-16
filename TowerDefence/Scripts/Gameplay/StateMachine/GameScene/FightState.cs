using System.Xml;
using Factorys;
using Gameplay.Builds;
using Gameplay.GameSceneScript;
using Gameplay.UI.Game.Canvas;
using Pathfinding.RVO;
using Plugins.DIContainer;

namespace Gameplay.StateMachine.GameScene
{
    public class FightState : GameSceneState
    {
        [DI] private GameSceneStateMachine _gameSceneStateMachine;
        [DI] private InterpreterWaveLevel _interpreterWaveLevel;
        [DI] private ContainerUIPRefab _containerUipRefab;
        [DI] private FactoryUIForGameScene _factoryUiForGameScene;
        
        public override void Enter()
        {
            _interpreterWaveLevel.Win += OnWin;
            _interpreterWaveLevel.Lose += OnLose;
            if(!_interpreterWaveLevel.LevelIsComplited)
                _interpreterWaveLevel.StartNextWave();
            GetCanvas().gameObject.SetActive(true);
        }

        private FightCanvas GetCanvas()
        {
            return _factoryUiForGameScene.GetOrCreate<FightCanvas>(_containerUipRefab.FightCanvas, "fightCanvasss");
        }


        public override void Update()
        {
        }

        public override void Exit()
        {
            _interpreterWaveLevel.Win -= OnWin;
            _interpreterWaveLevel.Lose -= OnLose;
            GetCanvas().gameObject.SetActive(false);
        }

        private void OnLose()
        {
            _gameSceneStateMachine.Enter<LoseState>();
        }
        

        private void OnWin()
        {
            if(_interpreterWaveLevel.LevelIsComplited) _gameSceneStateMachine.Enter<WinState>();
            else _gameSceneStateMachine.Enter<BuildState>();
        }


    }
}