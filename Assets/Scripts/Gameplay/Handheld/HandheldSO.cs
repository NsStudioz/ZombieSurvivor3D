using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    [CreateAssetMenu(fileName = "Handheld", menuName = "New Handheld SO", order = 1)]
    public class HandheldSO : ScriptableObject
    {
        [Header("Main Elements")]
        [SerializeField] public GameObject HandheldPrefab;
        [SerializeField] public GameObject HandheldBulletPrefab;
        [SerializeField] public RuntimeAnimatorController RigAnimController;

        public HandheldTypes HandheldType = HandheldTypes.Pistol;
        public FiringModes FiringMode = FiringModes.Single;

        [Header("Attributes")]
        [SerializeField] public string handheldName;
        [SerializeField] public int ammoCapacity;
        [SerializeField] public float fireRate;
        [SerializeField] public float fireRateCooldown;
        [SerializeField] public float reloadCooldown;

        [Header("Identification")]
        [SerializeField] public int Id;

        public enum HandheldTypes
        {
            Pistol, Shotgun, SubmachineGun, AssaultRifle, SniperRifle, Launcher, GiftedArmament,
            Equipment, SpecialEquipment
        }

        public enum FiringModes
        {
            Auto, Burst, Semi, Single
        }
    }
}

