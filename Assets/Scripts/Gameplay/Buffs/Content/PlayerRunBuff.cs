using System;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Buffs.Content
{
    [CreateAssetMenu(menuName = "Buffs/Player Run", order = 0)]
    public class PlayerRunBuff : BuffsTemplateSO
    {
        public static event Action<float, float> OnActivated;

        public override void Apply()
        {
            OnActivated.Invoke(buffDuration, amount);
        }
    }
}
