namespace HealthSystem
{
    public interface IDamageApplier
    {
        void SetDamageAmount(int newDamage);
        int GetDamageAmount();
    }
}