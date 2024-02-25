using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Bullets;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Health;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public class Turret : TrapBase
    {

        [Header("Activation")]
        //[SerializeField] private bool isEnabled = false;
        [SerializeField] private bool hasCaliber = false;

        [Header("Main Attributes")]
        [SerializeField] private Transform target = null;  // for targeting the enemy
        [SerializeField] private Transform partToRotate;
        [SerializeField] private string targetTag;

        [Header("Attack Attributes")]
        [SerializeField] private int damage = 100;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float fireCooldown = 0f;

        [Header("Defense Attributes")]
        [SerializeField] private float range = 1f;
        [SerializeField] private float rayHitRange = 20f;
        [SerializeField] private float turnSpeed = 1f;

        [Header("Bullet Setup")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private Transform bulletStorage; // used bullet storage.

        [Header("Targetting frequency")]
        [SerializeField] private float repeatRate = 0.5f;

        private const float ZEROED_VALUE = 0f;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion

        void Start()
        {
            // test:
            Activate();
            //
            InvokeRepeating(nameof(AquireTarget), ZEROED_VALUE, repeatRate);
        }

        void Update()
        {
            if (!isActivated)
                return;

            if (target == null)
                return;

            LockRotation();

            OpenFire();
        }

        #region Locking & Rotating Systems:

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
            Vector3 dir = target.position - partToRotate.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // eulerAngles = converts the below Quaternion into Vector3:
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            // Rotate the turret:
            partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, ZEROED_VALUE);

            //Backup:
            //  Vector3 dir = target.position - transform.position;
            //partToRotate.rotation = Quaternion.Euler(ZEROED_VALUE, rotation.y, ZEROED_VALUE);
        }

        #endregion

        /// <summary>
        /// Fire at the target.
        /// </summary>
        private void OpenFire()
        {
            if (fireCooldown <= ZEROED_VALUE)
            {
                if (!hasCaliber)
                    Fire();
                else
                    FireCaliber();

                fireCooldown = 1f / fireRate;
            }

            fireCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// Fire at a designated target using ray-casting.
        /// </summary>
        private void Fire()
        {
            // ray instance:
            Ray ray = new Ray(partToRotate.position, partToRotate.TransformDirection(Vector3.forward));

            // if the ray hits nothing:
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, rayHitRange))
            {
                //Debug.Log("Nothing");
                Debug.DrawRay(partToRotate.position, partToRotate.TransformDirection(Vector3.forward) * rayHitRange, Color.green);
                return;
            }
            else // if the ray hits something:
            {
                //Debug.Log("Hit");
                Debug.DrawRay(partToRotate.position, partToRotate.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
                // hit enemy:
                IDamageable damageable = hitInfo.collider.GetComponentInChildren<IDamageable>();
                damageable?.TakeDamage(damage);
            }
        }

        /// <summary>
        /// Release a caliber from turret's chamber.
        /// </summary>
        private void FireCaliber()
        {
            GameObject caliberGO = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            caliberGO.transform.parent = bulletStorage;
            TriggerCaliber(caliberGO, target);
        }

        /// <summary>
        /// Force a turret's caliber object to seek target.
        /// </summary>
        private void TriggerCaliber(GameObject caliberIns, Transform target)
        {
            TurretCaliber newCaliber = caliberIns.GetComponent<TurretCaliber>();

            if (newCaliber != null)
                newCaliber.AquireTarget(target);


        }

        #region OldCode_RemoveSoon:

        //[SerializeField] private float activeDuration;


        //StartCoroutine(TimeToFade(caliberGO));

        /*        /// <summary>
                /// Set timer to "Destroy" bullet instance object in the scene.
                /// </summary>
                /// <param name="bulletInstance"></param>
                /// <returns></returns>
                private IEnumerator TimeToFade(GameObject bulletInstance)
                {
                    yield return new WaitForSecondsRealtime(activeDuration);
                    Destroy(bulletInstance);
                }*/

        /*        private void ActivateOld()
                {
                    isEnabled = true;
                }

                private void DeactivateOld()
                {
                    isEnabled = false;
                }*/


        #endregion

    }
}
