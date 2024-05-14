using System;
using System.Numerics;

namespace ZombieSurvivor3D.Gameplay.RNG
{
    public class NumberGenerator
    {

        //public static event Action<float> OnRNGBuffs;
        //public static event Action<float> OnRNGLoot;
        //public static event Action<UnityEngine.Vector3> OnRNGPickups;

        public static void GenerateForLoot()
        {
            float rnd = GetRndFloat();
            //OnRNGLoot?.Invoke(rnd);
            EventManager<float>.Raise(Events.EventKey.OnRNGLoot.ToString(), rnd);
        }

        public static void GenerateForBuffs()
        {
            float rnd = GetRndFloat();
            //OnRNGBuffs?.Invoke(rnd);
            EventManager<float>.Raise(Events.EventKey.OnRNGBuffs.ToString(), rnd);
        }

        public static void GenerateForPickups(UnityEngine.Vector3 pos)
        {
            float rnd = GetRndFloat();

            // currently guarantees invokes every time:
            if (rnd >= 1)
                EventManager<UnityEngine.Vector3>.Raise(Events.EventKey.OnRNGPickups.ToString() ,pos);
                //OnRNGPickups?.Invoke(pos);
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
