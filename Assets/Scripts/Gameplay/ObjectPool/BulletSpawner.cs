using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Handheld;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.ObjectPool
{
    public class BulletSpawner : MonoBehaviour
    {
        public static BulletSpawner Instance;

        [SerializeField] private Queue<GameObject> bulletQueue = new Queue<GameObject>();

        [SerializeField] private GameObject bulletPrefab = null;

        [SerializeField] private int bulletMaxReservedCount = 5;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Something went wrong. there are more than 1 buildmanagers in the game!");
                return;
            }
            Instance = this;

            HandheldCarrier.OnHandheldChanged += SwitchBulletType;

            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnEnable()
        {
            HandheldCarrier.OnHandheldChanged += SwitchBulletType;
        }

        private void OnDisable()
        {
            HandheldCarrier.OnHandheldChanged -= SwitchBulletType;
        }

        public void SpawnBullet(Vector3 position, Quaternion rotation)
        {
            ObjectPool.Instance.SpawnObject(bulletQueue, bulletPrefab, position, rotation, transform);
        }

        public void DespawnBullet(GameObject instanceToDespawn)
        {
            ObjectPool.Instance.DespawnObjectImmediately(bulletQueue, instanceToDespawn, transform.position, transform.rotation);
        }

        private void SwitchBulletType(GameObject newBulletType)
        {
            if (bulletPrefab == newBulletType)
                return;

            bulletPrefab = newBulletType;

            for (int i = bulletQueue.Count; i > 0; i--)
            {
                GameObject instanceToDestroy = bulletQueue.Dequeue();
                Destroy(instanceToDestroy);

                if (i <= 0)
                    ClearQueue();
            }

            // EXPERIMENTAL:
            for (int i = 0; i < bulletMaxReservedCount + 1; i++)
            {
                ObjectPool.Instance.SpawnAndReserveObjectInPool(bulletQueue, bulletPrefab, transform.position, transform.rotation, transform);
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
            bulletQueue.Clear();
        }
    }
}

