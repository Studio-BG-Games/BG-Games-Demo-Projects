using DefaultNamespace;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.UI.Menu;
using Infrastructure.ConfigData;
using Interface;
using Plugins.DIContainer;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;
using UnityEngine;

namespace Infrastructure.SceneStates
{
    public class MainMenu : IEnterState, IPayLoadedState<MainMenu_WinData>
    {
        [DI] private SceneLoader _sceneLoader;
        [DI] private ConfigGame _configGame;
        [DI] private ICoroutineRunner _coroutineRunner;
        [DI] private ICurtain _curtainProgress;
        [DI(SaveDataProvider.LocalID)] private SaveDataProvider _provderLocal;
        [DI] private AudioMixerScript _audioMixerScript;

        [DI(SaveDataProvider.OnlineID)] private SaveDataProvider _provider;
        
        public void Enter()
        {
            _sceneLoader.Load(_configGame.SceneNames.Menu, () => { DefaultOnLoad(); });
        }

        private void DefaultOnLoad()
        {
            _curtainProgress.Unfade();
            var factroyUI = DiBox.MainBox.ResolveSingle<FactoryUIForMainMenu>();
            var container = DiBox.MainBox.ResolveSingle<ContainerUIPrefabMainMenu>();
            var canvas = factroyUI.GetOrCreate(container.MainCanvas);
            canvas.gameObject.SetActive(true);
            ((RectTransform) canvas.RootPanel.transform).anchoredPosition = Vector2.zero;
            _audioMixerScript.SetValueToMixer(_provderLocal.GetOrDefault<DataSettingGame>());
        }

        public void Enter(MainMenu_WinData dataScene)
        {
            int toAddMainGold = dataScene.LevelSetProfile.Target.GetLevelSet(dataScene.LevelSetProfile.CurrentData.CurrentIndexLevel).AwardForLevel;
            dataScene.LevelSetProfile.CurrentData.CurrentIndexLevel++;
            if (dataScene.LevelSetProfile.Target.CountLevel == dataScene.LevelSetProfile.CurrentData.CurrentIndexLevel)
                dataScene.LevelSetProfile.CurrentData.IsCompletedSet = true;
            
            dataScene.LevelSetProfile.SaveNewData(_provider,dataScene.LevelSetProfile.CurrentData);
            _sceneLoader.Load(_configGame.SceneNames.Menu, () =>
            {
                DefaultOnLoad();
                DiBox.MainBox.ResolveSingle<MainGold>().Add(toAddMainGold);
            });
        }

        public void Exit()
        {
            
        }
    }

    public class MainMenu_WinData
    {
        public LevelSetProfile LevelSetProfile { get; private set; }

        public MainMenu_WinData(LevelSetProfile setProfile) => LevelSetProfile = setProfile;
    }
}