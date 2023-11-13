using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D
{
    public class LootRandomizerSystem : MonoBehaviour
    {

        [Header("Weapon/Equipment Loot Lists")]
        [SerializeField] private List<GameObject> CommonLoot;
        [SerializeField] private List<GameObject> UncommonLoot;
        [SerializeField] private List<GameObject> RareLoot;

        [Header("Pity Lock Elements")]
        [SerializeField] private int pityLockCount = 0;
        [SerializeField] private bool isPityLocked = false;

        #region RarityElements:

        private int minCommon = 1;
        private int maxCommon = 60;
        private int minUncommon = 61;
        private int maxUncommon = 90;
        private int minRare = 91;
        private int maxRare = 100;

        #endregion

        #region EventListeners:

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            NumberGenerator.OnRandomNumberGenerated += CycleThroughLootRarity;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            NumberGenerator.OnRandomNumberGenerated -= CycleThroughLootRarity;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartLootRandomizer();
            }
        }

        private void StartLootRandomizer()
        {
            NumberGenerator.Generate();
        }

        private void CycleThroughLootRarity(int rnd)
        {
            int value = rnd;
            CountLockedPity();
            Debug.Log("Random Value: " + value);

            if (isPityLocked && value >= minRare)
            {
                int nonRareRandomizer = UnityEngine.Random.Range(10, 50);
                value -= nonRareRandomizer;
            }

            if (value >= minCommon && value <= maxCommon)
            {
                // COMMON loot
                SpawnLoot(CommonLoot);
            }
            else if (value >= minUncommon && value <= maxUncommon)
            {
                // UNCOMMON loot
                SpawnLoot(UncommonLoot);
            }
            else if (value >= minRare && value <= maxRare)
            {
                // RARE loot
                SpawnLoot(RareLoot);
                LockPity();
                Debug.Log("You got a rare item! locking pity for the following duration: " + pityLockCount);
            }
        }

        private void SpawnLoot(List<GameObject> lootList)
        {
            int rnd = UnityEngine.Random.Range(0, lootList.Count);

            Debug.Log("Chosen Loot: " + lootList[rnd]);

            //GameObject lootInstance = Instantiate(lootList[rnd], transform.position, transform.rotation);
        }

        private void CountLockedPity()
        {
            if (!isPityLocked)
                return;

            pityLockCount--;
            Debug.Log("Pity Lock Count: " + pityLockCount);

            if (pityLockCount <= 0)
                isPityLocked = false;
        }

        private void LockPity()
        {
            isPityLocked = true;
            pityLockCount = UnityEngine.Random.Range(3, 5);
        }


    }
}
