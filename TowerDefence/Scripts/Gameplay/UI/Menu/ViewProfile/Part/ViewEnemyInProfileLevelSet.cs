using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewEnemyInProfileLevelSet : ViewPartProfile, IInitByIOpenViewProfilePanel
    {
        [SerializeField] private HorizontalOrVerticalLayoutGroup _horizontalOrVertical;

        [DI] private FactoryUIForMainMenu _factoryUi;
        [DI] private ContainerUIPrefabMainMenu _containerUi;
        [DI] private ConfigGame _configGame;
        private IOpenViewProfilePanel _viewProfiler;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            ClearChild();
            if(!(profileToView is LevelSetProfile))
                return;
            var p = profileToView as LevelSetProfile;
            var index = p.CurrentData.CurrentIndexLevel >= p.Target.CountLevel ? 0 : p.CurrentData.CurrentIndexLevel;
            var level = p.Target.GetLevelSet(index).Level;

            var idsEnemy = level.GetAllId();
            var correctEnemy = _configGame.EnemyUnitProfiles.Where(x => idsEnemy.Contains(x.Target.MainDates.GetOrNull<IdContainer>().ID));
            if(correctEnemy.Count()!=idsEnemy.Count)
                Debug.LogError("Удалось отобразить не всех врагов, проверьте наличие профайлов для отсуствующий врагов");
            foreach (var enemy in correctEnemy)
            {
                var view = _factoryUi.CreateViewProfile(_containerUi.ViewEnenmyProfileBig);
                view.Init(enemy, _viewProfiler);
                view.transform.SetParent(_horizontalOrVertical.transform);
            }
        }

        private void ClearChild()
        {
            _horizontalOrVertical
                .GetComponentsInChildren<Transform>()
                .Except(new []{_horizontalOrVertical.transform})
                .ToList()
                .ForEach(x=>Destroy(x.gameObject));
        }

        public void Init(IOpenViewProfilePanel viewProfilePanel) => _viewProfiler = viewProfilePanel;
    }
}