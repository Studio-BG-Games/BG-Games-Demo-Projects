namespace Gameplay.Units.Beh
{
    [BehaviourButton("Chance/bool")]
    public class BoolChance : AbsChance<bool>
    {
        protected override bool Succes => true;
        protected override bool Lose => false;
    }
}