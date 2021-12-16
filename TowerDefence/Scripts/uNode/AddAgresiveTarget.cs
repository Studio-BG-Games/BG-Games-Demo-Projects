using Gameplay;
using Gameplay.Units;
using MaxyGames.uNode;

namespace DefaultNamespace.uNode
{
    public class AddAgresiveTarget : IFlowNode
    {
        public TargetContainer TargetContainer;
        public DamageBy DamageBy;
        
        public void Execute(object graph)
        {
            TargetContainer.SetTargetAttack(DamageBy.Target);
        }
    }
}