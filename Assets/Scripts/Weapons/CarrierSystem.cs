using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarrierSystem : MonoBehaviour
{

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
        _CurrentHandheldGO.transform.position = Vector3.zero;
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


    // This code block template is for Handheld input actions:
    public void OnAnyAction(InputAction.CallbackContext context)
    {
        if (_CurrentHandheldInterface != null)
        {
            // ADD INPUT ACTION HERE FROM NEW INPUT SYSTEM => INPUT ACTIONS FOR WEAPONS
            // EXAMPLE:
            // _CurrentHandheldInterface.OnAction00(context);
        }
    }

    
    // Weapon/Equipment Switching;
    // This code block template is for Handheld input actions:
    public void OnAnyActionPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _CurrentHandheldIndex += 1 * (int)Mathf.Sign(context.ReadValue<float>()); // can't use int, must cast to float.
            _CurrentHandheldIndex = Mathf.Clamp(_CurrentHandheldIndex, 0, EquipableHandhelds.Count - 1); // clamp between 0 to max count - 1.

            SwitchHandheld(EquipableHandhelds[_CurrentHandheldIndex]);
        }

        if (_CurrentHandheldInterface != null)
        {
            // ADD INPUT ACTION HERE FROM NEW INPUT SYSTEM => INPUT ACTIONS FOR WEAPONS
            // EXAMPLE:
            // _CurrentHandheldInterface.OnAction00(context);
        }
    }

}