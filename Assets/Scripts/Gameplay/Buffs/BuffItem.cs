using System;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    /// <summary>
    /// Buff Item, will trigger and pass arguments to a Buff UI object.
    /// </summary>
    [CreateAssetMenu(menuName = "Buffs/Buff Item", order = 2)]
    public class BuffItem : BuffsTemplateSO
    {
        public static event Action<string, Rarity, float, float> OnActivated;

        public override void Apply()
        {
            OnActivated.Invoke(buffTagId, rarity, buffDuration, amount);
        }
    }
}
