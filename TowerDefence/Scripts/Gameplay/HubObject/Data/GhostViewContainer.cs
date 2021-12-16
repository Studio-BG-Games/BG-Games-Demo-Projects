using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds
{
    public class GhostViewContainer : DataProperty
    {
        public ModelGhost ViewGhost => _viewGhost;
        [SerializeField] private ModelGhost _viewGhost;
    }
}