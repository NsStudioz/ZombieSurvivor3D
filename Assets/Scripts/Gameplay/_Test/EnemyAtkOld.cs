using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class EnemyAtkOld : MonoBehaviour
    {
        // EXPERIMENTAL:
        /*        [SerializeField] LayerMask PlayerLayer;
                int playerLayerInt;
                [SerializeField] int rayRange;*/

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


        /*        private void Start()
                {
                    //playerLayerInt = PlayerLayer.value;
                }*/

    }
}
