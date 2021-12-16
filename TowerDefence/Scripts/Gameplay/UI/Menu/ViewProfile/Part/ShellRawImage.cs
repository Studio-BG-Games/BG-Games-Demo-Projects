using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    [RequireComponent(typeof(RawImage))]    
    public class ShellRawImage : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [DI] private PreviewCamera _previewCamera;
        
        private RawImage _rawImage;
        private Vector2 _lastPosition; 
        
        private void Awake() => _rawImage = GetComponent<RawImage>();

        public void SetRenderTexture(RenderTexture texture) => _rawImage.texture = texture;

        public void OnBeginDrag(PointerEventData eventData) => _lastPosition = eventData.position;

        public void OnDrag(PointerEventData eventData)
        {
            var dir = _lastPosition - eventData.position;

            _previewCamera.Rotate(dir.x);
            
            _lastPosition = eventData.position;
        }
    }
}