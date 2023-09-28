using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Health;


namespace ZombieSurvivor3D.Gameplay.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        int damageToPlayer = 1;

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }


        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                IDamageable damageableNew = col.GetComponent<IDamageable>();
                damageableNew?.TakeDamage(damageToPlayer);
                //Debug.Log("Player Hit");
            }
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }
    }
}
