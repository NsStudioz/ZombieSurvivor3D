using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    [CreateAssetMenu(fileName = "Buffs", menuName = "New Buff SO", order = 1)]
    public class BuffsSO : ScriptableObject
    {

        [Header("Identification")]
        public Category category = Category.None;
        public string buffTagId;

        [Header("UI Elements")]
        public string buffName;
        public string buffDescription;
        public string buffDurationUI;
        public Image buffImage; // not sure if should be sprite or normal UI texture...

        [Header("Duration")]
        public float buffDuration; // tie it with buffDurationUI field;

        [Header("Stats")]
        public StatType statType = StatType.None;
        public float bonusToStats;
        public float nerfToStats;

        public enum Category
        {
            None ,Buff, Debuff
        }

        public enum StatType
        {
            None, Player, Handheld, Money, Traps, Enemies, Abilities, WaveSpawner
        }
    }
}
