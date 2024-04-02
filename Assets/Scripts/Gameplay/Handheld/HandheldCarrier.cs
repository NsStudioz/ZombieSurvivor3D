using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Loot;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldCarrier : MonoBehaviour, ControlsPC.IGameplayControlsActions, ControlsPC.IInteractionControlsActions
    {
        // System for important logic (Math + fire weapon + etc...)
        [Header("My Equipped Handhelds")]
        public List<HandheldSO> EquipedHandhelds;

        [Header("Main Elements")]
        [SerializeField] Transform rigSocket;
        [SerializeField] Animator rigAnimator;

        // Current Handheld Elements
        HandheldSO currentHandheldSO;

        IHandheldObject currentHandheldI;
        int currentHandheldIndex;

        [Header("Handheld Interaction")]
        [SerializeField] bool isInteractButtonClickHeld = false;
        [SerializeField] HandheldSO interactableHandheldSO = null;

        [Header("Player Input")]
        [SerializeField] PlayerInput playerInput;

        // EXPERIMENT:
        [Header("Handhelds recycling")]
        public List<GameObject> HandheldsGO;
        [SerializeField] Transform preservedHandheldsTransform;
        int handheldSOIndex = 0;
        
        //Events
        public static event Action<HandheldSO> OnInteractSimilarHandheld;
        public static event Action<GameObject> OnHandheldChanged;
        public static event Action OnArsenalBoxItemInteracted;

        #region Helpers

        public Animator GetAnimator()
        {
            return rigAnimator;
        }

        public HandheldSO GetCurrentHandheldSO()
        {
            return currentHandheldSO;
        }

        #endregion

        #region Event_Listeners

        private void OnEnable()
        {
            playerInput.actions.FindActionMap("GameplayControls").Enable();
            playerInput.actions.FindActionMap("InteractionControls").Enable();
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            playerInput.actions.FindActionMap("GameplayControls").Disable();
            playerInput.actions.FindActionMap("InteractionControls").Disable();
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        #region Initialization:

        private void Start()
        {
            InitializeHandheldGOsList();
            SwitchHandheld(EquipedHandhelds[0]);

            OnHandheldChanged?.Invoke(EquipedHandhelds[0].HandheldBulletPrefab);
        }

        private void InitializeHandheldGOsList()
        {
            for (int i = 0; i < HandheldsGO.Count; i++)
                SetHandheldPosition(i, preservedHandheldsTransform, false, true);
        }

        #endregion

        #region Interaction:

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Handheld"))
            {
                interactableHandheldSO = col.GetComponentInChildren<HandheldSOTag>().GetHandheldSOTag();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Handheld"))
            {
                interactableHandheldSO = null;
            }
        }

        private void ApplyInteraction()
        {
            // if interacting with a similar weapon, restock ammo:
            for (int i = 0; i < EquipedHandhelds.Count; i++)
                if (EquipedHandhelds[i] == interactableHandheldSO)
                {
                    OnInteractSimilarHandheld?.Invoke(interactableHandheldSO); // Listener = HandheldWeapon
                    return;
                }

            // De-sync gun data:
            HandheldsGO[handheldSOIndex].GetComponentInChildren<HandheldWeapon>().RemoveFromPlayer();
            //Replace current with the new weapon:
            EquipedHandhelds[currentHandheldIndex] = interactableHandheldSO;
            SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);
            OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab); // Listener = BulletSpawner

/*            if (EquipedHandhelds[currentHandheldIndex] == interactableHandheldSO)
                OnInteractSimilarHandheld?.Invoke(interactableHandheldSO); // Listener = HandheldWeapon

            // if interacting with a new weapon:
            else if (EquipedHandhelds[currentHandheldIndex] != interactableHandheldSO)
            {
                // currently works:
                HandheldsGO[handheldSOIndex].GetComponentInChildren<HandheldWeapon>().RemoveHandheldFromPlayer();
                //Replace current with the new weapon:
                EquipedHandhelds[currentHandheldIndex] = interactableHandheldSO;
                SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);
                OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab); // Listener = BulletSpawner    
            }*/
        }

        #endregion

        #region HandheldSwitching:

        private void ApplySwitch(float value)
        {
            currentHandheldIndex += 1 * (int)Mathf.Sign(value); // can't use int, must cast to float.
            currentHandheldIndex = Mathf.Clamp(currentHandheldIndex, 0, EquipedHandhelds.Count - 1); // clamp between 0 to max count - 1.

            SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);
            OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab); // Listener = BulletSpawner
        }

        public void SwitchHandheld(HandheldSO handheld)
        {
            if (currentHandheldSO == handheld)
                return;

            // remove previous handheld:
            SetHandheldPosition(handheldSOIndex, preservedHandheldsTransform, false, true);

            // grab next handheld and id:
            currentHandheldSO = handheld;
            handheldSOIndex = handheld.Id;

            // use ID to attach to player:
            SetHandheldPosition(handheldSOIndex, rigSocket, true, true);

            // getting the interface in child to set animations and events on our model and not parent object.
            currentHandheldI = HandheldsGO[handheldSOIndex].GetComponentInChildren<IHandheldObject>();

            if (currentHandheldI != null)
            {
                currentHandheldI.OnAttachedCarrier(this);
                currentHandheldI.OnEquip();

                rigAnimator.runtimeAnimatorController = handheld.RigAnimController;
            }
            else
                RemoveHandheld(handheldSOIndex);
        }

        private void SetHandheldPosition(int index, Transform parent, bool isActive, bool worldPositionStays)
        {
            HandheldsGO[index].SetActive(isActive);
            HandheldsGO[index].transform.SetParent(parent, worldPositionStays);
            HandheldsGO[index].transform.localPosition = Vector3.zero;
            HandheldsGO[index].transform.localRotation = Quaternion.identity;
        }

        private void RemoveHandheld(int index)
        {
            //HandheldsGO[index].SetActive(false); // Temporarily kept.
            SetHandheldPosition(index, preservedHandheldsTransform, false, true);
            currentHandheldSO = null;
            currentHandheldI = null;
        }

        #endregion

        #region Used_Input_Events:

        public void OnReload(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldI != null)
                currentHandheldI.OnReload(context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (interactableHandheldSO == null)
                return;

            if (context.performed)
            {
                isInteractButtonClickHeld = true;
            }

            else if (context.canceled)
                isInteractButtonClickHeld = false;

            if (isInteractButtonClickHeld)
            {
                ApplyInteraction();
                // Remove interacted item in arsenal box:
                OnArsenalBoxItemInteracted?.Invoke(); // listener = ArsenalBoxDetector
            }
        }

        // NEEDS MAJOR OVERHAUL, CANNOT USE THIS IN FINISHED PRODUCT, TOO COMPLICATED FOR KEY REBINDING/CHANGING:
        public void OnScroll(InputAction.CallbackContext context)
        {
            // Scroll up = Next Weapon | Scroll down = Previous Weapon:
            InputAction action = GetComponentInParent<PlayerInput>().actions["Scroll"];
            float value = action.ReadValue<float>();

            //Debug.Log("Value = " + value);

            if (value > 0)
                ApplySwitch(value);
                //Debug.Log("Scroll Up");

            if (value < 0)
                ApplySwitch(value);
                //Debug.Log("Scroll Down");

            if (currentHandheldI != null)
                currentHandheldI.OnScroll(context);
        }

        // PROTOTYPE, DELETE IF NO LONGER NEEDED:
        public void OnNextWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputAction action = GetComponentInParent<PlayerInput>().actions["NextWeapon"];
                float valueH = action.ReadValue<float>();

                // valueH = 120
                if (valueH <= 0)
                    return;

                ApplySwitch(valueH);

                /*if (valueH > 0)
                {
                    Debug.Log("ValueH = " + valueH); // = 120
                    Debug.Log("Scroll Up");
                }*/
            }

            if (currentHandheldI != null)
                currentHandheldI.OnNextWeapon(context);
        }

        // PROTOTYPE, DELETE IF NO LONGER NEEDED:
        public void OnPreviousWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputAction action = GetComponentInParent<PlayerInput>().actions["PreviousWeapon"];
                float valueL = action.ReadValue<float>();

                valueL = -valueL;

                // valueL = -120
                if (valueL >= 0)
                    return;

                ApplySwitch(valueL);

/*                if (valueL < 0)
                {
                    Debug.Log("ValueL = " + valueL); // = -120
                    Debug.Log("Scroll Down");
                }*/
            }

            if (currentHandheldI != null)
                currentHandheldI.OnPreviousWeapon(context);
        }

        #endregion

        #region OnHold_Input_Events:

        public void OnFire1(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldI != null)
                currentHandheldI.OnFire1(context);
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldI != null)
                currentHandheldI.OnFire2(context);
        }

        #endregion

    }

}


// (OLD PROTOTYPE) Weapon/Equipment Switching. Temporarily Keeping it as a backup:
//GameObject currentHandheldGO; // used in method: SwitchHandheldOld()
//currentHandheldGO = null; // put in: RemoveHandheld if needed.
/*        public void SwitchHandheldOld(HandheldSO handheld)
        {
            if (_CurrentHandheldSO == handheld)
                return;

            Destroy(_CurrentHandheldGO);

            _CurrentHandheldSO = handheld;
            _CurrentHandheldGO = Instantiate(_CurrentHandheldSO.HandheldPrefab, RigSocket, true); // prototype code, needs to be updated to support reload

            _CurrentHandheldGO.transform.localPosition = Vector3.zero;
            _CurrentHandheldGO.transform.localRotation = Quaternion.identity;

            // we want the IHandheldObject script to always be on the model and not on the parent object.
            // that way we can set animations and events on our handhelds
            _CurrentHandheldInterface = _CurrentHandheldGO.GetComponentInChildren<IHandheldObject>();

            if (_CurrentHandheldInterface != null)
            {
                _CurrentHandheldInterface.OnAttachedCarrier(this);
                _CurrentHandheldInterface.OnEquip();

                RigAnimator.runtimeAnimatorController = handheld.RigAnimController;
            }
            else
            {
                DestroyImmediate(_CurrentHandheldGO);
                _CurrentHandheldSO = null;
                _CurrentHandheldInterface = null;
                _CurrentHandheldGO = null;
            }
        }*/


// PROTOTYPE, DELETE IF NO LONGER NEEDED:
/*        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentHandheldIndex += 1 * (int)Mathf.Sign(context.ReadValue<float>()); // can't use int, must cast to float.
                currentHandheldIndex = Mathf.Clamp(currentHandheldIndex, 0, EquipedHandhelds.Count - 1); // clamp between 0 to max count - 1.

                SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);

                OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab); // Listener = BulletSpawner
            }

            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldI != null)
                currentHandheldI.OnSwitchWeapon(context);
        }*/
