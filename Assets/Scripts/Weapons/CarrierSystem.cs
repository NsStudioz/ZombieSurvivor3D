using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        playerInput.actions.FindActionMap("GameplayControls").Enable();
        playerInput.actions.FindActionMap("InteractionControls").Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.FindActionMap("GameplayControls").Disable();
        playerInput.actions.FindActionMap("InteractionControls").Disable();
    }

    #endregion

    private void Awake()
    {
        SwitchHandheld(EquipableHandhelds[0]);
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
            }
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _CurrentHandheldIndex += 1 * (int)Mathf.Sign(context.ReadValue<float>()); // can't use int, must cast to float.
            _CurrentHandheldIndex = Mathf.Clamp(_CurrentHandheldIndex, 0, EquipableHandhelds.Count - 1); // clamp between 0 to max count - 1.

            SwitchHandheld(EquipableHandhelds[_CurrentHandheldIndex]);
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
