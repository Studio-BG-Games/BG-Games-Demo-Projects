using System.Collections.Generic;
using System.Linq;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.SketchBook
{
    public class FactoryPage : MonoBehaviour
    {
        [DI] private ConfigGame _configGame;

        [SerializeField] private Page _pageTemplate;

        private DiBox _diBox = DiBox.MainBox;
        
        public List<Page> CreatePage(Book book)
        {
            var listLevel = CopyList(_configGame.ConfigLevels);
            var result = new List<Page>();
            int count = 0;
            while (listLevel.Count>0)
            {
                var newPage = _diBox.CreatePrefab(_pageTemplate);
                newPage.Init(listLevel, out var usedLevels);
                foreach (var usedLevel in usedLevels) 
                    listLevel.Remove(usedLevel);
                result.Add(newPage);
                newPage.transform.SetParent(book.ParentForPage);
                newPage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newPage.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                newPage.Hide(0.00001f);
                count++;
                if(count>2)
                    break;
            }

            return result;
        }

        private List<ConfigLevel> CopyList(List<ConfigLevel> configGameConfigLevels)
        {
            var result = new List<ConfigLevel>();
            configGameConfigLevels.ForEach(x=>result.Add(x));
            return result;
        }
    }
}