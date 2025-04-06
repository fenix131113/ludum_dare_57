namespace HealthSystem
{
    public interface IHealth
    {
        int GetHealth();
        int GetMaxHealth();
        bool CanGetDamage();
        void TakeDamage(int damage);
    }
}