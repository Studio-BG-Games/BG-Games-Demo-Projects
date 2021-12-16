using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds.Data
{
    [DisallowMultipleComponent]
    public class MaterialBuild : BuildProperty
    {
        [SerializeField] private string _material;
    }
}