using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public static class Events
    {
        public enum EventKey
        {
            None = 0,
            OnGameStateChange = 1,
            OnBuffRoll = 2,
            OnRNGLoot = 3,
            OnRNGBuffs = 4,
            OnRNGPickups = 5,
            OnSpawnLoot = 6,
            OnHandheldChanged = 7,
            OnHandheldSimilar = 8,
            OnArsenalBoxItemInteracted = 9,

        }

    }
}
