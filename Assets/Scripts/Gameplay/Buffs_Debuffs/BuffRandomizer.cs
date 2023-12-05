using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Loot;

namespace ZombieSurvivor3D.Gameplay.BuffsDebuffs
{
    public class BuffRandomizer : MonoBehaviour
    {
        [Header("Buffs Lists")]
        [SerializeField] private List<GameObject> CommonBuffs;
        [SerializeField] private List<GameObject> UncommonBuffs;
        [SerializeField] private List<GameObject> RareBuffs;
        [SerializeField] private List<GameObject> Debuffs;

        [Header("Pity Lock Elements")]
        [SerializeField] private int pityLockCount = 0;
        [SerializeField] private bool isPityLocked = false;

        // Events:
        public static event Action<GameObject,int> OnBuffRolled;

        #region RarityElements:

        // Common First half:
        private int minMinCommon = 1;
        private int maxMinCommon = 35;
        // Uncommon First half:
        private int minMinUncommon = 36;
        private int maxMinUncommon = 46;
        // Rare:
        private int minRare = 47;
        private int maxRare = 53;
        // Uncommon Second half:
        private int minMaxUncommon = 54;
        private int maxMaxUncommon = 64;
        // Common Second half:
        private int minMaxCommon = 65;
        private int maxMaxCommon = 100;

        #endregion

        #region DebuffsElements:

        private int debuffMinInt = 10;
        private int debuffMaxInt = 90;

        #endregion

        #region EventListeners:

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            NumberGenerator.OnRandomNumberGeneratedBuffs += RollRandomBuff;
            NumberGenerator.OnRandomNumberGeneratedBuffs += RollRandomDebuff;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            NumberGenerator.OnRandomNumberGeneratedBuffs -= RollRandomBuff;
            NumberGenerator.OnRandomNumberGeneratedBuffs -= RollRandomDebuff;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        #region RarityHelpers:

        private bool IsCommonRolledPreRareIndex(int value)
        {
            return value >= minMinCommon && value <= maxMinCommon;
        }
        private bool IsCommonRolledPostRareIndex(int value)
        {
            return value >= minMaxCommon && value <= maxMaxCommon;
        }

        private bool IsUncommonRolledPreRareIndex(int value)
        {
            return value >= minMinUncommon && value <= maxMinUncommon;
        }
        private bool IsUncommonRolledPostRareIndex(int value)
        {
            return value >= minMaxUncommon && value <= maxMaxUncommon;
        }

        private bool IsRareRolled(int value)
        {
            return value >= minRare && value <= maxRare;
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
            pityLockCount = UnityEngine.Random.Range(8, 10);
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                NumberGenerator.GenerateForBuffs();
            }
        }

        // Spawn a buff for the player, Currently only spawns 1 buff:
        private void RollRandomBuff(int rnd)
        {
            int value = rnd;

            // Pity Calculation:
            CountLockedPity();

            if (isPityLocked && IsRareRolled(value))
            {
                int nonRareRandomizer = UnityEngine.Random.Range(10, 30);
                value -= nonRareRandomizer;
            }

            Debug.Log("Random Value: " + value);

            // Start Rolling a buff, complete on buffs branch:
            if (IsCommonRolledPreRareIndex(value) || IsCommonRolledPostRareIndex(value))
            {
                // spawn random common buff:
                //RollBuff(CommonBuffs);
                Debug.Log("Spawn Common");
            }
            else if (IsUncommonRolledPreRareIndex(value) || IsUncommonRolledPostRareIndex(value))
            {
                // spawn random Uncommon buff:
                //RollBuff(UncommonBuffs);
                Debug.Log("Spawn Uncommon");
            }
            else if (IsRareRolled(value))
            {
                // spawn random rare buff:
                //RollBuff(RareBuffs);
                Debug.Log("Spawn Rare");
                LockPity();
            }
        }

        private void RollBuff(List<GameObject> buffList)
        {
            int rnd = RollRandomItemFromList(buffList);

            Debug.Log("Chosen Buff: " + buffList[rnd]);

            OnBuffRolled?.Invoke(buffList[rnd], 1);
        }

        private void RollRandomDebuff(int rnd)
        {
            int value = rnd;

            if (value <= debuffMinInt || value >= debuffMaxInt)
            {
                // Spawn Debuff:
                Debug.Log("Spawn Debuff");
                //RollDebuff(Debuffs);
            }
        }

        private void RollDebuff(List<GameObject> buffList)
        {
            int rnd = RollRandomItemFromList(buffList);

            Debug.Log("Chosen Debuff: " + buffList[rnd]);

            OnBuffRolled?.Invoke(buffList[rnd], -1);
        }

        private int RollRandomItemFromList(List<GameObject> buffList)
        {
            return UnityEngine.Random.Range(0, buffList.Count);
        }
    }
}
