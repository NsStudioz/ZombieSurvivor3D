using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Health;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public class TurretCaliber : MonoBehaviour
    {
        [Header("Targeting")]
        private Transform target = null;

        [Header("Init")]
        public int damage = 50;
        public float rayRange = 0f;

        [Header("Caliber Attributes")]
        public float speed = 10f;
        public float explosionRadius = 0f;

        #region EventListeners:

        private void Start()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            SeekTarget();
        }

        public void AquireTarget(Transform _target)
        {
            target = _target;
        }

        private void SeekTarget()
        {
            Vector3 dir = target.position - transform.position;

            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            // Move towards the target:
            transform.Translate(dir.normalized * distanceThisFrame, Space.World); // move at a constant speed by using normalized function, while at world space.
            transform.LookAt(target); // point towards the target.
        }

        private void HitTarget()
        {
            if (explosionRadius <= 0f)
                Damage(); // damage a single target.
            else
                Explode(); // Damage multiple targets.

            Destroy(gameObject);
        }

        private void Damage()
        {
            IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
            ApplyDamage(damageable);
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); // shoot out a sphere, as an explosion. We'll see what it hits.

            foreach (Collider affectedCol in colliders) // loop through all of the things it hits.
            {
                if (affectedCol.tag == "Enemy") // if the colliders are tagged as Enemy.
                {
                    IDamageable damageable = affectedCol.GetComponent<IDamageable>();
                    ApplyDamage(damageable);
                }
            }
        }

        private void ApplyDamage(IDamageable damageable)
        {
            damageable?.TakeDamage(damage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
