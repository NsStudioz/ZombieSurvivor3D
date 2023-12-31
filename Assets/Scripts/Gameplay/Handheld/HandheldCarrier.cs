using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldCarrier : MonoBehaviour, ControlsPC.IGameplayControlsActions, ControlsPC.IInteractionControlsActions
    {
        // System for important logic (Math + fire weapon + etc...)

        [Header("Main Elements")]
        [SerializeField] Transform rigSocket;
        [SerializeField] Animator rigAnimator;
        public List<HandheldSO> EquipedHandhelds;

        // Current Handheld Elements
        HandheldSO currentHandheldSO;
        GameObject currentHandheldGO; // used in method: SwitchHandheldOld()
        IHandheldObject currentHandheldInterface;
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

        #region Helpers

        public Animator GetAnimator()
        {
            return rigAnimator;
        }

        public HandheldSO GetCurrentHandheldScriptableObject()
        {
            return currentHandheldSO;
        }

        #endregion

        #region Event_Listeners

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            playerInput.actions.FindActionMap("GameplayControls").Enable();
            playerInput.actions.FindActionMap("InteractionControls").Enable();
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

        private void Start()
        {
            InitializeHandheldGOsList();
            SwitchHandheld(EquipedHandhelds[0]);

            OnHandheldChanged?.Invoke(EquipedHandhelds[0].HandheldBulletPrefab);
        }

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

        private void InitializeHandheldGOsList()
        {
            for (int i = 0; i < HandheldsGO.Count; i++)
                SetHandheldPosition(i, preservedHandheldsTransform, false, true);
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
            currentHandheldInterface = HandheldsGO[handheldSOIndex].GetComponentInChildren<IHandheldObject>();

            if (currentHandheldInterface != null)
            {
                currentHandheldInterface.OnAttachedCarrier(this);
                currentHandheldInterface.OnEquip();

                rigAnimator.runtimeAnimatorController = handheld.RigAnimController;
            }
            else
                RemoveHandheld(handheldSOIndex);
        }

        private void RemoveHandheld(int index)
        {
            //HandheldsGO[index].SetActive(false); // Temporarily kept.
            SetHandheldPosition(index, preservedHandheldsTransform, false, true);
            currentHandheldSO = null;
            currentHandheldInterface = null;
            currentHandheldGO = null;
        }

        private void SetHandheldPosition(int index, Transform parent, bool isActive, bool worldPositionStays)
        {
            HandheldsGO[index].SetActive(isActive);
            HandheldsGO[index].transform.SetParent(parent, worldPositionStays);
            HandheldsGO[index].transform.localPosition = Vector3.zero;
            HandheldsGO[index].transform.localRotation = Quaternion.identity;
        }

        // (OLD PROTOTYPE) Weapon/Equipment Switching. Temporarily Keeping it as a backup:
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

        #region Input_Events:

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
                if (EquipedHandhelds[currentHandheldIndex] == interactableHandheldSO)
                    OnInteractSimilarHandheld?.Invoke(interactableHandheldSO);

                else if (EquipedHandhelds[currentHandheldIndex] != interactableHandheldSO)
                {
                    EquipedHandhelds[currentHandheldIndex] = interactableHandheldSO;
                    SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);
                    OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab);
                }
            }
        }

        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentHandheldIndex += 1 * (int)Mathf.Sign(context.ReadValue<float>()); // can't use int, must cast to float.
                currentHandheldIndex = Mathf.Clamp(currentHandheldIndex, 0, EquipedHandhelds.Count - 1); // clamp between 0 to max count - 1.

                SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);

                OnHandheldChanged?.Invoke(EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab);
            }

            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldInterface != null)
                currentHandheldInterface.OnSwitchWeapon(context);
        }

        public void OnFire1(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldInterface != null)
                currentHandheldInterface.OnFire1(context);
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldInterface != null)
                currentHandheldInterface.OnFire2(context);
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (currentHandheldInterface != null)
                currentHandheldInterface.OnReload(context);
        }


        #endregion

    }

}
