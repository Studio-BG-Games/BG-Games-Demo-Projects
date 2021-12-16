using System;
using System.Collections.Generic;
using Gameplay.UI;
using Gameplay.UI.Game.Canvas;
using Plugins.DIContainer;
using Plugins.HabObject;

namespace Gameplay.GameSceneScript
{
    public class ConstructExtraBuildState : ConstructHabSatate
    {
        [DI] private AvaibleHabToCreate _avaible;

        protected override ConstructButtonHab GetButtonTemplate(ContainerUIPRefab containerUipRefab) => containerUipRefab.BuildButton;

        protected override List<HabObject> GetHabs()
        {
            List<HabObject> result = new List<HabObject>();
            _avaible.ExtraBuild.ForEach(x=>result.Add(x));
            return result;
        }
    }
}