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
            InitializeHealthComponent(3);

        else if (gameObject.tag == ENEMY_TAG)
            InitializeHealthComponent(100);
    }

    private void Update()
    {
        RegisterPlayerHit();
        RegenerateHealth();

        // PROTOTYPE:
        if (Input.GetKeyDown(KeyCode.M))
            DamageEnemy();


        if (gameObject.tag == ENEMY_TAG)
            if (healthComponent.GetCurrentHealth() <= 0)
                Destroy(gameObject);
        //---------------------------------------------------
    }

    private void InitializeHealthComponent(int health)
    {
        HealthComponent newHealthComp = new HealthComponent(health);
        healthComponent = newHealthComp;
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

    private void DamageEnemy()
    {
        if (gameObject.tag == ENEMY_TAG)
        {
            TakeDamage(50);
        }
    }
}
