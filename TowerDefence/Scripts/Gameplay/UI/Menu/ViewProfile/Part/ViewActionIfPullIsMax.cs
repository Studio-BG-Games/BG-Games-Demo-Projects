using System.Collections;
using DefaultNamespace.Infrastructure.Data;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewActionIfPullIsMax : ViewPartProfile
    {
        public UnityEvent PullIsMax;
        public UnityAction PullNotIsMax;
        
        [DI] private ICoroutineRunner _coroutineRunner;
        [DI] private DataGameMono _dataGameMono;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if(profileToView is PlayerUnitProfile || profileToView is PlayerBuildProfile)
                _coroutineRunner.StartCoroutine(ViewCor(profileToView));
        }

        private IEnumerator ViewCor<T, TData>(ObjectCardProfile<T, TData> profileToView) where T : Object where TData : SaveDataProfile
        {
            yield return new WaitForEndOfFrame();
            if (profileToView is PlayerUnitProfile)
            {
                if(_dataGameMono.UnitPullIsFull)
                    PullIsMax?.Invoke();
                else
                    PullNotIsMax?.Invoke();
            }
            else if(profileToView is PlayerBuildProfile)
            {
                if(_dataGameMono.BuildPullIsFull)
                    PullIsMax?.Invoke();
                else
                    PullNotIsMax?.Invoke();
            }
        }
    }
}