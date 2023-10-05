using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.ObjectPool;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldWeapon : MonoBehaviour, IHandheldObject
    {
        // Input Handling will trigger animations and weapon/equipment logic:

        [Header("Main Elements")]
        [SerializeField] Animator weaponAnimator;
        [SerializeField] string handheldName;

        [Header("Main Attributes")]
        [SerializeField] int ammoCapacity;
        [SerializeField] float fireRate;
        [SerializeField] float fireRateCooldown;
        [SerializeField] float reloadCooldown;

        [Header("Second Attributes")]
        [SerializeField] int FiringModeInt = 0;

/*        [Header("Firing Modes Attributes")]
        private const int MODE_SINGLE = 3;
        private const int MODE_SEMI = 2;
        private const int MODE_BURST = 1;
        private const int MODE_AUTO = 0;*/


        [Header("Testing Purposes")]
        [SerializeField] GameObject bulletTestGO;
        [SerializeField] bool isLeftMouseClickHeld = false;

        /*    [Header("Audio Files")]
            [SerializeField] AudioClip _EquipAudio;
            [SerializeField] AudioClip _UnequipAudio;
            [SerializeField] AudioClip _FireAudio;
            [SerializeField] AudioClip _FailedFireAudio;*/

        HandheldCarrier handheldCarrier;

        #region Event_Listeners

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            HandheldCarrier.OnInteractSimilarHandheld += RestockHandheldAmmo;
        }

        private void OnDestroy()
        {
            HandheldCarrier.OnInteractSimilarHandheld -= RestockHandheldAmmo;
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void RestockHandheldAmmo(HandheldSO handheld)
        {
            ammoCapacity = handheld.AmmoCapacity;
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
                    BulletSpawner.Instance.SpawnBullet(transform.position, transform.rotation);
                    ammoCapacity--;
                    fireRate = fireRateCooldown;
                }

                break;
            }
        }

        // updates our carrier, with that we can use the animations of a specific handheld:
        public void OnAttachedCarrier(HandheldCarrier attachedCarrier)
        {
            handheldCarrier = attachedCarrier;
            weaponAnimator = handheldCarrier.GetAnimator();
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
            handheldName = handheldCarrier.GetCurrentHandheldScriptableObject().HandheldName;
            ammoCapacity = handheldCarrier.GetCurrentHandheldScriptableObject().AmmoCapacity;
            fireRate = handheldCarrier.GetCurrentHandheldScriptableObject().FireRate;
            fireRateCooldown = handheldCarrier.GetCurrentHandheldScriptableObject().FireRateCooldown;
            reloadCooldown = handheldCarrier.GetCurrentHandheldScriptableObject().ReloadCooldown;
            FiringModeInt = (int)handheldCarrier.GetCurrentHandheldScriptableObject().FiringMode;
            //  
            bulletTestGO = handheldCarrier.GetCurrentHandheldScriptableObject().HandheldBulletPrefab;
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

}
