using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldCarrier : GameListener, ControlsPC.IGameplayControlsActions, ControlsPC.IInteractionControlsActions
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

        #region Input_Init

        private void OnEnable()
        {
            playerInput.actions.FindActionMap("GameplayControls").Enable();
            playerInput.actions.FindActionMap("InteractionControls").Enable();
        }

        private void OnDisable()
        {
            playerInput.actions.FindActionMap("GameplayControls").Disable();
            playerInput.actions.FindActionMap("InteractionControls").Disable();
        }

        #endregion

        #region Initialization:

        private void Start()
        {
            InitializeHandheldGOsList();
            SwitchHandheld(EquipedHandhelds[0]);

            EventManager<GameObject>.Raise(Events.EventKey.OnHandheldChanged.ToString(), 
                                           EquipedHandhelds[0].HandheldBulletPrefab);
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
                    // Listener = HandheldWeapon:
                    EventManager<HandheldSO>.Raise(Events.EventKey.OnHandheldSimilar.ToString(), interactableHandheldSO);
                    return;
                }

            // De-sync gun data:
            HandheldsGO[handheldSOIndex].GetComponentInChildren<HandheldWeapon>().RemoveFromPlayer();

            //Replace current with the new weapon:
            EquipedHandhelds[currentHandheldIndex] = interactableHandheldSO;
            SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);

            // Listener = BulletSpawner
            EventManager<GameObject>.Raise(Events.EventKey.OnHandheldChanged.ToString(), 
                                           EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab);
        }

        #endregion

        #region HandheldSwitching:

        private void ApplySwitch(float value)
        {
            // can't use int, must cast to float:
            currentHandheldIndex += 1 * (int)Mathf.Sign(value);

            // clamp between 0 to max count - 1:1
            currentHandheldIndex = Mathf.Clamp(currentHandheldIndex, 0, EquipedHandhelds.Count - 1); 

            SwitchHandheld(EquipedHandhelds[currentHandheldIndex]);

            // Listener = BulletSpawner
            EventManager<GameObject>.Raise(Events.EventKey.OnHandheldChanged.ToString(), 
                                           EquipedHandhelds[currentHandheldIndex].HandheldBulletPrefab);
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
                // listener = ArsenalBoxDetector
                EventManager<int>.Raise(Events.EventKey.OnArsenalBoxItemInteracted.ToString(), 0);
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



