using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Health;


namespace ZombieSurvivor3D.Gameplay.Enemy
{
    public class EnemyAttack : GameListener
    {
        int damageToPlayer = 1;
        const string PLAYER_TAG = "Player";

        // EXPERIMENTAL:
        /*        [SerializeField] LayerMask PlayerLayer;
                int playerLayerInt;
                [SerializeField] int rayRange;*/


        private void Start()
        {
            //playerLayerInt = PlayerLayer.value;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag(PLAYER_TAG))
            {
                IDamageable damageableNew = col.GetComponent<IDamageable>();
                damageableNew?.TakeDamage(damageToPlayer);
                //Debug.Log("Player Hit");
            }
        }
    }
}

/*        // EXPERIMENTAL:
        private void Update()
        {
            SimulateRayCast();
        }

        // EXPERIMENTAL:
        private void SimulateRayCast()
        {
            Vector3 direction = Vector3.forward;
            Ray newRay = new Ray(transform.position, transform.TransformDirection(direction * rayRange));

            //Debug.DrawRay(transform.position, transform.TransformDirection(direction * rayRange), Color.green);

            if (Physics.Raycast(newRay, out RaycastHit hit, rayRange, playerLayerInt))
            {
                if (hit.collider.CompareTag(PLAYER_TAG))
                {
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    damageable?.TakeDamage(damageToPlayer);
                }
            }
        }*/