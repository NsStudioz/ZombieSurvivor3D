using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Loot;

namespace ZombieSurvivor3D.Gameplay.Pickups
{
    public class PickupSpawner : MonoBehaviour
    {

        // Spawns the following possible pickups: De/Buffs, Perks, Hop-Ups
        // When an enemy dies. there is a chance for a pickup to appear.
        // If the chance occurs and is guaranteed, spawn the pickup.
        // Pickup spawn on enemy. Pass the vector3 of the enemy with a Y-Axis offset to here.


        [Header("Spawn Position")]
        [SerializeField] private Vector3 spawnPos;

        [Header("Rarity Randomizer")]
        [SerializeField] private float rndLowest = 0.1f;
        [SerializeField]  private float rndHighest = 100.0f;
        private float rndFloat;
        private int zeroValue = 0; // for list index

        [Header("Spawnables")]
        [SerializeField] private List<GameObject> pickupsCommon = new List<GameObject>();
        [SerializeField] private List<GameObject> pickupsUncommon = new List<GameObject>();
        [SerializeField] private List<GameObject> pickupsRare = new List<GameObject>();

        /// <summary>
        /// 
        /// common   = 66.6f  // perks (Any).
        /// uncommon = 99.6f  // buffs, hop-ups: FIRE-SALE
        /// rare     = 100.0f  // buffs, hop-ups: Kaboom, Max-Ammo, Insta-Kill, Double-Points... (Maybe...Carpenter)
        /// 
        /// (Rare = must survive a while before trigger rare:)
        /// 
        /// </summary>


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
                spawnPos = new Vector3(Random.Range(0, 1),
                                       Random.Range(0, 1),
                                       Random.Range(0, 1));

                NumberGenerator.GenerateForPickups(spawnPos);
            }
        }

        /// <summary>
        /// Pickup spawn on enemy. Passes a Vector3 of the enemy with a Y-Axis offset to here.
        /// </summary>
        private void SpawnPickup(Vector3 pos)
        {
            rndFloat = Random.Range(rndLowest, rndHighest);

            if (Randomizer.IsCommon(rndFloat))
            {   
                ChoosePickup(pickupsCommon, pos);
                Debug.Log("Spawn Common Pickup " + pos); 
            }
            else if (Randomizer.IsUncommon(rndFloat))
            {   
                ChoosePickup(pickupsUncommon, pos);
                Debug.Log("Spawn Uncommon Pickup " + pos); 
            }
            else if (Randomizer.IsRare(rndFloat))
            {   
                ChoosePickup(pickupsRare, pos);
                Debug.Log("Spawn Rare Pickup " + pos); 
            }
        }

        // Access interace of gameobject  =  IEquipable:
        private void ChoosePickup(List<GameObject> list, Vector3 pos)
        {
            GameObject pickupGO = GetPickupFromList(list, pos);
            //
            IPickupable pickupable = pickupGO.GetComponent<IPickupable>();
            pickupable?.OnPickup();
        }

        /// <summary>
        /// Get Pickup Gameobject from desired list and pass the designated position.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private GameObject GetPickupFromList(List<GameObject> list, Vector3 pos)
        {
            return Instantiate(
                    list[Random.Range(zeroValue, list.Count)],
                    pos,
                    Quaternion.identity);
        }

    }
}
