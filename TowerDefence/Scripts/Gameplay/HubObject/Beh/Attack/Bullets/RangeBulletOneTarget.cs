using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.Attack
{
    public class RangeBulletOneTarget : AbsBullet
    {
        protected override List<HabObject> GetTargets()
        {
            return new List<HabObject>(){Target};
        }
    }
}