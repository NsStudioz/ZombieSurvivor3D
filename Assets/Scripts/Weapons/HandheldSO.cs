using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Handheld", menuName = "New Handheld SO", order = 1)]
public class HandheldSO : ScriptableObject
{
    [Header("Main Elements")]
    [SerializeField] GameObject HandheldPrefab;
    [SerializeField] RuntimeAnimatorController RigAnimController;

    public HandheldTypes HandheldType { get; private set; }
    public FiringModes FiringMode { get; private set; }

    [Header("Attributes")]
    [SerializeField] string handheldName;
    [SerializeField] int ammoCapacity;
    [SerializeField] int fireRate;
    [SerializeField] int fireRateCooldown;
    [SerializeField] float reloadCooldown;

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
