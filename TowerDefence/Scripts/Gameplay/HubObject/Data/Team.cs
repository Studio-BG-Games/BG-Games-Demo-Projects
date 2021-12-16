using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.HubObject.Data
{
    public class Team : DataProperty
    {
        public Typ Type => _typ;
        [SerializeField] private Typ _typ;
        
        public enum Typ
        {
            Player, NonePlayer
        }
    }
}