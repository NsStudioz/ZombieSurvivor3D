using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class HandCarry : MonoBehaviour
    {
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
    }
}
