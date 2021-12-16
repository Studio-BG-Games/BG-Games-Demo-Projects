using System.Linq;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Player Unit",order = 51)]
    public class PlayerUnitProfile : AbsCardProfile<Unit, PlayerUnitData>
    {
        protected override void CustomOnValidate()
        {
            if(_target)
                if (_target.MainDates.GetOrNull<Team>().Type != Team.Typ.Player)
                {
                    Debug.LogWarning($"{_target.name} юнит не в команде игрока");
                    _target = null;
                }
        }
    }
}