using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class LootRandomizerSystem : MonoBehaviour
    {

        [Header("Weapon/Equipment Loot Lists")]
        [SerializeField] private List<GameObject> CommonLoot;
        [SerializeField] private List<GameObject> UncommonLoot;
        [SerializeField] private List<GameObject> RareLoot;

        // Events:
        public static event Action<GameObject> OnSpawnLoot;

        #region EventListeners:

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            NumberGenerator.OnRandomNumberGeneratedLoot += CycleThroughLootRarity;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            NumberGenerator.OnRandomNumberGeneratedLoot -= CycleThroughLootRarity;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        private void CycleThroughLootRarity(float rnd)
        {
            float value = rnd;
            //Debug.Log("Random Value: " + value);

            if (Randomizer.IsCommon(value))
            {
                SpawnLoot(CommonLoot);
                Debug.Log("Common Loot!");
            }
            else if (Randomizer.IsUncommon(value))
            {
                SpawnLoot(UncommonLoot);
                Debug.Log("Uncommon Loot!");
            }
            else if (Randomizer.IsRare(value))
            {
                SpawnLoot(RareLoot);
                Debug.Log("Rare Loot!");
            }
        }

        private void SpawnLoot(List<GameObject> lootList)
        {
            int rnd = UnityEngine.Random.Range(0, lootList.Count);

            Debug.Log("Chosen Loot: " + lootList[rnd]);

            OnSpawnLoot?.Invoke(lootList[rnd]);
        }

    }
}
