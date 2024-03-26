using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Health;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public class Fence : TrapBase
    {

        //[SerializeField] private GameObject fenceGO;
        //[SerializeField] private BoxCollider boxCol;
        //[SerializeField] private int damage = 1;

        [Header("Main Attributes")]
        [SerializeField] private string targetTag;
        [SerializeField] float damageOverTimeDelay;

        private void OnTriggerEnter(Collider col)
        {
            if (!isActivated)
                return;

            IgnoreNonTarget(col);
            Damage(col);         
        }

        private void OnTriggerExit(Collider col)
        {
            if (!isActivated)
                return;

            IgnoreNonTarget(col);
            StopContinuousDamage(col);
        }

        private void IgnoreNonTarget(Collider col)
        {
            if (col.CompareTag(targetTag) || col == null)
                return;
        }

        private void Damage(Collider col)
        {
            // Damage collider target over time:
            //IDamageable damageable = col.GetComponentInChildren<IDamageable>();
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.TakeContinuousDamage(damage, damageOverTimeDelay);
        }

        private void StopContinuousDamage(Collider col)
        {
            // Stop damage over time effect:
            //IDamageable damageable = col.GetComponentInChildren<IDamageable>();
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.StopContinuousDamage();
        }

    }
}
