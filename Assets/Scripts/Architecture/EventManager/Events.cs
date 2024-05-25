using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public static class Events
    {
        public enum Gameplay
        {
            None,
            OnGameStateChange,
            OnHandheldChanged,
            OnHandheldSimilar,
            OnArsenalBoxItemInteracted,
            OnPlayerDead,
            OnSpecialEvent,
            OnTimerStateChange,
            OnGameOverResult,
            OnGameOverButtonsCallback
        }

        public enum GameplayRNG
        {
            None,
            OnBuffRoll,
            OnRNGLoot,
            OnRNGBuffs,
            OnRNGPickups,
            OnSpawnLoot,
        }

        public enum MainMenu
        {
            None,
        }

        public enum Settings
        {
            None,
        }

    }
}
