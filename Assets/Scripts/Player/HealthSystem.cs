using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{

    [Header("Main Elements")]
    [SerializeField] HealthComponent healthComponent;

    [Header("Player Elements")]
    [SerializeField] bool isPlayerHit = false;
    [SerializeField] float timeElapsed;
    [SerializeField] float timeElapsedThreshold = 3f;

    [SerializeField] const string ENEMY_TAG = "Enemy";
    [SerializeField] const string PLAYER_TAG = "Player";

    [Header("Enemy Elements")]
    [SerializeField] private EnemyTypes enemyType = EnemyTypes.Weakling;

    public enum EnemyTypes
    {
        Weakling, Elite, Boss
    }

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        if (gameObject.tag == PLAYER_TAG)
            InitializeHealthComponent(3);

        else if (gameObject.tag == ENEMY_TAG)
            SetEnemyTypeHealth();
    }

    private void Update()
    {
        if (gameObject.tag == PLAYER_TAG)
        {
            RegisterPlayerHit();
            RegenerateHealth();
        }

        // Prototype
        if (gameObject.tag == PLAYER_TAG && healthComponent.GetCurrentHealth() <= 0)
            Debug.Log("Player: Died!");

        else if (gameObject.tag == ENEMY_TAG && healthComponent.GetCurrentHealth() <= 0)
            Debug.Log("Enemy: Died!");
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void SetEnemyTypeHealth()
    {
        if (enemyType == EnemyTypes.Weakling)
            InitializeHealthComponent(100);

        if (enemyType == EnemyTypes.Elite)
            InitializeHealthComponent(200);

        if (enemyType == EnemyTypes.Boss)
            InitializeHealthComponent(1000);
    }

    private void InitializeHealthComponent(int health)
    {
        HealthComponent newHealthComp = new HealthComponent(health);
        healthComponent = newHealthComp;
    }

    private void DamageOld(int damage)
    {
        healthComponent.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider enemyCol)
    {
        if (gameObject.tag == PLAYER_TAG && enemyCol.CompareTag(ENEMY_TAG))
        {
            DamageOld(1);
            timeElapsed = timeElapsedThreshold;
            Debug.Log("Player Health: " + healthComponent.GetCurrentHealth());
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

    public void TakeDamage(int damageAmount)
    {
        healthComponent.TakeDamage(damageAmount);
    }
}


/*            if (healthComponent.GetCurrentHealth() <= 0)
                Debug.Log("Player: Died!");  */

/*    private void DamageEnemy()
    {
        if (gameObject.tag == ENEMY_TAG)
        {
            DamageOld(50);
        }
    }*/
