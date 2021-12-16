using System;
using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Gameplay.Localizators;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public abstract class ViewStringInfo : ViewPartProfile
    {
        [SerializeField] private string _targetIdStringInfo;

        public Localizator Localizator => _localizator??=DiBox.MainBox.ResolveSingle<Localizator>();
        private Localizator _localizator;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
             var card = profileToView.GetAll<StringInfoPartCard>().FirstOrDefault(x=>x.IdCard==_targetIdStringInfo);
             if (!card)
             {
                 Debug.LogWarning($"No target id in view of part card", this);
                 SetText("");
             }
             else
             {
                 var str = Localizator.GetText(card.NameFile, card.IdLocalization, out var boolResultBool);
                 if (!boolResultBool)
                 {
                     Debug.LogError($"Установка текста из серилизатора неправильная, проверьте этот объект SO {card.name} на правильность имя файла и id, кликни на мне два раза", card);
                 }
                 SetText(str);
                 Localizator.NewLangSet += ()=>View<T, TData>(profileToView);
             }
        }
        
        protected abstract void SetText(string text);
    }
}