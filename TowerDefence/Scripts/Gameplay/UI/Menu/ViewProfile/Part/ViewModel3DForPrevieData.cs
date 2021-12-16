using System;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewModel3DForPrevieData : ViewPartProfile
    {
        [DI] private PreviewCamera _preview;
        [SerializeField] private ShellRawImage _shellRawImage;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            _preview.DeleteModel();
            _shellRawImage.SetRenderTexture(_preview.RenderTexture);
            var modelData = profileToView.GetFirstOrNull<Model3DForPrevieData>();
            if(!modelData)
                return;
            if (modelData.Model == null)
            {
                Debug.LogError($"{profileToView}, нет модели для отображения", profileToView);
                return;
            }

            
            _preview.SetModel(modelData.Model);
        }
    }
}