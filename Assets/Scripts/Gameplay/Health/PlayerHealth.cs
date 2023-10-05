using System.Collections;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Health
{
    public class PlayerHealth : HealthBase
    {

        [Header("Player Elements")]
        [SerializeField] bool isPlayerHit = false;
        [SerializeField] float timeElapsed;
        [SerializeField] float timeElapsedThreshold = 3f;

        private const int PLAYER_MAX_HEALTH = 3;

        void Start() => InitializeHealthComponent(PLAYER_MAX_HEALTH);

        private void Update() => RegenerateHealth();


        private void RegenerateHealth()
        {
            if (HealthComponent.GetCurrentHealth() <= ZERO_HEALTH)
                return;

            if (isPlayerHit)
            {
                timeElapsed -= Time.deltaTime;

                if (timeElapsed <= 0)
                {
                    HealthComponent.SetCurrentHealthToMax();
                    isPlayerHit = false;
                }
            }
        }

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);

            timeElapsed = timeElapsedThreshold;
            isPlayerHit = true;

            if (HealthComponent.GetCurrentHealth() <= ZERO_HEALTH)
            {
                isPlayerHit = false;
                gameObject.SetActive(false);
                //Debug.Log("Object Died!: " + transform.parent.gameObject.name);
            }
        }


    }
}