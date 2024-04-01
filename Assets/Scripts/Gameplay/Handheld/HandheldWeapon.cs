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

        HandheldCarrier handheldCarrier;

        [Header("Main Elements")]
        [SerializeField] Animator weaponAnimator;
        [SerializeField] string handheldName;
        [SerializeField] private bool isFirstTimeEquipped = false; // new weapon in carrier.

        [Header("Ammo")]
        [SerializeField] int ammoInMag;
        [SerializeField] int ammoInMagFull;
        [SerializeField] int ammoMax;

        [Header("Firing")]
        [SerializeField] float fireRate;
        [SerializeField] float fireRateCooldown;
        [SerializeField] int FiringModeInt = 0;
        [SerializeField] bool isLeftMouseClickHeld = false;

        [Header("Reload")]
        [SerializeField] float reloadCooldown;

        [Header("Testing Purposes")]
        [SerializeField] GameObject bulletTestGO;


        /*        [Header("Firing Modes Attributes")]
                private const int MODE_SINGLE = 3;
                private const int MODE_SEMI = 2;
                private const int MODE_BURST = 1;
                private const int MODE_AUTO = 0;*/

        /*    [Header("Audio Files")]
            [SerializeField] AudioClip _EquipAudio;
            [SerializeField] AudioClip _UnequipAudio;
            [SerializeField] AudioClip _FireAudio;
            [SerializeField] AudioClip _FailedFireAudio;*/



        #region Event_Listeners

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            HandheldCarrier.OnInteractSimilarHandheld += RestockHandheldAmmo;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            HandheldCarrier.OnInteractSimilarHandheld -= RestockHandheldAmmo;
        }

        private void RestockHandheldAmmo(HandheldSO handheld)
        {
            // if string not working, use tag: handheld != handHeldTag.GetHandheldSOTag()
            if (handheld.HandheldName != handheldName) 
                return;

            ammoInMag = handheld.AmmoInMag;
            ammoMax = handheld.AmmoMax;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            if (this != null)
                enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        private void Update()
        {
            if (ammoInMag <= 0)
                return;

            while (isLeftMouseClickHeld)
            {
                fireRate -= Time.deltaTime;

                if (fireRate <= 0f)
                {
                    BulletSpawner.Instance.SpawnBullet(transform.position, transform.rotation);
                    ammoInMag--;
                    fireRate = fireRateCooldown;
                }

                break;
            }
        }


        #region InterfaceFunctions:

        // updates our carrier, with that we can use the animations of a specific handheld:
        public void OnAttachedCarrier(HandheldCarrier attachedCarrier)
        {
            handheldCarrier = attachedCarrier;
            weaponAnimator = handheldCarrier.GetAnimator();
        }

        public void OnEquip()
        {
            //Example: carrierSystem.GetAnimator().SetTrigger("Equip");

            if (!isFirstTimeEquipped)
                return;

            SyncHandheldGunData();
            SyncAmmoFirstTime();
            isFirstTimeEquipped = false;
        }

        public void OnUnequip()
        {

        }

        #endregion

        private void SyncHandheldGunData()
        {
            handheldName = handheldCarrier.GetCurrentHandheldScriptableObject().HandheldName;
            fireRate = handheldCarrier.GetCurrentHandheldScriptableObject().FireRate;
            fireRateCooldown = handheldCarrier.GetCurrentHandheldScriptableObject().FireRateCooldown;
            reloadCooldown = handheldCarrier.GetCurrentHandheldScriptableObject().ReloadCooldown;
            FiringModeInt = (int)handheldCarrier.GetCurrentHandheldScriptableObject().FiringMode;
            //  
            bulletTestGO = handheldCarrier.GetCurrentHandheldScriptableObject().HandheldBulletPrefab;
        }

        private void SyncAmmoFirstTime()
        {
            ammoInMag = handheldCarrier.GetCurrentHandheldScriptableObject().AmmoInMag;
            ammoInMagFull = ammoInMag;
            ammoMax = handheldCarrier.GetCurrentHandheldScriptableObject().AmmoMax;
        }

        /// <summary>
        /// Re-sync gun data when re-picking this gun up from the environment.
        /// </summary>
        public void RemoveHandheldFromPlayer()
        {
            isFirstTimeEquipped = true;
        }

        #region Used_Input_Events:
        // These code block templates are for Handheld input actions and logic:

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

        public void OnReload(InputAction.CallbackContext context)
        {
            if (ammoMax <= 0 || ammoInMag == ammoInMagFull)
                return;

            // how many bullets spend in a mag:
            int spentAmmo;
            spentAmmo = ammoInMagFull - ammoInMag;

            // AF = 30 | AS = 2 | AIM = 28 | ammoMax = 1
            if (ammoMax <= spentAmmo) // if this is the last magazine:
            {
                ammoInMag += ammoMax; // AIM = 29
                ammoMax -= ammoMax;   // AM = 0
                return;
            }

            ammoMax -= spentAmmo;
            ammoInMag += spentAmmo;
        }

        #endregion

        #region OnHold_Input_Events:

        public void OnFire2(InputAction.CallbackContext context)
        {

        }

        public void OnNextWeapon(InputAction.CallbackContext context)
        {
            //Debug.Log("Individual Handheld Registered Scroll Up");
        }

        public void OnPreviousWeapon(InputAction.CallbackContext context)
        {
            //Debug.Log("Individual Handheld Registered Scroll Down");
        }

        public void OnScroll(InputAction.CallbackContext context)
        {

        }

        #endregion

    }

}
