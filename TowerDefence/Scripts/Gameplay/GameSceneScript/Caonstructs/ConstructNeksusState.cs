using System;
using Factorys;
using Gameplay.StateMachine.GameScene;
using Gameplay.UI.Game.Canvas;
using Infrastructure.SceneStates;
using Interface;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class ConstructNeksusState : ConstructState
    {
        [DI] private ContainerUIPRefab _containerUipRefab;
        [DI] private GameSceneData _gameSceneData;
        [DI] private FactoryBuild _factoryBuild;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;

        public override void On(LayoutCanvasWithPoint canvasForButton)
        {
            var set = _gameSceneData.LevelSetProfile.Target.GetLevelSet(_gameSceneData.LevelSetProfile.CurrentData.CurrentIndexLevel);
            var neksus = set.Level.Neksus;
            neksus.transform.eulerAngles = Vector3.zero;
            var posneksus = set.PostionNeksus;

            var result = _factoryBuild.Spawn(neksus, new Vector3(posneksus.x, 0, posneksus.y), Vector3.zero);
            if(!result)
                throw new Exception($"НЕКСУС НЕ БЫЛ ПОСТАВЛЕН! ПРОВЕРЬТЕ ВАЛИДНОСТЬ ПОЗИЦИИ - {posneksus}");
            _gameSceneStateMachine.Enter<BuildState>();
        }

        public override void Off() => Ghost.TurnTo(false);
    }
}