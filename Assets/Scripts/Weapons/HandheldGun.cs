using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandheldSO;

public class HandheldGun : MonoBehaviour, IHandheldObject
{
    [Header("Main Elements")]
    [SerializeField] private Animator _WeaponAnimator;
    [SerializeField] string handheldName;

    public HandheldTypes HandheldType { get; private set; }
    public FiringModes FiringMode { get; private set; }

    [Header("Attributes")]
    [SerializeField] int ammoCapacity;
    [SerializeField] int fireRate;
    [SerializeField] int fireRateCooldown;
    [SerializeField] float reloadCooldown;

/*    [Header("Audio Files")]
    [SerializeField] AudioClip _EquipAudio;
    [SerializeField] AudioClip _UnequipAudio;
    [SerializeField] AudioClip _FireAudio;
    [SerializeField] AudioClip _FailedFireAudio;*/

    private CarrierSystem carrierSystem;


    // This code block template is for Handheld input actions:
    public void OnAnyAction(InputAction.CallbackContext context)
    {
/*        if (_CurrentHandheldInterface != null)
        {
            // ADD INPUT ACTION HERE FROM NEW INPUT SYSTEM => INPUT ACTIONS FOR WEAPONS
            // EXAMPLE:
            // _CurrentHandheldInterface.OnAction00(context);
        }*/
    }

    // updates our carrier, with that we can use the animations of a specific handheld:
    public void OnAttachedCarrier(CarrierSystem attachedCarrier)
    {
        carrierSystem = attachedCarrier;
        _WeaponAnimator = carrierSystem.GetAnimator();
    }

    public void OnEquip()
    {
        // Example:
        //carrierSystem.GetAnimator().SetTrigger("Equip");
    }

    public void OnUnequip()
    {
        
    }
}
