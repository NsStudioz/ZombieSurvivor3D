using System;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class NumberGenerator
    {

        public static event Action<int> OnRandomNumberGeneratedLoot;
        public static event Action<int> OnRandomNumberGeneratedBuffs;

        public static void GenerateForLoot()
        {
            int rnd = GetRnd();
            OnRandomNumberGeneratedLoot?.Invoke(rnd);
        }

        public static void GenerateForBuffs()
        {
            int rnd = GetRnd();
            OnRandomNumberGeneratedBuffs?.Invoke(rnd);
        }

        private static int GetRnd()
        {
            return UnityEngine.Random.Range(1, 100);
        }
    }
}
