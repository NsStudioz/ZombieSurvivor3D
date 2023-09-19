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
        RegenerateHealth();
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

    private void RegenerateHealth()
    {
        if (isPlayerHit)
        {
            if (healthComponent.GetCurrentHealth() <= 0)
            {
                //isPlayerHit = false;
                return;
            }

            timeElapsed -= Time.deltaTime;

            if (timeElapsed <= 0)
            {
                healthComponent.SetCurrentHealthToMax();
                isPlayerHit = false;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        healthComponent.TakeDamage(damageAmount);

        // Prototype
        if (gameObject.tag == PLAYER_TAG)
        {
            timeElapsed = timeElapsedThreshold;
            //Debug.Log("Player Health: " + healthComponent.GetCurrentHealth());
            isPlayerHit = true;
        }

        DespawnEnemy();

        if (healthComponent.GetCurrentHealth() <= 0)
        {
            isPlayerHit = false;
            Debug.Log("Object Died!: " + transform.parent.gameObject.name);
        }
    }

    private void DespawnEnemy()
    {
        if(gameObject.tag == ENEMY_TAG && healthComponent.GetCurrentHealth() <= 0)
        {
            EnemySpawner.Instance.ReduceEnemyCount(transform.parent.gameObject);
        }
    }

}
