using System;
using System.Numerics;

namespace ZombieSurvivor3D.Gameplay.RNG
{
    public class NumberGenerator
    {

        public static void GenerateForLoot()
        {
            float rnd = GetRndFloat();
            EventManager<float>.Raise(Events.EventKey.OnRNGLoot.ToString(), rnd);
        }

        public static void GenerateForBuffs()
        {
            float rnd = GetRndFloat();
            EventManager<float>.Raise(Events.EventKey.OnRNGBuffs.ToString(), rnd);
        }

        public static void GenerateForPickups(UnityEngine.Vector3 pos)
        {
            float rnd = GetRndFloat();

            // currently guarantees invokes every time:
            if (rnd >= 1)
                EventManager<UnityEngine.Vector3>.Raise(Events.EventKey.OnRNGPickups.ToString() ,pos);
        }

        private static int GetRnd()
        {
            return UnityEngine.Random.Range(0, 100);
        }

        private static float GetRndFloat()
        {
            return UnityEngine.Random.Range(0f, 100);
        }
    }
}
