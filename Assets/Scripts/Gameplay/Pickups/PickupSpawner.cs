using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Loot;

namespace ZombieSurvivor3D
{
    public class PickupSpawner : MonoBehaviour
    {
        /// <summary>
        /// /// The following possible pickups: De/Buffs, Perks, Hop-Ups
        /// 
        /// Entry:
        /// Enemy dies. there is a chance for a pickup to appear.
        /// If the chance occurs and is guaranteed, spawn the pickup.
        /// Pickup spawn on enemy. Pass the vector3 of the enemy with a Y-Axis offset to here.
        /// 
        /// Mid:
        /// Set a default timer for the pickup to remain on the ground.
        /// When Timer reaches or about to reach its end, The pickup will slowly blip, up to 5 times.
        /// Each blip will have a delay of 1 second.
        /// 
        /// Exit:
        /// On the 5th blip, the pickup shall disappear.
        /// On disappear, either destroy or move the pickup to a different location.
        /// Disable all possible colliders to prevent accidental triggers.
        /// </summary>

        [SerializeField] private float Timer;
        [SerializeField] private Vector3 spawnPos;

        void Awake()
        {
            NumberGenerator.OnRandomNumberGeneratedPickups += SpawnPickup;
        }

        private void OnDestroy()
        {
            NumberGenerator.OnRandomNumberGeneratedPickups -= SpawnPickup;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                spawnPos = new Vector3(Random.Range(1, 100), 
                                       Random.Range(1, 100), 
                                       Random.Range(1, 100));

                NumberGenerator.GenerateForPickups(spawnPos);
            }
        }

        private void SpawnPickup(Vector3 pos)
        {
            Debug.Log("Spawn Pickup at the following possition" + pos);
        }


    }
}
