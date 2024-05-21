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