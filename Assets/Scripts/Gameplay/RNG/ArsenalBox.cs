using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class ArsenalBox : MonoBehaviour
    {
        // when project finishes, remove this injection and make a proper value:
        [SerializeField] private int pointsCost;

        [SerializeField] private GameObject LootInstance = null;
        [SerializeField] private Transform LootPosOffset;

        [SerializeField] private float timeElapsed;
        [SerializeField] private float timeElapsedThreshold;
        [SerializeField] private bool isInteracted;

        private void Awake()
        {
            LootRandomizerSystem.OnSpawnLoot += SpawnChosenLoot;
        }

        private void OnDestroy()
        {
            LootRandomizerSystem.OnSpawnLoot -= SpawnChosenLoot;
        }

        public int GetPointsCost()
        {
            return pointsCost;
        }

        public void OpenArsenalBoxForLoot()
        {
            if (isInteracted)
                return;

            InteractThisBox();
            NumberGenerator.Generate();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                OpenArsenalBoxForLoot();
            }

            if (LootInstance == null)
                return;

            timeElapsed -= Time.deltaTime;

            if(timeElapsed <= 0)
            {
                ResetLootState();
                ResetBoxInteractionState();
            }
        }

        /// <summary>
        /// Remove loot from the box and reset timer.
        /// </summary>
        private void ResetLootState()
        {
            Destroy(LootInstance);
            LootInstance = null;
            timeElapsed = timeElapsedThreshold;
        }

        private void InteractThisBox()
        {
            isInteracted = true;
        }

        private void ResetBoxInteractionState()
        {
            isInteracted = false;
        }

        private void SpawnChosenLoot(GameObject lootGameObject)
        {
            if (!isInteracted)
                return;

            GameObject lootInstance = Instantiate(lootGameObject, LootPosOffset.transform.position, LootPosOffset.transform.rotation);
            LootInstance = lootInstance;
        }

    }
}
