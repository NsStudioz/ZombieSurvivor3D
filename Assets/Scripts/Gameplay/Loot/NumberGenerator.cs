using System;
using System.Numerics;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class NumberGenerator
    {

        public static event Action<int> OnRandomNumberGeneratedLoot;
        public static event Action<int> OnRandomNumberGeneratedBuffs;
        public static event Action<UnityEngine.Vector3> OnRandomNumberGeneratedPickups;

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

        public static void GenerateForPickups(UnityEngine.Vector3 pos)
        {
            float rnd = GetRndFloat();

            if (rnd >= 1)
                OnRandomNumberGeneratedPickups?.Invoke(pos);
        }

        private static int GetRnd()
        {
            return UnityEngine.Random.Range(1, 100);
        }

        private static float GetRndFloat()
        {
            return UnityEngine.Random.Range(1, 100);
        }
    }
}
