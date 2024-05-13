using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.RNG;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    /// <summary>
    /// Grants a randomized buff or debuff, then passes the stats to a buff UI object.
    /// </summary>
    public class BuffRandomizer : GameListener
    {
        [Header("Buffs Lists")]
        [SerializeField] private List<BuffsTemplateSO> CommonBuffs;    // Contains buffs & Debuffs
        [SerializeField] private List<BuffsTemplateSO> UncommonBuffs;  // Contains buffs & Debuffs
        [SerializeField] private List<BuffsTemplateSO> RareBuffs;      // Contains buffs only

        [Header("Pity Lock Elements")]
        [SerializeField] private int pityLockCount = 0;
        [SerializeField] private bool isPityLocked = false;

        // Events:
        public static event Action<BuffsTemplateSO> OnBuffRolled;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            NumberGenerator.OnRandomNumberGeneratedBuffs += RollRandomBuff;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            NumberGenerator.OnRandomNumberGeneratedBuffs -= RollRandomBuff;
        }

        #endregion

        #region PitySystem:

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
            pityLockCount = UnityEngine.Random.Range(5, 6);
        }

        #endregion

        // Roll 1 buff for the player:
        private void RollRandomBuff(float rnd)
        {
            float value = rnd;

            // Pity Calculation:
            CountLockedPity();

            if (isPityLocked && RNGHelper.IsRare(value))
            {
                int nonRareRandomizer = UnityEngine.Random.Range(10, 30);
                value -= nonRareRandomizer;
            }
            //Debug.Log("Random Value: " + value);

            if (RNGHelper.IsCommon(value))
            {
                // spawn random common buff:
                RollBuff(CommonBuffs);
                Debug.Log("Spawn Common Buff");
            }
            else if (RNGHelper.IsUncommon(value))
            {
                // spawn random Uncommon buff:
                RollBuff(UncommonBuffs);
                Debug.Log("Spawn Uncommon Buff");
            }
            else if (RNGHelper.IsRare(value))
            {
                // spawn random rare buff:
                RollBuff(RareBuffs);
                Debug.Log("Spawn Rare Buff");
                // lock rare buffs temporarily:
                LockPity();
            }
        }

        private int RollRandomItemFromList(List<BuffsTemplateSO> buffList)
        {
            return UnityEngine.Random.Range(0, buffList.Count);
        }

        // spawn the desired buff:
        private void RollBuff(List<BuffsTemplateSO> buffList)
        {
            int rnd = RollRandomItemFromList(buffList);

            Debug.Log("Chosen Buff: " + buffList[rnd]);
            
            OnBuffRolled?.Invoke(buffList[rnd]); // Listener = BuffCardUI.cs
        }
    }
}
