using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.UI
{
    public abstract class PartInitConstructButton : MonoBehaviour
    {
        public abstract void Init<T>(T template) where T : HabObject;
    }
}