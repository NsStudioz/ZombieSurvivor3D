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
