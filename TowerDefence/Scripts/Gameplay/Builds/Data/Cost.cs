using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds.Data
{
    [DisallowMultipleComponent]
    public class Cost : BuildProperty
    {
        [SerializeField] [Min(0)] private int _cost;
        public int Gold => _cost;
    }
}