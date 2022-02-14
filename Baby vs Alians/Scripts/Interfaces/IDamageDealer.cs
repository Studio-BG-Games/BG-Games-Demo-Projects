namespace Baby_vs_Aliens
{
    public interface IDamageDealer
    {
        EntityType Type { get; }

        int Damage { get; }
    }
}