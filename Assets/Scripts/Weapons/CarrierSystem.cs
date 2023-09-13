using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarrierSystem : MonoBehaviour, ControlsPC.IGameplayControlsActions
{
    // System for important logic (Math + fire weapon + etc...)

    [SerializeField] Transform RigSocket;
    [SerializeField] Animator RigAnimator;

    public List<HandheldSO> EquipableHandhelds;

    private HandheldSO _CurrentHandheldSO;
    private GameObject _CurrentHandheldGO;
    private IHandheldObject _CurrentHandheldInterface;

    private int _CurrentHandheldIndex;

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

    private void Awake()
    {
        SwitchHandheld(EquipableHandhelds[0]);
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

    public void OnReplaceWeapon(InputAction.CallbackContext context)
    {
        // Using this code block to avoid binding/unbiding from our input system:
        if (_CurrentHandheldInterface != null)
            _CurrentHandheldInterface.OnReplaceWeapon(context);
    }

    #endregion

}
