using Gameplay.Units.Beh;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    [BehaviourButton("Combat/Attack/Choise two attack")]
    public class ChoiseBettwenAttack : AbsAttack
    {
        [SerializeField] private AbsAttack _attack1;
        [SerializeField] private AbsAttack _attack2;
        [SerializeField] private BoolChance _boolChance;

        private AbsAttack _curentAttack;

        public override bool InProgress => _curentAttack ? _curentAttack.InProgress : false;
                
        public override void Attack()
        {
            if(InProgress)
                return;
            if (!InProgress)
                _curentAttack = ChoiseAttack();
            CallStartEvet();
            _curentAttack.Attack();
        }

        private AbsAttack ChoiseAttack() => _boolChance.Check() ? _attack1 : _attack2;
    }
}