using Gameplay.Builds;
using Gameplay.HubObject.Data;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Player Build", order = 51)]
    public class PlayerBuildProfile : AbsCardProfile<Build, PlayerBuildData>
    {
        protected override void CustomOnValidate()
        {
            if(_target)
                if (_target.MainDates.GetOrNull<Team>().Type != Team.Typ.Player)
                {
                    Debug.LogWarning($"{_target.name} здание не в команде игрока");
                    _target = null;
                }
        }
    }
}