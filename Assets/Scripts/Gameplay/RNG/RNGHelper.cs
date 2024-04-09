using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.RNG
{
    public class RNGHelper
    {

        private static float zeroValueF = 0f;
        private static float common = 33.3f;
        private static float uncommon = 66.6f;
        private static float rare = 100.0f;

        public static bool IsCommon(float value)
        {
            return value >= zeroValueF && value <= common;
        }
        public static bool IsUncommon(float value)
        {
            return value > common && value <= uncommon;
        }

        public static bool IsRare(float value)
        {
            return value > uncommon && value <= rare;
        }
    }
}

/*        public void SimulateRNG<T>(T list1, List<T> list2, List<T> list3,
                                   Action<T> func1, Action<List<T>> func2, Action<List<T>> func3,
                                   int rnd)
        {
            int value = rnd;
            Debug.Log("Random Value: " + value);

            if (value >= 0 && value <= common)
            {
                // COMMON:
                func1(list1);
                Debug.Log("Common");
            }
            else if (value >= common && value <= uncommon)
            {
                // UNCOMMON:
                func2(list2);
                Debug.Log("Uncommon");
            }
            else if (value >= uncommon && value <= rare)
            {
                // RARE:
                func3(list3);
                Debug.Log("Rare!");
            }
        }*/
