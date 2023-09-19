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
    //[SerializeField] int enemyHealth = 0;
    //[SerializeField] int enemyMaxCount = 5;

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
        enemyQueue.Clear();
        timer = Random.Range(minFloat, maxFloat);
    }


    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            pointerIndex = Random.Range(minInt, maxInt);
            GameObject newEnemyInstance = Instantiate(enemyStandardPrefab, spawnPoints[pointerIndex].transform.position, transform.rotation);
            timer = Random.Range(minFloat, maxFloat);
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }


    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

}
