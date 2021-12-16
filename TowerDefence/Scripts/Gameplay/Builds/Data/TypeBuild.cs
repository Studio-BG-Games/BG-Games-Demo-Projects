using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds.Data
{
    [DisallowMultipleComponent]
    public class TypeBuild : BuildProperty
    {
        public Category Categor => _type;
        [SerializeField]private Category _type;
        
        public enum Category
        {
            Main, Extra
        }
    }
}