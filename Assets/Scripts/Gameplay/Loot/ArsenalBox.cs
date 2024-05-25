using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Items;
using ZombieSurvivor3D.Gameplay.RNG;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class ArsenalBox : GameListener
    {
        // when project finishes, remove this injection and make a proper value:
        [Header("Points Cost")]
        [SerializeField] private int pointsCost;

        [Header("Main Attributes")]
        [SerializeField] private GameObject LootInstance = null;
        [SerializeField] private Transform LootPosOffset;

        [Header("Activity")]
        [SerializeField] private float timeElapsed;
        [SerializeField] private float timeElapsedThreshold;
        [SerializeField] private bool isInteracted;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            EventManager<GameObject>.Register(Events.GameplayRNG.OnSpawnLoot.ToString(), SpawnChosenLoot);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager<GameObject>.Unregister(Events.GameplayRNG.OnSpawnLoot.ToString(), SpawnChosenLoot);
        }

        #endregion

        public int GetPointsCost()
        {
            return pointsCost;
        }

        /// <summary>
        /// When you take a weapon/equipment spawned from the box, it will reset the box and loot leftovers
        /// </summary>
        public void RemoveLootFromBox()
        {
            if (LootInstance != null)
                StartCoroutine(CountDownToResetLoot());
        }

        /// <summary>
        /// Re-test this function's WaitForSeconds function on a 144hz production build.
        /// On player interaction, should Remove the loot item without respawning an item again until the next interaction input.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CountDownToResetLoot()
        {
            yield return new WaitForSeconds(0.1f);
            ResetBoxInteractionState();
            ResetLootState();
        }

        private void InteractThisBox()
        {
            isInteracted = true;
        }

        public void OpenArsenalBoxForLoot()
        {
            if (isInteracted)
                return;

            InteractThisBox();
            NumberGenerator.GenerateForLoot();
        }

        private void Update()
        {
            if (LootInstance == null)
                return;

            if (timeElapsed > 0)
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
            LootInstance.GetComponentInChildren<ItemColliderMover>().Move();
            LootInstance = null;
            timeElapsed = timeElapsedThreshold;
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
