using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Pickups
{
    public class Pickup : GameListener, IPickupable
    {
        /// <summary>
        /// 
        /// Entry:
        /// Set a default timer for the pickup to remain on the ground.
        /// When Timer reaches or about to reach its end, The pickup will slowly blip, up to 5 times.
        /// Each blip will have a delay of 1 second.
        /// 
        /// Exit:
        /// On the 5th blip, the pickup shall disappear.
        /// On disappear, either destroy or move the pickup to a different location.
        /// Disable all possible colliders to prevent accidental triggers.
        ///
        /// </summary>

        [Header("Attributes")]
        [SerializeField] private bool isSpawned;
        [SerializeField] private float Timer; // Time to disappear/blip
        private IEnumerator iEnumeratorRef;

        #region EventListeners

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopCoroutine(iEnumeratorRef);
        }

        protected override void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
/*            if (this == null)
                return;*/

            base.OnGameStateChanged(newGameState);

            if (newGameState == GameStateManager.GameState.Paused)
                StopCoroutine(iEnumeratorRef);

            else if (newGameState == GameStateManager.GameState.Gameplay)
                StartCoroutine(iEnumeratorRef);
        }

        #endregion

        public void OnSpawned()
        {
            isSpawned = true;
            iEnumeratorRef = SetVisibilityTimer();
            StartCoroutine(iEnumeratorRef);  // DOES NOT WORK WITH GAMESTATEMANAGER. TIMER DOES NOT STOP.
        }

        public void OnIgnored()
        {
            isSpawned = false;
            Destroy(gameObject);
        }

        public void OnPicked()
        {
            // Picked
        }

        /// <summary>
        /// 
        /// While true...
        /// Play animation and SFX for 'Timer' amount of seconds.
        /// When 'Timer' reaches its end...
        /// Start blipping (Play animation + SFX)
        /// On the 5th Blip, disappear the object entirely.
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetVisibilityTimer()
        {
            while (isSpawned)
            {
                // play animation (maybe can make the blip in animation).
                // play sound.
                for (int i = 0; i < 100; i++)
                    yield return new WaitForSeconds(Timer / 100);
                
                OnIgnored();
            }   
        }


    }
}
