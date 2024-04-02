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
        [SerializeField] public string HandheldName;
        [SerializeField] public int AmmoInMag;
        [SerializeField] public int AmmoMax;
        [SerializeField] public float FireRate;
        [SerializeField] public float FireRateCooldown;
        [SerializeField] public float ReloadCooldown;

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

