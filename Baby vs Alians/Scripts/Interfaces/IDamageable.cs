namespace Baby_vs_Aliens
{
    public interface IDamageable
    {
        EntityType Type { get; }

        void TakeDamege(int damage);
    }
}