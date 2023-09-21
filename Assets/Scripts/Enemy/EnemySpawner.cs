using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Main Elements")]
    [SerializeField] GameObject enemyStandardPrefab;
    [SerializeField] Queue<GameObject> enemyQueue = new Queue<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawning")]
    [SerializeField] int pointerIndex = 0;
    //
    [SerializeField] float timer;
    [SerializeField] float timerThreshold;
    //
    //[SerializeField] int enemyHealth = 100;
    public int EnemyMaxCount { get; private set; }
    [SerializeField] private int enemyMaxCountModifier = 5;
    [SerializeField] private int remainingEnemies = 10;
    [SerializeField] private int enemyCount = 0;

    [Header("Limits")]
    [SerializeField] int minInt;
    [SerializeField] int maxInt;
    [SerializeField] int minFloat;
    [SerializeField] int maxFloat;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Something went wrong. there are more than 1 EnemySpawner in the game!");
            return;
        }
        Instance = this;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        EnemyMaxCount = enemyMaxCountModifier;

        RandomizeEnemySpawn();
    }

    void Update()
    {
        if (remainingEnemies <= 0)
            return;

        if (enemyCount >= EnemyMaxCount)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        ObjectPool.Instance.SpawnObject(enemyQueue, EnemyMaxCount, enemyStandardPrefab, spawnPoints[pointerIndex].transform.position, transform.rotation);

        remainingEnemies--;
        enemyCount++;

        RandomizeEnemySpawn();
    }

    public void DespawnEnemy(GameObject instanceToDespawn)
    {
        if (enemyQueue.Count >= EnemyMaxCount)
            Destroy(instanceToDespawn);

        else if(enemyQueue.Count < EnemyMaxCount)
            ObjectPool.Instance.DespawnObjectDelay(0.2f, enemyQueue, instanceToDespawn, transform.position, transform.rotation);

        enemyCount--;
    }

    private void RandomizeEnemySpawn()
    {
        pointerIndex = Random.Range(minInt, maxInt);
        timer = Random.Range(minFloat, maxFloat);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        ClearQueue();
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void ClearQueue()
    {
        enemyQueue.Clear();
        remainingEnemies = 0;
    }

}