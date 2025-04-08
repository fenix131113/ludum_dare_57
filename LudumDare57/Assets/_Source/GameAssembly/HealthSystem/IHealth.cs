namespace HealthSystem
{
    public interface IHealth
    {
        int GetHealth();
        int GetMaxHealth();

        bool CanGetDamage() => true;
        void TakeDamage(int damage);
    }
}