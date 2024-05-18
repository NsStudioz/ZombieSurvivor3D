using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public static class Events
    {
        public enum EventKey
        {
            None,
            OnGameStateChange,
            OnBuffRoll,
            OnRNGLoot,
            OnRNGBuffs,
            OnRNGPickups,
            OnSpawnLoot,
            OnHandheldChanged,
            OnHandheldSimilar,
            OnArsenalBoxItemInteracted,
            OnPlayerDead
        }

    }
}
