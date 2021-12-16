using System.Security.Cryptography;
using UnityEngine;

namespace Gameplay.UI.Game.Canvas
{
    public class LayoutCanvas : CustomCanvas
    {
        [SerializeField] private Transform _content;
        
        public void AddContent(Transform rectTransform) => rectTransform.SetParent(_content);

        public void RemoveAllContent()
        {
            foreach (Transform trans in _content.GetComponentsInChildren<Transform>())
                if(trans!=null)
                    if(trans!=_content)
                        Destroy(trans.gameObject);
        }
    }
}