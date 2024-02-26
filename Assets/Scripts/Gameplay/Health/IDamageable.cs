namespace ZombieSurvivor3D.Gameplay.Health
{
    public interface IDamageable
    {
        void TakeDamage(int damageAmount);

        void TakeContinuousDamage(int damageAmount, float timeDelay);

        void StopContinuousDamage();

    }
}