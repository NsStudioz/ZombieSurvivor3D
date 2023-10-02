using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class CarrierSystem : MonoBehaviour, ControlsPC.IGameplayControlsActions, ControlsPC.IInteractionControlsActions
    {
        // System for important logic (Math + fire weapon + etc...)

        [SerializeField] Transform RigSocket;
        [SerializeField] Animator RigAnimator;

        public List<HandheldSO> EquipableHandhelds;

        private HandheldSO _CurrentHandheldSO;
        private GameObject _CurrentHandheldGO;
        private IHandheldObject _CurrentHandheldInterface;

        private int _CurrentHandheldIndex;

        [SerializeField] private bool isInteractButtonClickHeld = false;

        [SerializeField] private HandheldSO _InteractableHandheldSO = null;

        [SerializeField] PlayerInput playerInput;

        public static event Action<HandheldSO> OnInteractSimilarHandheld;

        public static event Action<GameObject> OnHandheldChanged;


        // EXPERIMENT:
        public List<GameObject> HandheldsGO;
        private int handheldSOIndex = 0;
        [SerializeField] private Transform preservedHandheldsTransform;
        //

        #region Helpers

        public Animator GetAnimator()
        {
            return RigAnimator;
        }

        public HandheldSO GetCurrentHandheldScriptableObject()
        {
            return _CurrentHandheldSO;
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
            // EXPERIMENT:
            InitializeHandheldGOsList();
            BetterSwitchHandheld(EquipableHandhelds[0]);
            //------------------------------------------------
            //SwitchHandheld(EquipableHandhelds[0]);
            OnHandheldChanged?.Invoke(EquipableHandhelds[0].HandheldBulletPrefab);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Handheld"))
            {
                _InteractableHandheldSO = col.GetComponentInChildren<HandheldSOTag>().GetHandheldSOTag();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Handheld"))
            {
                _InteractableHandheldSO = null;
            }
        }

        private void InitializeHandheldGOsList()
        {
            for (int i = 0; i < HandheldsGO.Count; i++)
            {
                HandheldsGO[i].SetActive(false);
                HandheldsGO[i].transform.SetParent(preservedHandheldsTransform, true);
                HandheldsGO[i].transform.localPosition = Vector3.zero;
                HandheldsGO[i].transform.localRotation = Quaternion.identity;
            }
        }

        public void BetterSwitchHandheld(HandheldSO handheld)
        {
            if (_CurrentHandheldSO == handheld)
                return;

            // remove previous handheld
            HandheldsGO[handheldSOIndex].SetActive(false);
            HandheldsGO[handheldSOIndex].transform.SetParent(preservedHandheldsTransform, true);
            HandheldsGO[handheldSOIndex].transform.localPosition = Vector3.zero;
            HandheldsGO[handheldSOIndex].transform.localRotation = Quaternion.identity;

            // grab next handheld and id
            _CurrentHandheldSO = handheld;
            handheldSOIndex = handheld.Id;

            // use ID to attach to player
            HandheldsGO[handheldSOIndex].SetActive(true);
            HandheldsGO[handheldSOIndex].transform.SetParent(RigSocket, true);
            HandheldsGO[handheldSOIndex].transform.localPosition = Vector3.zero;
            HandheldsGO[handheldSOIndex].transform.localRotation = Quaternion.identity;

            // getting the interface to set animations and events on our model and not parent object.
            _CurrentHandheldInterface = HandheldsGO[handheldSOIndex].GetComponentInChildren<IHandheldObject>();

            if (_CurrentHandheldInterface != null)
            {
                _CurrentHandheldInterface.OnAttachedCarrier(this);
                _CurrentHandheldInterface.OnEquip();

                RigAnimator.runtimeAnimatorController = handheld.RigAnimController;
            }
            else
            {
                HandheldsGO[handheldSOIndex].SetActive(false);
                _CurrentHandheldSO = null;
                _CurrentHandheldInterface = null;
                _CurrentHandheldGO = null;
            }
        }

        // Weapon/Equipment Switching:
        public void SwitchHandheld(HandheldSO handheld)
        {
            if (_CurrentHandheldSO == handheld)
                return;

            Destroy(_CurrentHandheldGO);

            _CurrentHandheldSO = handheld;
            _CurrentHandheldGO = Instantiate(_CurrentHandheldSO.HandheldPrefab, RigSocket, true); // prototype code, needs to be updated to support reload
                                                                                                  //_CurrentHandheldGO.transform.position = Vector3.zero;
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
        }

        #region Input_Events:

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (_InteractableHandheldSO == null)
                return;

            if (context.performed)
            {
                isInteractButtonClickHeld = true;
            }

            else if (context.canceled)
                isInteractButtonClickHeld = false;

            if (isInteractButtonClickHeld)
            {
                if (EquipableHandhelds[_CurrentHandheldIndex] == _InteractableHandheldSO)
                    OnInteractSimilarHandheld?.Invoke(_InteractableHandheldSO);

                else if (EquipableHandhelds[_CurrentHandheldIndex] != _InteractableHandheldSO)
                {
                    EquipableHandhelds[_CurrentHandheldIndex] = _InteractableHandheldSO;
                    SwitchHandheld(EquipableHandhelds[_CurrentHandheldIndex]);
                    OnHandheldChanged?.Invoke(EquipableHandhelds[_CurrentHandheldIndex].HandheldBulletPrefab);
                }
            }
        }

        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _CurrentHandheldIndex += 1 * (int)Mathf.Sign(context.ReadValue<float>()); // can't use int, must cast to float.
                _CurrentHandheldIndex = Mathf.Clamp(_CurrentHandheldIndex, 0, EquipableHandhelds.Count - 1); // clamp between 0 to max count - 1.

                //SwitchHandheld(EquipableHandhelds[_CurrentHandheldIndex]);
                BetterSwitchHandheld(EquipableHandhelds[_CurrentHandheldIndex]);

                // Bug: deletes bullets on press, regardless if this is the correct behaviour:
                OnHandheldChanged?.Invoke(EquipableHandhelds[_CurrentHandheldIndex].HandheldBulletPrefab);
            }

            // Using this code block to avoid binding/unbiding from our input system:
            if (_CurrentHandheldInterface != null)
                _CurrentHandheldInterface.OnSwitchWeapon(context);
        }

        public void OnFire1(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (_CurrentHandheldInterface != null)
                _CurrentHandheldInterface.OnFire1(context);
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (_CurrentHandheldInterface != null)
                _CurrentHandheldInterface.OnFire2(context);
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            // Using this code block to avoid binding/unbiding from our input system:
            if (_CurrentHandheldInterface != null)
                _CurrentHandheldInterface.OnReload(context);
        }


        #endregion

    }

}
