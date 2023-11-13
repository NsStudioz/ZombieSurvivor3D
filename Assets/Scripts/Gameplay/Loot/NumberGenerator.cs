using System;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class NumberGenerator
    {

        public static event Action<int> OnRandomNumberGenerated;

        public static void Generate()
        {
            int rnd = UnityEngine.Random.Range(1, 100);
            OnRandomNumberGenerated?.Invoke(rnd);
        }
    }
}
