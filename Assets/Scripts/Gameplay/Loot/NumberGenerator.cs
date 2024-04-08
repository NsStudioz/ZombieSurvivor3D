using System;
using System.Numerics;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class NumberGenerator
    {

        public static event Action<float> OnRandomNumberGeneratedLoot;
        public static event Action<float> OnRandomNumberGeneratedBuffs;
        public static event Action<UnityEngine.Vector3> OnRandomNumberGeneratedPickups;

        public static void GenerateForLoot()
        {
            float rnd = GetRndFloat();
            OnRandomNumberGeneratedLoot?.Invoke(rnd);
        }

        public static void GenerateForBuffs()
        {
            float rnd = GetRndFloat();
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
