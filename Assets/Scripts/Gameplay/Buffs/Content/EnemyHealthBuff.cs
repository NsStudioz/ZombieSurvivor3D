using System;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Buffs.Content
{
    [CreateAssetMenu(menuName = "Buffs/Enemy Health", order = 2)]
    public class EnemyHealthBuff : BuffsTemplateSO
    {
        public static event Action<float, float> OnActivated;

        public override void Apply()
        {
            OnActivated.Invoke(buffDuration, amount);
        }
    }
}
