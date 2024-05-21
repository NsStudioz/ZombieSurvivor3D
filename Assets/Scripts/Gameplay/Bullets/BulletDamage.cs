using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.ObjectPool;
using ZombieSurvivor3D.Gameplay.Health;


namespace ZombieSurvivor3D.Gameplay.Bullets
{
    public class BulletDamage : GameListener
    {

        [Header("Attributes")]
        [SerializeField] int bulletDamage;
        [SerializeField] int rayRange;

        [Header("Layer Collisions")]
        [SerializeField] LayerMask EnemyLayer;
        [SerializeField] LayerMask GroundLayer;
        int enemyLayerInt;
        int groundLayerInt;

        const string ENEMY_TAG = "Enemy";
        const string GROUND_TAG = "Ground";

        protected override void Awake()
        {
            base.Awake();
            //
            enemyLayerInt = EnemyLayer;
            groundLayerInt = GroundLayer;
        }

        private void Update()
        {
            SimulateRayCast();
        }

        private void SimulateRayCast()
        {
            Vector3 direction = Vector3.forward;
            Ray newRay = new Ray(transform.position, transform.TransformDirection(direction * rayRange));

            //Debug.DrawRay(transform.position, transform.TransformDirection(direction * rayRange), Color.green);

            if (Physics.Raycast(newRay, out RaycastHit hit, rayRange, enemyLayerInt))
            {
                if (hit.collider.CompareTag(ENEMY_TAG))
                {
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    damageable?.TakeDamage(bulletDamage);
                    BulletSpawner.Instance.DespawnBullet(gameObject);
                }
            }
            else if(Physics.Raycast(newRay, out RaycastHit otherHit, rayRange, groundLayerInt))
            {
                if (otherHit.collider.CompareTag(GROUND_TAG))
                    BulletSpawner.Instance.DespawnBullet(gameObject);
            }
        }
    }
}