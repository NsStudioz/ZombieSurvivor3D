using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{

    [SerializeField] int bulletDamage;

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
        if (col.CompareTag("Enemy"))
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.TakeDamage(bulletDamage); // if damageable is not null...Then Damage
            //Debug.Log("Enemy Hit!");
            Destroy(gameObject);
        }

        //Destroy(gameObject);
        // Play effects on hit maybe... 
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

}
