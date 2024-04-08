using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Pickups
{
    public class Pickup : MonoBehaviour, IPickupable
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


        [SerializeField] private float Timer;
        [SerializeField] private bool isSpawned;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void OnPickup()
        {
            isSpawned = true;
            StartCoroutine(AppearanceDuration());
        }

        public void OnIgnored()
        {
            isSpawned = false;
            Destroy(gameObject);
        }

        /// <summary>
        /// While true
        /// 
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
