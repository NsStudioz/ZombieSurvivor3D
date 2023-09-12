using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandheldGun : MonoBehaviour, IHandheldObject
{
    // Input Handling will trigger animations and weapon/equipment logic:

    [Header("Main Elements")]
    [SerializeField] private Animator _WeaponAnimator;
    [SerializeField] string handheldName;

    [Header("Main Attributes")]
    [SerializeField] int ammoCapacity;
    [SerializeField] float fireRate;
    [SerializeField] float fireRateCooldown;
    [SerializeField] float reloadCooldown;

    [Header("Second Attributes")]
    [SerializeField] string HandheldType = "";
    [SerializeField] string FiringMode = "";

/*    [Header("Audio Files")]
    [SerializeField] AudioClip _EquipAudio;
    [SerializeField] AudioClip _UnequipAudio;
    [SerializeField] AudioClip _FireAudio;
    [SerializeField] AudioClip _FailedFireAudio;*/

    private CarrierSystem carrierSystem;

    // updates our carrier, with that we can use the animations of a specific handheld:
    public void OnAttachedCarrier(CarrierSystem attachedCarrier)
    {
        carrierSystem = attachedCarrier;
        _WeaponAnimator = carrierSystem.GetAnimator();
    }

    public void OnEquip()
    {
        SyncHandheldGunData();
        // Example:
        //carrierSystem.GetAnimator().SetTrigger("Equip");
    }

    public void OnUnequip()
    {
        
    }

    private void SyncHandheldGunData()
    {
        handheldName = carrierSystem.GetCurrentHandheldScriptableObject().handheldName;
        ammoCapacity = carrierSystem.GetCurrentHandheldScriptableObject().ammoCapacity;
        fireRate = carrierSystem.GetCurrentHandheldScriptableObject().fireRate;
        fireRateCooldown = carrierSystem.GetCurrentHandheldScriptableObject().fireRateCooldown;
        reloadCooldown = carrierSystem.GetCurrentHandheldScriptableObject().reloadCooldown;
        //  
        HandheldType = carrierSystem.GetCurrentHandheldScriptableObject().HandheldType.ToString();
        FiringMode = carrierSystem.GetCurrentHandheldScriptableObject().FiringMode.ToString();
    }


    #region Input_Events:

    // These code block templates are for Handheld input actions:
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {

    }

    public void OnFire1(InputAction.CallbackContext context)
    {

    }

    public void OnFire2(InputAction.CallbackContext context)
    {

    }

    public void OnReload(InputAction.CallbackContext context)
    {

    }

    public void OnReplaceWeapon(InputAction.CallbackContext context)
    {

    }

    #endregion

}
