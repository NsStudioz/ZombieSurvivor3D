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
        [SerializeField] private float fireCooldown = 0f;

        [Header("Defense Attributes")]
        [SerializeField] private float range = 1f;
        [SerializeField] private float turnSpeed = 1f;

        [Header("Bullet Setup")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private Transform bulletStorage; // used bullet storage.
        [SerializeField] private float activeDuration;

        [Header("Targetting frequency")]
        [SerializeField] private float repeatRate = 0.5f;

        private const float ZEROED_VALUE = 0f;

        void Start()
        {
            // test:
            Activate();
            //
            InvokeRepeating(nameof(AquireTarget), ZEROED_VALUE, repeatRate);
        }

        void Update()
        {
            if (!isEnabled)
                return;

            if (target == null)
                return;

            LockRotation();

            SetRateofFire();
        }

        /// <summary>
        /// Find a target in the vicinity.
        /// </summary>
        private void AquireTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            float shortestDistance = Mathf.Infinity;
            GameObject nearestTarget = null;

            foreach (GameObject targetIns in targets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetIns.transform.position);

                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    nearestTarget = targetIns;
                }
            }

            if (nearestTarget != null && shortestDistance <= range)
            {
                target = nearestTarget.transform;
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

        /// <summary>
        /// Release bullet from turret's chamber.
        /// </summary>
        private void Fire()
        {
            GameObject bulletIns = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            bulletIns.transform.parent = bulletStorage;
            StartCoroutine(TimeToFade(bulletIns));
        }

        /// <summary>
        /// Open Fire at the target.
        /// </summary>
        private void SetRateofFire()
        {
            if (fireCooldown <= ZEROED_VALUE)
            {
                Fire();
                fireCooldown = 1f / fireRate;
            }

            fireCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// Set timer to "Destroy" bullet instance object in the scene.
        /// </summary>
        /// <param name="bulletInstance"></param>
        /// <returns></returns>
        private IEnumerator TimeToFade(GameObject bulletInstance)
        {
            yield return new WaitForSecondsRealtime(activeDuration);
            Destroy(bulletInstance);
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
