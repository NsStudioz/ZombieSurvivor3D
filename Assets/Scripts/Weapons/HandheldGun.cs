using System;
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
    [SerializeField] int FiringModeInt = 0;

    [Header("Firing Modes Attributes")]
    private const int MODE_SINGLE = 3;
    private const int MODE_SEMI = 2;
    private const int MODE_BURST = 1;
    private const int MODE_AUTO = 0;
    

    [Header("Testing Purposes")]
    [SerializeField] GameObject bulletTestGO;
    [SerializeField] private bool isLeftMouseClickHeld = false;

/*    [Header("Audio Files")]
    [SerializeField] AudioClip _EquipAudio;
    [SerializeField] AudioClip _UnequipAudio;
    [SerializeField] AudioClip _FireAudio;
    [SerializeField] AudioClip _FailedFireAudio;*/

    private CarrierSystem carrierSystem;

    #region Event_Listeners

    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        CarrierSystem.OnInteractSimilarHandheld += RestockHandheldAmmo;
    }

    private void OnDestroy()
    {
        CarrierSystem.OnInteractSimilarHandheld -= RestockHandheldAmmo;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void RestockHandheldAmmo(HandheldSO handheld)
    {
        ammoCapacity = handheld.ammoCapacity;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if (this != null)
            enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

    #endregion

    private void Update()
    {
        if (ammoCapacity <= 0)
            return;

        while (isLeftMouseClickHeld)
        {
            fireRate -= Time.deltaTime;

            if (fireRate <= 0f)
            {
                //GameObject bulletInstance = Instantiate(bulletTestGO, transform.position, transform.rotation);
                BulletSpawner.Instance.SpawnBullet(transform.position, transform.rotation);
                ammoCapacity--;
                fireRate = fireRateCooldown;
            }

            break;
        }
    }

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
        FiringModeInt = (int)carrierSystem.GetCurrentHandheldScriptableObject().FiringMode;
        //  
        bulletTestGO = carrierSystem.GetCurrentHandheldScriptableObject().HandheldBulletPrefab;
    }


    #region Input_Events:

    // These code block templates are for Handheld input actions:
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {

    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLeftMouseClickHeld = true;
        }

        else if (context.canceled)
        {
            isLeftMouseClickHeld = false;
            fireRate = 0f;
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {

    }

    public void OnReload(InputAction.CallbackContext context)
    {

    }

    #endregion

}
