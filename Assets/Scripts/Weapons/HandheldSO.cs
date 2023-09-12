using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Handheld", menuName = "New Handheld SO", order = 1)]
public class HandheldSO : ScriptableObject
{
    [Header("Main Elements")]
    [SerializeField] public GameObject HandheldPrefab;
    [SerializeField] public RuntimeAnimatorController RigAnimController;

    public HandheldTypes HandheldType { get; private set; }
    public FiringModes FiringMode { get; private set; }

    [Header("Attributes")]
    [SerializeField] public string handheldName;
    [SerializeField] public int ammoCapacity;
    [SerializeField] public int fireRate;
    [SerializeField] public int fireRateCooldown;
    [SerializeField] public float reloadCooldown;

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
