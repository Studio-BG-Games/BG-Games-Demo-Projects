using BehTreeBrick.Action;
using UnityEngine;

namespace BehTreeBrick.Conditrion
{
    [BehaviourButton("Combat/RicherTargetRadius")]
    public class RicherTargetRadius : RicherTarget
    {
        [SerializeField] private SearcherTargetInRadius _searcherTargetInRadius;


        public override bool IsRich()
            => _searcherTargetInRadius.HasAnyTargetInZone;
    
    }
}