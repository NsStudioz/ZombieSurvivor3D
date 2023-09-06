using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Main Elements")]
    [SerializeField] HealthComponent healthComponent;

    [Header("Player Elements")]
    [SerializeField] bool isPlayerHit = false;
    [SerializeField] float timeElapsed;
    [SerializeField] float timeElapsedThreshold = 3f;

    [SerializeField] const string ENEMY_TAG = "Enemy";
    [SerializeField] const string PLAYER_TAG = "Player";

    private void Start()
    {
        if (gameObject.tag == PLAYER_TAG)
        {
            HealthComponent playerHealthComp = new HealthComponent(3);
            healthComponent = playerHealthComp;
        }
        else if (gameObject.tag == ENEMY_TAG)
        {
            HealthComponent enemyHealthComp = new HealthComponent(100);
            healthComponent = enemyHealthComp;
        }
    }

    private void Update()
    {
        RegisterPlayerHit();
        RegenerateHealth();
    }

    private void TakeDamage(int damage)
    {
        healthComponent.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider enemyCol)
    {
        if (gameObject.tag == PLAYER_TAG && enemyCol.CompareTag(ENEMY_TAG))
        {
            TakeDamage(1);
            timeElapsed = timeElapsedThreshold;
            Debug.Log("Player Health: " + healthComponent.GetCurrentHealth());

            if (healthComponent.GetCurrentHealth() <= 0)
                Debug.Log("Player: Died!");  
        }
    }

    private void RegisterPlayerHit()
    {
        if (healthComponent.GetCurrentHealth() < healthComponent.GetMaxHealth())
            isPlayerHit = true;
    }

    private void RegenerateHealth()
    {
        if (healthComponent.GetCurrentHealth() <= 0)
            return;

        if (isPlayerHit)
            timeElapsed -= Time.deltaTime;

        if (timeElapsed <= 0)
        {
            healthComponent.SetCurrentHealthToMax();
            isPlayerHit = false;
        }
    }

}
