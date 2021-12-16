using System;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewBuyButton : ViewPartProfile
    {
        public UnityEvent ActionIfIsBuy;
        public UnityEvent HasBuyEvent;
        public UnityEvent WrongByyEvent;
        public UnityEvent ActionIfNotIsBuy;
        
        public event Action HasBuy;
        public event Action WrongBuy;

        [SerializeField] private Button _button;
        [DI] private MainGold _mainGold;
        [DI(SaveDataProvider.OnlineID)] private SaveDataProvider _provider;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            var data = profileToView.CurrentData;
            if (!(data is PlayerBuildData) && !(data is PlayerUnitData))
            {
                Debug.LogWarning($"Этот тип {data.GetType()} не может быть отображен для покупки");
                return;
            }

            if (IsBuy(data))
            {
                ActionIfIsBuy?.Invoke();
                return;
            }
            
            if (!profileToView.GetFirstOrNull<CostPart>())
            {
                Debug.LogWarning($"{profileToView.name} не имеет цены для отображения кнопки перед покупкой");
                return;
            }
            ActionIfNotIsBuy?.Invoke();
            _button?.onClick.AddListener(()=>TryBuy(profileToView, data));
        }

        private void TryBuy<T, TData>(ObjectCardProfile<T, TData> profileToView, TData data) where T : Object where TData : SaveDataProfile
        {
            var cost = profileToView.GetFirstOrNull<CostPart>();
            if (_mainGold.TryRemove(cost.Cost))
            {
                if (data is PlayerBuildData)
                {
                    (data as PlayerBuildData)._isOpen = true;
                    profileToView.SaveNewData(_provider, data);
                }
                else
                {
                    (data as PlayerUnitData)._isOpen = true;
                    profileToView.SaveNewData(_provider, data);
                }
                HasBuyEvent?.Invoke(); 
                HasBuy?.Invoke();
            }
            else
            {
                WrongByyEvent?.Invoke();
                WrongBuy?.Invoke();
            }
        }

        private bool IsBuy<TData>(TData data) where TData : SaveDataProfile
        {
            var data1 = (data as PlayerBuildData);
            if (data1!=null)
                return data1._isOpen;
            return (data as PlayerUnitData)._isOpen;
        }
    }
}