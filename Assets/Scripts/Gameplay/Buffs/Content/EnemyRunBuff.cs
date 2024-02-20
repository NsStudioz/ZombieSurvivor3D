using System;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Buffs.Content
{
    [CreateAssetMenu(menuName = "Buffs/Enemy Run", order = 1)]
    public class EnemyRunBuff : BuffsTemplateSO
    {
        public static event Action<float, float> OnActivated;

        public override void Apply()
        {
            OnActivated.Invoke(buffDuration, amount);
        }
    }
}
