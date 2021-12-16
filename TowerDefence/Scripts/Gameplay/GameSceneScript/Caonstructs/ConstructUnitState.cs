using System;
using System.Collections.Generic;
using Factorys;
using Gameplay.UI;
using Gameplay.UI.Game.Canvas;
using Interface;
using Plugins.DIContainer;
using Plugins.HabObject;

namespace Gameplay.GameSceneScript
{
    public class ConstructUnitState : ConstructHabSatate
    {
        [DI] private AvaibleHabToCreate _avaible;

        protected override ConstructButtonHab GetButtonTemplate(ContainerUIPRefab containerUipRefab) => containerUipRefab.UnitButton;

        protected override List<HabObject> GetHabs()
        {
            List<HabObject> result = new List<HabObject>();
            _avaible.GetAvaibleUnit().ForEach(x=>result.Add(x));
            return result;
        }
    }
}