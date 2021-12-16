using Gameplay;
using MaxyGames.uNode;

namespace DefaultNamespace.uNode
{
    public class HasAgresiveTarget : IStateNode
    {
        public DamageBy DamageBy;
        
        public bool Execute(object graph)
        {
            return DamageBy.HasTarget;
        }
    }
}