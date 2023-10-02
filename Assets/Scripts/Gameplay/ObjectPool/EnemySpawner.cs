using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.ObjectPool
{
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


        #region Monobehavior Methods:

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

            InitializeEnemiesInSpawner();

            RandomizeEnemySpawn();
        }

        private void InitializeEnemiesInSpawner()
        {
            for (int i = 0; i < EnemyMaxCount + 1; i++)
            {
                ObjectPool.Instance.SpawnAndReserveObjectInPool(enemyQueue, enemyStandardPrefab, transform.position, transform.rotation, transform);
            }
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

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            ClearQueue();
        }

        #endregion

        private void SpawnEnemy()
        {
            ObjectPool.Instance.SpawnObject(enemyQueue, enemyStandardPrefab, spawnPoints[pointerIndex].transform.position, transform.rotation, transform);

            remainingEnemies--;
            enemyCount++;

            RandomizeEnemySpawn();
        }

        public void DespawnEnemy(GameObject instanceToDespawn)
        {
            if (enemyQueue.Count >= EnemyMaxCount)
                Destroy(instanceToDespawn);

            else if (enemyQueue.Count < EnemyMaxCount)
            {
                ObjectPool.Instance.DespawnObjectImmediately(enemyQueue, instanceToDespawn, transform.position, transform.rotation);
                //ObjectPool.Instance.DespawnObjectDelay(0.2f, enemyQueue, instanceToDespawn, transform.position, transform.rotation);
            }

            enemyCount--;
        }

        private void RandomizeEnemySpawn()
        {
            pointerIndex = Random.Range(minInt, maxInt);
            timer = Random.Range(minFloat, maxFloat);
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
}