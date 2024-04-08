using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.Bullets;
using ZombieSurvivor3D.Gameplay.Health;
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
        //[SerializeField] private float Timer;

        [Header("Rarity Randomizer")]
        [SerializeField] private float rndLowest = 0.1f;
        [SerializeField]  private float rndHighest = 100.0f;
        private float rndFloat;

        [Header("Spawnables")]
        [SerializeField] private List<GameObject> pickupsCommon = new List<GameObject>();
        [SerializeField] private List<GameObject> pickupsUncommon = new List<GameObject>();
        [SerializeField] private List<GameObject> pickupsRare = new List<GameObject>();
        private int zeroValue = 0; // for list index

        #region Rarity

        private float rare = 100.0f;     // perks (Any).
        private float uncommon = 66.6f; // buffs, hop-ups: FIRE-SALE
        private float common = 33.3f;   // buffs, hop-ups: Kaboom, Max-Ammo, Insta-Kill, Double-Points... (Maybe...Carpenter)

        /// <summary>
        /// 
        /// common   = 66.6f
        /// uncommon = 99.6f
        /// rare     = 99.7f (must survive a while before trigger rare:)
        /// 
        /// </summary>

        #endregion

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

        /// <summary>
        /// Pickup spawn on enemy. Passes a Vector3 of the enemy with a Y-Axis offset to here.
        /// </summary>
        private void SpawnPickup(Vector3 pos)
        {
            rndFloat = Random.Range(rndLowest, rndHighest);

            if (rndFloat > 0 && rndFloat <= common)
            {   
                ChoosePickup(pickupsCommon, spawnPos);
                Debug.Log("Spawn Common Pickup " + pos); 
            }
            else if (rndFloat > common && rndFloat <= uncommon)
            {   
                ChoosePickup(pickupsUncommon, spawnPos);
                Debug.Log("Spawn Uncommon Pickup " + pos); 
            }
            else if (rndFloat > uncommon && rndFloat <= rare)
            {   
                ChoosePickup(pickupsRare, spawnPos);
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
