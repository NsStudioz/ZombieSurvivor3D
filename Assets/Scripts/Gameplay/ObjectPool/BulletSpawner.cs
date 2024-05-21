using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Handheld;


namespace ZombieSurvivor3D.Gameplay.ObjectPool
{
    public class BulletSpawner : GameListener
    {
        public static BulletSpawner Instance;

        [Header("List")]
        [SerializeField] private Queue<GameObject> bulletQueue = new Queue<GameObject>();

        [Header("Atrributes")]
        [SerializeField] private GameObject bulletPrefab = null;
        [SerializeField] private int bulletMaxReservedCount = 5;

        #region EventListeners:

        protected override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Something went wrong. there are more than 1 buildmanagers in the game!");
                return;
            }
            Instance = this;

            base.Awake();
            EventManager<GameObject>.Register(Events.EventKey.OnHandheldChanged.ToString(),SwitchBulletType);
        }

        protected override void OnDestroy()
        {
            ClearQueue();
            base.OnDestroy();
            EventManager<GameObject>.Unregister(Events.EventKey.OnHandheldChanged.ToString(),SwitchBulletType);
        }

        #endregion

        private void ClearQueue()
        {
            bulletQueue.Clear();
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

    }
}

