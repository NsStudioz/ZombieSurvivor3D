using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor.ObjectPool.Spawners;
using ZombieSurvivor.Interfaces;

public class BulletDamage : MonoBehaviour
{

    [SerializeField] int bulletDamage;

    private void Start()
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
            //Destroy(gameObject);
            BulletSpawner.Instance.DespawnBullet(gameObject);

/*            var layerMask = col.gameObject.layer;
            LayerMask.LayerToName(layerMask);*/
        }

        BulletSpawner.Instance.DespawnBullet(gameObject);

        //Destroy(gameObject);
        // Play effects on hit maybe... 
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

}
