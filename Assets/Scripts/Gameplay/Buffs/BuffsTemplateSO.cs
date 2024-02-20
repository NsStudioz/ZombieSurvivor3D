using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    /// <summary>
    /// Buff Blueprint as a Scriptable Object. The base template for creating a buff item.
    /// </summary>
    public abstract class BuffsTemplateSO : ScriptableObject
    {

        [Header("Identification")]
        public Category category = Category.None;
        public string buffTagId;

        [Header("UI Elements")]
        public string buffName;
        public string buffDescription;
        public string buffDurationUI;
        public string buffAmountUI;
        public Sprite buffSprite = null; // not sure if should be sprite or normal UI texture...

        [Header("Stats")]
        public Rarity rarity = Rarity.None;
        public StatType statType = StatType.None;
        public float buffDuration; // tie it with buffDurationUI field;
        public float amount;

        public enum Category
        {
            None ,Buff, Debuff
        }

        public enum Rarity
        {
            None, Common, Uncommon, Rare
        }

        public enum StatType
        {
            None, Player, Handheld, Money, Traps, Enemies, Abilities, WaveSpawner
        }

        public abstract void Apply();
    }
}

//[CreateAssetMenu(fileName = "Buffs", menuName = "New Buff SO", order = 1)]
//[Header("Duration")]
//public float bonusToStats;
//public float nerfToStats;
//public Image buffImage = null; // not sure if should be sprite or normal UI texture...