using System.Collections;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.ObjectPool;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Health
{
    public class EnemyHealth : HealthBase
    {

        [Header("Enemy Elements")]
        [SerializeField] private EnemyTypes enemyType = EnemyTypes.Weakling;

        public enum EnemyTypes
        {
            Weakling, Elite, Boss
        }

        private void OnEnable() => SetEnemyTypeHealth();


        void Start() => SetEnemyTypeHealth();


        private void SetEnemyTypeHealth()
        {
            if (enemyType == EnemyTypes.Weakling)
                InitializeHealthComponent(100);

            if (enemyType == EnemyTypes.Elite)
                InitializeHealthComponent(200);

            if (enemyType == EnemyTypes.Boss)
                InitializeHealthComponent(1000);
        }

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);

            DespawnEnemy();
            UpdateScore(10);
        }

        public override void TakeContinuousDamage(int damageAmount, float timeDelay)
        {
            base.TakeContinuousDamage(damageAmount, timeDelay);
            StartCoroutine(DamageOverTimeCoroutine(damageAmount, timeDelay));
        }

        private IEnumerator DamageOverTimeCoroutine(int damageAmount, float timeDelay)
        {
            int hitCount = 0;
            while (hitCount < 5 || isAffected) // hitCount < 5 || isAffected = true;
            {
                if (isAffected)
                    hitCount = 0;

                TakeDamage(damageAmount);
                hitCount++;
                yield return new WaitForSeconds(timeDelay);
            }
        }

        private void DespawnEnemy()
        {
            if (HealthComponent.GetCurrentHealth() <= ZERO_HEALTH)
            {
                StopAllCoroutines();
                //Debug.Log("Object Died!: " + transform.parent.gameObject.name);
                EnemySpawner.Instance.DespawnEnemy(transform.parent.gameObject);
                UpdateScore(100);
            }
        }

        private void UpdateScore(int scorePoints)
        {
            ScoreSystem.Instance.UpdateScore(scorePoints);
        }
    }
}