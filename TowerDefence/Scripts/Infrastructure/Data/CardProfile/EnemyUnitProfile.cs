using Gameplay.HubObject.Data;
using Gameplay.Units;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Enemy unit", order = 51)]
    public class EnemyUnitProfile : AbsCardProfile<Unit, EnemyUnitData>
    {
        protected override void CustomOnValidate()
        {
            if(Target)
                if (Target.MainDates.GetOrNull<Team>().Type != Team.Typ.NonePlayer)
                {
                    Debug.LogWarning($"{Target.name} юнит не входит в команду противников");
                    _target = null;
                }
        }
    }
}