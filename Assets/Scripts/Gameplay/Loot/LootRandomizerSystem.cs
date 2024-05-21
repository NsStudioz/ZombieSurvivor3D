using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.RNG;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class LootRandomizerSystem : GameListener
    {

        [Header("Weapon/Equipment Loot Lists")]
        [SerializeField] private List<GameObject> CommonLoot;
        [SerializeField] private List<GameObject> UncommonLoot;
        [SerializeField] private List<GameObject> RareLoot;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            EventManager<float>.Register(Events.EventKey.OnRNGLoot.ToString(), CycleThroughLootRarity);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager<float>.Unregister(Events.EventKey.OnRNGLoot.ToString(), CycleThroughLootRarity);
        }

        #endregion

        private void CycleThroughLootRarity(float rnd)
        {
            float value = rnd;
            //Debug.Log("Random Value: " + value);

            if (RNGHelper.IsCommon(value))
            {
                SpawnLoot(CommonLoot);
                //Debug.Log("Common Loot!");
            }
            else if (RNGHelper.IsUncommon(value))
            {
                SpawnLoot(UncommonLoot);
                //Debug.Log("Uncommon Loot!");
            }
            else if (RNGHelper.IsRare(value))
            {
                SpawnLoot(RareLoot);
                //Debug.Log("Rare Loot!");
            }
        }

        private void SpawnLoot(List<GameObject> lootList)
        {
            int rnd = UnityEngine.Random.Range(0, lootList.Count);

            //Debug.Log("Chosen Loot: " + lootList[rnd]);

            EventManager<GameObject>.Raise(Events.EventKey.OnSpawnLoot.ToString(), lootList[rnd]);
        }

    }
}
