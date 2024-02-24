using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Health;

namespace ZombieSurvivor3D
{
    public class Turret : MonoBehaviour
    {

        [Header("Activation")]
        [SerializeField] private bool isEnabled = false;

        [Header("Main Attributes")]
        [SerializeField] private Transform target = null;         // for targeting the enemy
        [SerializeField] private Transform partToRotate;
        [SerializeField] private string targetTag;

        // for accessing the enemy's health component. Might be for future use:
        //[SerializeField] private EnemyHealth EnemyTarget;  

        [Header("Attack Attributes")]
        [SerializeField] private float damage = 100f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float fireCooldownThreshold = 0f;

        [Header("Defense Attributes")]
        [SerializeField] private float range = 1f;
        [SerializeField] private float turnSpeed = 1f;

        //[Header("Bullet Setup")]
        //[SerializeField] private GameObject bulletPrefab;
        //[SerializeField] private Transform firingPosition;

        [Header("Targetting frequency")]
        [SerializeField] private float repeatRate = 0.5f;

        private const float ZEROED_VALUE = 0f;

        void Start()
        {
            // test:
            Activate();
            //InvokeRepeating(nameof(AquireTarget), ZEROED_VALUE, repeatRate);
            InvokeRepeating("AquireTarget", ZEROED_VALUE, repeatRate);
        }

        void Update()
        {
            if (!isEnabled)
                return;

            if (target == null)
                return;

            LockRotation();
            //AquireTarget();
        }

        /// <summary>
        /// Find a target in the vicinity.
        /// </summary>
        private void AquireTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else
                target = null;
        }

        /// <summary>
        /// Rotate turret towards the target's position.
        /// </summary>
        private void LockRotation()
        {
            Vector3 dir = target.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // eulerAngles = converts the below Quaternion into Vector3:
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            // Rotate the turret:
            partToRotate.rotation = Quaternion.Euler(ZEROED_VALUE, rotation.y, ZEROED_VALUE);
        }

        private void Activate()
        {
            isEnabled = true;
        }

        private void Deactivate()
        {
            isEnabled = false;
        }


        private void OnDestroy()
        {
            if (isEnabled)
                Deactivate();
        }

    }
}
