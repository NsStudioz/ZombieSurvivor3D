using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();
        }

        public void OnSpawned()
        {
            isSpawned = true;
            StartCoroutine(AppearanceDuration());  // DOES NOT WORK WITH GAMESTATEMANAGER. TIMER DOES NOT STOP.
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
        private IEnumerator AppearanceDuration()
        {
            while (isSpawned)
            {
                // play animation (maybe can make the blip in animation).
                // play sound.
                yield return new WaitForSeconds(Timer); // timer should be between 15-20 seconds.
                
                OnIgnored();
            }   
        }


    }
}
