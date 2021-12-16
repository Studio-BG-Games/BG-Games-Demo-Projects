using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewStrengthAssessment : ViewPartProfile
    {
        [SerializeField] private HorizontalOrVerticalLayoutGroup _horizontalLayout;
        [SerializeField] private Image _imagePrefab;
        [SerializeField] private Sprite _actinIcon;
        [SerializeField] private Sprite _unactinIcon;
        
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            foreach (var t in _horizontalLayout.GetComponentsInChildren<RectTransform>().Except( new []{(RectTransform)_horizontalLayout.transform}))
            {
                Destroy(t.gameObject);
            }
            var strength = profileToView.GetFirstOrNull<StrengthAssessment>();
            if(!strength)
                return;
            for (int i = 0; i < StrengthAssessment.Max; i++)
            {
                var spriteToSpawn = i < strength.Value ? _actinIcon : _unactinIcon;
                var image = Instantiate(_imagePrefab, _horizontalLayout.transform);
                image.sprite = spriteToSpawn;
            }
        }
    }
}