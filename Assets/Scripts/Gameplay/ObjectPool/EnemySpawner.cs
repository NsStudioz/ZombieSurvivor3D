using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.ObjectPool
{
    public class EnemySpawner : GameListener
    {
        public static EnemySpawner Instance;

        [Header("Lists")]
        [SerializeField] Queue<GameObject> enemyQueue = new Queue<GameObject>();
        [SerializeField] List<Transform> spawnPoints = new List<Transform>();

        [Header("Main Elements")]
        [SerializeField] GameObject enemyStandardPrefab;
        [SerializeField] Transform parentTransform;

        [Header("Spawning")]
        [SerializeField] int pointerIndex = 0;

        [Header("Timers")]
        [SerializeField] float timer;
        [SerializeField] float timerThreshold;

        [Header("Enemy Counting")]
        [SerializeField] int enemyMaxCountModifier = 5;
        [SerializeField] int remainingEnemies = 10;
        [SerializeField] int enemyCount = 0;
        
        public int EnemyMaxCount { get; private set; }
        //[SerializeField] int enemyHealth = 100;

        [Header("Limits")]
        [SerializeField] int minInt;
        [SerializeField] int maxInt;
        [SerializeField] int minFloat;
        [SerializeField] int maxFloat;


        protected override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Something went wrong. there are more than 1 EnemySpawner in the game!");
                return;
            }
            Instance = this;

            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearQueue();
        }

        void Start()
        {
            EnemyMaxCount = enemyMaxCountModifier;

            InitializeEnemiesInSpawner();

            RandomizeEnemySpawn();
        }

        private void ClearQueue()
        {
            enemyQueue.Clear();
            remainingEnemies = 0;
        }

        private void InitializeEnemiesInSpawner()
        {
            for (int i = 0; i < EnemyMaxCount + 1; i++)
            {
                ObjectPool.Instance.SpawnAndReserveObjectInPool(
                                enemyQueue, enemyStandardPrefab, 
                                transform.position, transform.rotation, 
                                parentTransform);
            }
        }

        private void RandomizeEnemySpawn()
        {
            pointerIndex = Random.Range(minInt, maxInt);
            timer = Random.Range(minFloat, maxFloat);
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
            ObjectPool.Instance.SpawnObject(enemyQueue, enemyStandardPrefab, spawnPoints[pointerIndex].transform.position, transform.rotation, parentTransform);

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
    }
}