using System.Collections.Generic;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    public abstract class GiveTarget : MonoBehaviour
    {
        public abstract List<HabObject> All();
        public abstract HabObject GetOne();
    }
}