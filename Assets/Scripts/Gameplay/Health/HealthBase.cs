using System.Collections;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Health
{
    public abstract class HealthBase : GameListener, IDamageable
    {

        [Header("Main Elements")]
        [SerializeField] protected HealthComponent HealthComponent;
        [SerializeField] protected bool isAffected;
        protected const int ZERO_HEALTH = 0;

        protected virtual void InitializeHealthComponent(int health)
        {
            HealthComponent newHealthComp = new HealthComponent(health);
            HealthComponent = newHealthComp;
        }

        // MUST BE PUBLIC, IN ORDER TO ACCESS DAMAGE:
        public virtual void TakeDamage(int damageAmount)
        {
            HealthComponent.TakeDamage(damageAmount);
        }

        public virtual void TakeContinuousDamage(int damageAmount, float timeDelay) 
        {
            isAffected = true;
        }

        public void StopContinuousDamage()
        {
            isAffected = false;
        }
    }
}