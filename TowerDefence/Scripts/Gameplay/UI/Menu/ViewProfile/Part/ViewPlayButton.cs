using System;
using System.Collections.Generic;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Gameplay.Map.Generator;
using Gameplay.Units;
using Infrastructure.ConfigData;
using Infrastructure.SceneStates;
using Interface;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewPlayButton : ViewPartProfile, IInitByIOpenViewProfilePanel
    {
        [SerializeField] private Button _button;

        [DI] private AppStateMachine _appStateMachine;
        [DI] private ICurtainProgress _curtainProgress;
        [DI] private DataGameMono _gameMono;
        [DI] private ConfigGame _configGame;
        private IOpenViewProfilePanel _profileView;

        [DI(SaveDataProvider.OnlineID)] private SaveDataProvider _saveDataProvider; 

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if (!(profileToView is LevelSetProfile))
            {
                Debug.LogError("Кнопка играть, должна находиться на View level set profile");
                return;
            }
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(()=>Play(profileToView as LevelSetProfile));
        }

        private void Play(LevelSetProfile levelSetProfile)
        {
            _button.interactable = false;
            ((MonoBehaviour) _profileView).GetComponent<CanvasGroup>().interactable = false;
            _curtainProgress.SetProgress(0);
            _curtainProgress.Fade(()=>_appStateMachine.Enter<GameScene, GameSceneData>(GetData(levelSetProfile)));
        }

        private GameSceneData GetData(LevelSetProfile levelSetProfile)
        {
            if (levelSetProfile.CurrentData.CurrentIndexLevel >= levelSetProfile.Target.CountLevel)
            {
                levelSetProfile.CurrentData.CurrentIndexLevel = 0;
                levelSetProfile.SaveNewData(_saveDataProvider, levelSetProfile.CurrentData);
            }
            return new GameSceneData(
                levelSetProfile.Target.DataMap, 
                GetGenerator(levelSetProfile), 
                GetBulds(_gameMono, _configGame), 
                GetUnits(_gameMono, _configGame),
                levelSetProfile.Target.GetLevelSet(levelSetProfile.CurrentData.CurrentIndexLevel).Level, 
                levelSetProfile);
        }

        private List<Unit> GetUnits(DataGameMono gameMono, ConfigGame configGame)
        {
            var playerPull = gameMono.GetUnitPull();
            List<Unit> result = new List<Unit>();
            foreach (var id in playerPull)
            {
                var unit = _configGame.ConfigContainerUnit.GetTByIdOrNull(id);
                
                if (unit == null)
                    throw new NullReferenceException($"по данному id {id} в списке юнитов в конфиге нет никакого юнита");
                if (unit.MainDates.GetOrNull<Team>().Type == Team.Typ.Player)
                    result.Add(unit);
                else
                    Debug.LogError($"{id} {unit} не в команде игрока, поэтому в игре его не будет", unit);
            }
            return result;
        }

        private List<Build> GetBulds(DataGameMono gameMono, ConfigGame configGame)
        {
            var buildId = gameMono.GetBuildPull();
            List<Build> result = new List<Build>();
            foreach (var id in buildId)
            {
                var build = configGame.ConfigContainerBuild.GetTByIdOrNull(id);
                if (build == null)
                    throw new NullReferenceException($"по данному id {id} в списке строений в конфиге нет никакого здания");
                if (build.MainDates.GetOrNull<Team>().Type == Team.Typ.Player) result.Add(build);
                else Debug.LogError($"{id}, {build} не в команде игрока, поэтому в игре его не будет", build);
            }
            return result;
        }

        private GeneratorMap GetGenerator(LevelSetProfile levelSetProfile)
        {
            var set = levelSetProfile.Target.GetLevelSet(levelSetProfile.CurrentData.CurrentIndexLevel);
            return new SimpelGenerator(set.SeedLansScape, set.SeedPropBuild,  set.WithWater, false);
        }

        public void Init(IOpenViewProfilePanel viewProfilePanel) => _profileView = viewProfilePanel;
    }
}