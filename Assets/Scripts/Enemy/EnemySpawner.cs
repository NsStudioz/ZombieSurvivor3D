using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

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
    [SerializeField] private int enemyCount = 0;

    [Header("Limits")]
    [SerializeField] int minInt;
    [SerializeField] int maxInt;
    [SerializeField] int minFloat;
    [SerializeField] int maxFloat;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        EnemyMaxCount = 5;
        timer = Random.Range(minFloat, maxFloat);
    }


    void Update()
    {
        if (enemyCount >= EnemyMaxCount)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            pointerIndex = Random.Range(minInt, maxInt);
            ObjectPool.Instance.SpawnObject(enemyQueue, EnemyMaxCount, enemyStandardPrefab, spawnPoints[pointerIndex].transform.position, transform.rotation);
            enemyCount++;
            timer = Random.Range(minFloat, maxFloat);
        }
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
        enemyCount = 0;
    }

}