using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Player
{
    public static class Immunity
    {
        private static bool isImmune = false;

        public static void ActivateImmunityOnPlayer()
        {
            isImmune = true;
        }

        public static void DeactivateImmunityOnPlayer()
        {
            isImmune = false;
        }
    }
}
